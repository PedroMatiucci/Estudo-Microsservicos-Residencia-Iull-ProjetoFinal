using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using scb_equipamentos.Daos;
using scb_equipamentos.Data.BicicletaDto;
using scb_equipamentos.Data.TotemDto;
using scb_equipamentos.Db;
using scb_equipamentos.Extensions;
using scb_equipamentos.Models;
using scb_equipamentos.Models.Enum;
using System.Collections.Generic;
using System.Diagnostics;

namespace scb_equipamentos.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TotemController : ControllerBase
    {
        private EquipamentoContext _context;
        private IMapper _mapper;
        private TotemDAO totemDAO;

        public TotemController(EquipamentoContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            totemDAO = new TotemDAO(context, _mapper);
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(Totem))]
        [ProducesResponseType(422)]
        public IActionResult CadastrarTotem([FromBody] CreateTotemDto totemDto)
        {
            Totem totem = _mapper.Map<Totem>(totemDto);
            totem.Trancas = new List<Tranca>();
            totemDAO.AdicionaTotem(totem);
            return Ok(totem);
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(Totem))]
        public IActionResult RecuperarTotems()
        {
            return Ok(_context.Totems.Include(t => t.Trancas).ThenInclude(tr => tr.Bicicleta));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(200, Type = typeof(Tranca))]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        public IActionResult AlterarTotem([FromBody] UpdateTotemDto totemDto, int id)
        {
            Totem totem = _context.Totems.Include(t => t.Trancas).FirstOrDefault(t => t.Numero == id);
            if (totem != null)
            {
                _mapper.Map(totemDto, totem);
                _context.SaveChanges();
                return Ok(totem);
            }
            return NotFound();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200, Type = typeof(Tranca))]
        [ProducesResponseType(404)]
        public IActionResult RemoverTotem(int id)
        {
            Totem totem = _context.Totems.Include(t => t.Trancas).FirstOrDefault(t => t.Numero == id);
            if (totem != null && totem.Trancas.Count == 0)
            {
                _context.Remove(totem);
                _context.SaveChanges();
                return Ok();
            }
            return NotFound();
        }

        [HttpGet("{id}/trancas")]
        [ProducesResponseType(200, Type = typeof(Totem))]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        public IActionResult ObterTrancasPorTotem(int id)
        {
            Totem totem = _context.Totems.Include(t => t.Trancas).ThenInclude(tr => tr.Bicicleta).FirstOrDefault(t => t.Numero == id);
            if (totem != null && totem.Trancas != null)
            {
                return Ok(totem.Trancas);
            }
            return NotFound();
        }

        [HttpGet("{id}/bicicletas")]
        [ProducesResponseType(200, Type = typeof(Totem))]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        public IActionResult ObterBicicletasPorTotem(int id)
        {
            Totem totem = _context.Totems.Include(t => t.Trancas).ThenInclude(tr => tr.Bicicleta).FirstOrDefault(t => t.Numero == id);
            if (totem != null && totem.Trancas != null)
            {
                List<Bicicleta> bicicletas = new List<Bicicleta>();
                foreach (Tranca t in totem.Trancas) bicicletas.Add(t.Bicicleta);

                return Ok(bicicletas);
            }
            return NotFound();
        }

    }
}
