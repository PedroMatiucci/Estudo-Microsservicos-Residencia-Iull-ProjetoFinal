using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using scb_equipamentos.Daos;
using scb_equipamentos.Data.BicicletaDto;
using scb_equipamentos.Data.TrancaDto;
using scb_equipamentos.Db;
using scb_equipamentos.Erros;
using scb_equipamentos.Extensions;
using scb_equipamentos.Factory;
using scb_equipamentos.Models;
using scb_equipamentos.Models.Enum;
using scb_equipamentos.Services;
using System.Net;

namespace scb_equipamentos.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TrancaController : ControllerBase
    {
        private IMapper _mapper;
        private TrancaDAO trancaDAO;
        private BicicletaDAO bicicletaDAO;
        private TotemDAO totemDAO;

        public TrancaController(EquipamentoContext context, IMapper mapper)
        {
            _mapper = mapper;
            trancaDAO = new TrancaDAO(context, _mapper);
            bicicletaDAO = new BicicletaDAO(context, _mapper);
            totemDAO = new TotemDAO(context, _mapper);
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<ReturnTrancaDto>))]
        public IActionResult RecuperarTrancas()
        {
            List<ReturnTrancaDto> trancas = trancaDAO.RetornaTrancas();
            return Ok(trancas);
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(ReturnTrancaDto))]
        [ProducesResponseType(422, Type = typeof(Erro))]
        public IActionResult CadastrarTranca([FromBody] CreateTrancaDto trancaDto)
        {
            Tranca tranca = _mapper.Map<Tranca>(trancaDto);
            tranca.StatusEnum = StatusTranca.NOVA;
            trancaDAO.AdicionaTranca(tranca);
            ReturnTrancaDto returnTranca = _mapper.Map<ReturnTrancaDto>(tranca);
            return Ok(returnTranca);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(ReturnTrancaDto))]
        [ProducesResponseType(404, Type = typeof(Erro))]
        public IActionResult RecuperarTrancaPorId(int id)
        {
            Tranca tranca = trancaDAO.RetornaTrancaPorId(id);
            if (tranca != null && !tranca.isExcluida()) {
                ReturnTrancaDto returnTranca = _mapper.Map<ReturnTrancaDto>(tranca);
                return Ok(returnTranca);
            };
            return NotFound();
        }

        [HttpPut("{id}")]
        [ProducesResponseType(200, Type = typeof(ReturnTrancaDto))]
        [ProducesResponseType(404, Type = typeof(Erro))]
        [ProducesResponseType(422, Type = typeof(Erro))]
        public IActionResult AlterarTranca([FromBody] UpdateTrancaDto trancaDto, int id)
        {
            Tranca tranca = trancaDAO.RetornaTrancaPorId(id);
            if (tranca != null && !tranca.isExcluida()) {
                if (trancaDto.Status == "EXCLUIDA") return UnprocessableEntity();

                trancaDAO.AtualizaTranca(_mapper.Map(trancaDto, tranca));
                ReturnTrancaDto returnTranca = _mapper.Map<ReturnTrancaDto>(tranca);
                return Ok(returnTranca);
            }
            return NotFound();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404, Type = typeof(Erro))]
        [ProducesResponseType(422, Type = typeof(Erro))]
        public IActionResult RemoverTranca(int id)
        {
            Tranca tranca = trancaDAO.RetornaTrancaPorId(id);
            if (tranca != null && !tranca.isExcluida()) {
                if (tranca.Bicicleta == null)
                {
                    trancaDAO.RemoveTranca(tranca);
                    return Ok();
                }
                return UnprocessableEntity();
            }
            return NotFound();
        }

        [HttpGet("{id}/bicicleta")]
        [ProducesResponseType(200, Type = typeof(ReturnBicicletaDto))]
        [ProducesResponseType(404, Type = typeof(Erro))]
        [ProducesResponseType(422, Type = typeof(Erro))]
        public IActionResult ObterBicicletaPorTranca(int id)
        {
            Tranca tranca = trancaDAO.RetornaTrancaComBicicletaPorId(id);
            if (tranca != null && tranca.Bicicleta != null)
            {
                ReturnBicicletaDto returnBicicleta = _mapper.Map<ReturnBicicletaDto>(tranca.Bicicleta);
                return Ok(returnBicicleta);
            }
            return NotFound();
        }

        [HttpPost("{id}/status/{acao}")]
        [ProducesResponseType(200, Type = typeof(ReturnTrancaDto))]
        [ProducesResponseType(404, Type = typeof(Erro))]
        [ProducesResponseType(422, Type = typeof(Erro))]
        public IActionResult AlterarStatusTranca(int id, string acao)
        {
            Tranca tranca = trancaDAO.RetornaTrancaPorId(id);
            if (tranca != null && !tranca.isExcluida())
            {
                if (Enum.IsDefined(typeof(StatusTranca), acao)) {
                    tranca.StatusEnum = acao.ParaValorTranca();
                    trancaDAO.AtualizaTranca(tranca);
                    ReturnTrancaDto returnTranca = _mapper.Map<ReturnTrancaDto>(tranca);
                    return Ok(returnTranca);
                }
                return UnprocessableEntity();
            }
            return NotFound();
        }

        [HttpPost("{id}/trancar")]
        [ProducesResponseType(200, Type = typeof(ReturnTrancaDto))]
        [ProducesResponseType(404, Type = typeof(Erro))]
        [ProducesResponseType(422, Type = typeof(Erro))]
        public IActionResult TrancarTranca([FromBody] TrancamentoTrancaDto? trancamentoTrancaDto, int id)
        {
            Tranca tranca = trancaDAO.RetornaTrancaPorId(id);
            if (tranca != null && !tranca.isExcluida())
            {
                if (tranca.StatusEnum != StatusTranca.OCUPADA)
                {
                    if (trancamentoTrancaDto.BicicletaId != null)
                    {
                        Bicicleta bicicleta = bicicletaDAO.RetornaBicicletaPorId((int) trancamentoTrancaDto.BicicletaId);

                        if (bicicleta == null) { return NotFound(); }
                        tranca.Bicicleta = bicicleta;
                        tranca.Bicicleta.StatusEnum = StatusBicicleta.DISPONIVEL;
                    }

                    tranca.StatusEnum = StatusTranca.OCUPADA;
                    trancaDAO.AtualizaTranca(tranca);
                    ReturnTrancaDto returnTranca = _mapper.Map<ReturnTrancaDto>(tranca);
                    return Ok(returnTranca);
                }
                return UnprocessableEntity();
            }
            return NotFound();
        }

        [HttpPost("{id}/destrancar")]
        [ProducesResponseType(200, Type = typeof(ReturnTrancaDto))]
        [ProducesResponseType(404, Type = typeof(Erro))]
        [ProducesResponseType(422, Type = typeof(Erro))]
        public IActionResult DestrancarTranca([FromBody] TrancamentoTrancaDto? trancamentoTrancaDto, int id)
        {
            Tranca tranca = trancaDAO.RetornaTrancaPorId(id);
            if (tranca != null && !tranca.isExcluida())
            {
                if (tranca.StatusEnum != StatusTranca.LIVRE)
                {
                    if (trancamentoTrancaDto.BicicletaId != null)
                    {
                        Bicicleta bicicleta = bicicletaDAO.RetornaBicicletaPorId((int) trancamentoTrancaDto.BicicletaId);

                        if (bicicleta == null) { return NotFound(); }
                        tranca.Bicicleta = bicicleta;
                        tranca.Bicicleta.StatusEnum = StatusBicicleta.EM_USO;
                    }

                    tranca.StatusEnum = StatusTranca.LIVRE;
                    trancaDAO.AtualizaTranca(tranca);
                    ReturnTrancaDto returnTranca = _mapper.Map<ReturnTrancaDto>(tranca);
                    return Ok(returnTranca);
                }
                return UnprocessableEntity();
            }
            return NotFound();
        }

        [HttpPost("integrarNaRede")]
        [ProducesResponseType(200)]
        [ProducesResponseType(422, Type = typeof(Erro))]
        public async Task<IActionResult> IntegrarTranca([FromBody] IntegrarTrancaDto integrarTrancaDto)
        {
            Totem totem = totemDAO.RetornaTotemComTrancasPorNumero(integrarTrancaDto.IdTotem);
            Tranca tranca = trancaDAO.RetornaTrancaPorId(integrarTrancaDto.IdTranca);
            Funcionario funcionario = AluguelAPI.RetornaFuncionarioPorId(integrarTrancaDto.IdFuncionario);

            if (tranca != null && !tranca.isExcluida() && totem != null && funcionario != null)
            {
                if (tranca.StatusEnum == StatusTranca.NOVA || tranca.StatusEnum == StatusTranca.EM_REPARO)
                {
                    NovoEmail? novoEmail = EmailFactory.Create(funcionario.Email, 
                        "Tranca Incluida", 
                        $"Tranca de Numero: {tranca.Numero} foi integrada na rede com sucesso.");
                    var statuscode = await ExternoAPI.EnviarEmail(novoEmail);
                    if (statuscode == HttpStatusCode.NotFound) return NotFound();
                    if (statuscode == HttpStatusCode.UnprocessableEntity) return UnprocessableEntity();

                    tranca.StatusEnum = StatusTranca.LIVRE;
                    totem.Trancas.Add(tranca);
                    totemDAO.AtualizaTotem(totem);
                    return Ok();
                }
                return UnprocessableEntity();
            }
            return NotFound();
        }

        [HttpPost("retirarDaRede")]
        [ProducesResponseType(200)]
        [ProducesResponseType(422, Type = typeof(Erro))]
        public async Task<IActionResult> RetirarTranca([FromBody] RetirarTrancaDto retirarTrancaDto)
        {
            Totem totem = totemDAO.RetornaTotemComTrancasPorNumero(retirarTrancaDto.IdTotem);
            Tranca tranca = trancaDAO.RetornaTrancaPorId(retirarTrancaDto.IdTranca);
            Funcionario funcionario = AluguelAPI.RetornaFuncionarioPorId(retirarTrancaDto.IdFuncionario);

            if (tranca != null && !tranca.isExcluida() && totem != null && funcionario != null)
            {
                if (tranca.StatusEnum == StatusTranca.REPARO_SOLICITADO && tranca.Bicicleta == null && 
                    (retirarTrancaDto.StatusAcaoReparador == "EM_REPARO" || retirarTrancaDto.StatusAcaoReparador == "APOSENTADA"))
                {
                    NovoEmail? novoEmail = EmailFactory.Create(funcionario.Email,
                        "Tranca Retirada",
                        $"Tranca de Numero: {tranca.Numero} retirada para: {retirarTrancaDto.StatusAcaoReparador}.");
                    var statuscode = await ExternoAPI.EnviarEmail(novoEmail);
                    if (statuscode == HttpStatusCode.NotFound) return NotFound();
                    if (statuscode == HttpStatusCode.UnprocessableEntity) return UnprocessableEntity();

                    tranca.StatusEnum = retirarTrancaDto.StatusAcaoReparador.ParaValorTranca();
                    totem.Trancas.Remove(tranca);
                    totemDAO.AtualizaTotem(totem);
                    return Ok();
                }
                return UnprocessableEntity();
            }
            return NotFound();
        }
    }
}
