using AutoMapper;
using scb_equipamentos.Data.BicicletaDto;
using scb_equipamentos.Db;
using scb_equipamentos.Models;
using scb_equipamentos.Models.Enum;

namespace scb_equipamentos.Daos
{
    public class BicicletaDAO
    {
        private EquipamentoContext _context;
        private IMapper _mapper;

        public BicicletaDAO(EquipamentoContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void AdicionaBicicleta(Bicicleta bicicleta)
        {
            _context.Bicicletas.Add(bicicleta);
            _context.SaveChanges();
        }

        public void AtualizaBicicleta(Bicicleta bicicleta)
        {
            _context.Bicicletas.Update(bicicleta);
            _context.SaveChanges();
        }

        public void RemoveBicicleta(Bicicleta bicicleta)
        {
            bicicleta.StatusEnum = StatusBicicleta.EXCLUIDA;
            _context.SaveChanges();
        }

        public Bicicleta RetornaBicicletaPorId(int id)
        {
            return _context.Bicicletas.Where(b => b.Status != "EXCLUIDA").FirstOrDefault(b => b.Id == id);
        }

        public List<ReturnBicicletaDto> RetornaBicicletas()
        {
            return _mapper.Map<List<Bicicleta>, List<ReturnBicicletaDto>>(_context.Bicicletas.Where(b => b.Status != "EXCLUIDA").ToList());
        }
    }
}
