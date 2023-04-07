using AutoMapper;
using Microsoft.EntityFrameworkCore;
using scb_equipamentos.Data.TrancaDto;
using scb_equipamentos.Db;
using scb_equipamentos.Extensions;
using scb_equipamentos.Models;
using scb_equipamentos.Models.Enum;

namespace scb_equipamentos.Daos
{
    public class TrancaDAO
    {
        private EquipamentoContext _context;
        private IMapper _mapper;

        public TrancaDAO(EquipamentoContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void AdicionaTranca(Tranca tranca)
        {
            _context.Trancas.Add(tranca);
            _context.SaveChanges();
        }

        public void AtualizaTranca(Tranca tranca) 
        { 
            _context.Trancas.Update(tranca);
            _context.SaveChanges();
        }

        public void RemoveTranca(Tranca tranca)
        {
            tranca.StatusEnum = StatusTranca.EXCLUIDA;
            _context.SaveChanges();
        }

        public Tranca RetornaTrancaPorId(int id) 
        {
            return _context.Trancas.FirstOrDefault(t => t.Id == id);
        }

        public Tranca RetornaTrancaComBicicletaPorId(int id)
        {
            return _context.Trancas.Include(t => t.Bicicleta).FirstOrDefault(t => t.Id == id);
        }

        public List<ReturnTrancaDto> RetornaTrancas() 
        { 
            return _mapper.Map<List<Tranca>, List<ReturnTrancaDto>>(_context.Trancas.Where(t => t.Status != StatusTranca.EXCLUIDA.ParaStringTranca()).ToList());
        }
    }
}
