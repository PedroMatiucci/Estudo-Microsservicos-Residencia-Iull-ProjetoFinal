using AutoMapper;
using Microsoft.EntityFrameworkCore;
using scb_equipamentos.Db;
using scb_equipamentos.Models.Enum;
using scb_equipamentos.Models;

namespace scb_equipamentos.Daos
{
    public class TotemDAO
    {
        private EquipamentoContext _context;
        private IMapper _mapper;

        public TotemDAO(EquipamentoContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void AdicionaTotem(Totem totem)
        {
            _context.Totems.Add(totem);
            _context.SaveChanges();
        }

        public void AtualizaTotem(Totem totem)
        {
            _context.Totems.Update(totem);
            _context.SaveChanges();
        }

        public Totem RetornaTotemPorNumero(int numero)
        {
            return _context.Totems.FirstOrDefault(t => t.Numero == numero);
        }

        public Totem RetornaTotemComTrancasPorNumero(int numero)
        {
            return _context.Totems.Include(t => t.Trancas).FirstOrDefault(t => t.Numero == numero);
        }

        public List<Totem> RetornaTotems()
        {
            return _context.Totems.ToList();
        }
    }
}
