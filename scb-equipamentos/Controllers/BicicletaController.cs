using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using scb_equipamentos.Daos;
using scb_equipamentos.Data.BicicletaDto;
using scb_equipamentos.Db;
using scb_equipamentos.Erros;
using scb_equipamentos.Extensions;
using scb_equipamentos.Factory;
using scb_equipamentos.Models;
using scb_equipamentos.Models.Enum;
using scb_equipamentos.Services;
using System.Diagnostics;
using System.Net;

namespace scb_equipamentos.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BicicletaController : ControllerBase
    {

        private EquipamentoContext _context;
        private IMapper _mapper;
        private Random rnd;
        private BicicletaDAO bicicletaDAO;
        private TrancaDAO trancaDAO;

        public BicicletaController(EquipamentoContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            rnd = new Random();
            bicicletaDAO = new BicicletaDAO(context, _mapper);
            trancaDAO = new TrancaDAO(context, _mapper);
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(ReturnBicicletaDto))]
        [ProducesResponseType(422, Type = typeof(Erro))]
        public IActionResult CadastrarBicicicleta([FromBody] CreateBicicletaDto bicicletaDto)
        {
            Bicicleta bicicleta = _mapper.Map<Bicicleta>(bicicletaDto);
            bicicleta.StatusEnum = StatusBicicleta.NOVA;
            bicicleta.Numero = rnd.Next();
            bicicletaDAO.AdicionaBicicleta(bicicleta);
            ReturnBicicletaDto returnBicicleta = _mapper.Map<ReturnBicicletaDto>(bicicleta);
            return Ok(returnBicicleta);
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ReturnBicicletaDto))]
        public IActionResult RecuperarBicicletas()
        {
            return Ok(bicicletaDAO.RetornaBicicletas());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(ReturnBicicletaDto))]
        [ProducesResponseType(404, Type = typeof(Erro))]
        public IActionResult RecuperarBicicletasPorId(int id)
        {
            Bicicleta bicicleta = bicicletaDAO.RetornaBicicletaPorId(id);
            if (bicicleta != null)
            {
                ReturnBicicletaDto returnBicicleta = _mapper.Map<ReturnBicicletaDto>(bicicleta);
                return Ok(returnBicicleta);
            }
            return NotFound();
        }

        [HttpPut("{id}")]
        [ProducesResponseType(200, Type = typeof(ReturnBicicletaDto))]
        [ProducesResponseType(404, Type=typeof(Erro))]
        [ProducesResponseType(422, Type = typeof(Erro))]
        public IActionResult AlterarBicicleta(int id, [FromBody] UpdateBicicletaDto bicicletaNova)
        {
            Bicicleta bicicleta = bicicletaDAO.RetornaBicicletaPorId(id);
            if (bicicleta == null) return NotFound();
            bicicletaDAO.AtualizaBicicleta(_mapper.Map(bicicletaNova, bicicleta));
            ReturnBicicletaDto returnBicicleta = _mapper.Map<ReturnBicicletaDto>(bicicleta);
            return Ok(returnBicicleta);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200, Type = typeof(Erro))]
        [ProducesResponseType(404, Type = typeof(Erro))]
        public IActionResult DeletarBicicleta(int id)
        {
            Bicicleta bicicleta = bicicletaDAO.RetornaBicicletaPorId(id);
            if (bicicleta != null)
            {
                if (bicicleta.StatusEnum == StatusBicicleta.APOSENTADA && bicicleta.Tranca == null)
                {
                    bicicletaDAO.RemoveBicicleta(bicicleta);
                    return Ok();
                }
            }
            return NotFound();
        }

        [HttpPost("{id}/status/{status}")]
        [ProducesResponseType(200, Type = typeof(ReturnBicicletaDto))]
        [ProducesResponseType(404, Type = typeof(Erro))]
        [ProducesResponseType(422, Type = typeof(Erro))]
        public IActionResult AtualizarStatus(int id, string status)
        {
            Bicicleta bicicleta = bicicletaDAO.RetornaBicicletaPorId(id);
            if (bicicleta == null) return NotFound();
            if (!Enum.IsDefined(typeof(StatusBicicleta), status)) return UnprocessableEntity();
            bicicleta.StatusEnum = status.ParaValorBicicleta();
            bicicletaDAO.AtualizaBicicleta(bicicleta);
            ReturnBicicletaDto returnBicicleta = _mapper.Map<ReturnBicicletaDto>(bicicleta);
            return Ok(returnBicicleta);
        }

        [HttpPost("integrarNaRede")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404, Type = typeof(Erro))]
        [ProducesResponseType(422, Type = typeof(Erro))]
        public async Task<IActionResult> IntegrarBicicletaNaRede([FromBody] IntegrarBicicletaDto integrarBicicletaDto)
        {
            Bicicleta? bicicleta = bicicletaDAO.RetornaBicicletaPorId(integrarBicicletaDto.IdBicicleta);
            Tranca? tranca = trancaDAO.RetornaTrancaPorId(integrarBicicletaDto.IdTranca);
            Funcionario? funcionario = AluguelAPI.RetornaFuncionarioPorId(integrarBicicletaDto.IdFuncionario);
            if (bicicleta == null || tranca == null || funcionario == null) return UnprocessableEntity();
            if ((bicicleta.StatusEnum == StatusBicicleta.NOVA || bicicleta.StatusEnum == StatusBicicleta.EM_REPARO) && (tranca.StatusEnum == StatusTranca.LIVRE))
            {
                NovoEmail? novoEmail = EmailFactory.Create(funcionario.Email, "Bicicleta Incluida", $"Bicicleta de Numero: {bicicleta.Numero} foi integrada Na rede com sucesso" );
                var statuscode = await ExternoAPI.EnviarEmail(novoEmail);
                if (statuscode == HttpStatusCode.NotFound) return NotFound();
                if (statuscode == HttpStatusCode.UnprocessableEntity) return UnprocessableEntity();
                bicicleta.Tranca = tranca;
                tranca.Bicicleta = bicicleta;
                bicicleta.StatusEnum = StatusBicicleta.DISPONIVEL;
                tranca.StatusEnum = StatusTranca.OCUPADA;
                _context.SaveChanges();
                return Ok();
            }
            return UnprocessableEntity();
        }

        [HttpPost("retirarDaRede")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404, Type = typeof(Erro))]
        [ProducesResponseType(422, Type = typeof(Erro))]
        public async Task<IActionResult> RetirarBicicletaNaRede([FromBody] RetirarBicicletaDto retirarBicicletaDto)
        {
            Bicicleta? bicicleta = _context.Bicicletas.Include(b => b.Tranca).FirstOrDefault(b => b.Id == retirarBicicletaDto.IdBicicleta);
            Tranca? tranca = _context.Trancas.Include(t =>t.Bicicleta).FirstOrDefault(t => t.Id == retirarBicicletaDto.IdTranca);
            Funcionario? funcionario = AluguelAPI.RetornaFuncionarioPorId(retirarBicicletaDto.IdFuncionario);
            if (bicicleta == null || tranca == null) return NotFound();
            if ((bicicleta.StatusEnum == StatusBicicleta.REPARO_SOLICITADO && bicicleta.Tranca != null))
            {
                if (retirarBicicletaDto.StatusAcaoReparador == "EM_REPARO" || retirarBicicletaDto.StatusAcaoReparador == "APOSENTADA")
                {
                    NovoEmail? novoEmail = EmailFactory.Create(funcionario.Email, $"Bicicleta Retirada Para: {retirarBicicletaDto.StatusAcaoReparador}", "BicicletaRetirada");
                    var statuscode = await ExternoAPI.EnviarEmail(novoEmail);
                    if (statuscode == HttpStatusCode.NotFound) return NotFound();
                    if (statuscode == HttpStatusCode.UnprocessableEntity) return UnprocessableEntity();
                    bicicleta.StatusEnum = retirarBicicletaDto.StatusAcaoReparador.ParaValorBicicleta();
                    tranca.Bicicleta = null;
                    bicicleta.Tranca = null;
                    tranca.StatusEnum = StatusTranca.LIVRE;
                    _context.SaveChanges();
                    return Ok();
                }
            }
            return UnprocessableEntity();
        }
    }
}
