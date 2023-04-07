using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using scb_equipamentos.Controllers;
using scb_equipamentos.Data.BicicletaDto;
using scb_equipamentos.Db;
using scb_equipamentos.Profiles;
using System.ComponentModel.DataAnnotations;

namespace TesteEquipamento.UC10
{
    public class CadastrarBicicletaTestes
    {
        private static IMapper _mapper;

        public static IMapper Mapper
        {
            get
            {
                if (_mapper == null)
                {
                    // Auto Mapper Configurations
                    var mappingConfig = new MapperConfiguration(mc =>
                    {
                        mc.AddProfile(new BicicletaProfile());
                    });
                    IMapper mapper = mappingConfig.CreateMapper();
                    _mapper = mapper;
                }
                return _mapper;

            }
        }

        private IList<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var ctx = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, ctx, validationResults, true);
            return validationResults;
        }

        [Theory]
        [InlineData("string", "string", 2023)]
        [InlineData("string", "string", 0)]
        public void CadastrarBicicletaValida(string marca, string modelo, int anoFabricacao)
        {
            //arrange
            var options = new DbContextOptionsBuilder<EquipamentoContext>().UseInMemoryDatabase("EquipamentoContext").Options;
            var context = new EquipamentoContext(options);
            var controladorBicicleta = new BicicletaController(context, Mapper);

            var bicicleta = new CreateBicicletaDto();
            bicicleta.Marca = marca;
            bicicleta.Modelo = modelo;
            bicicleta.AnoFabricacao = anoFabricacao;

            //act
            var erros = ValidateModel(bicicleta);
            IActionResult resultado;
            if (erros.Count == 0)
            {
                resultado = controladorBicicleta.CadastrarBicicicleta(bicicleta);
            }
            else
            {
                resultado = new UnprocessableEntityResult();
            }


            //assert
            Assert.IsType<OkObjectResult>(resultado);
        }


        [Theory]
        [InlineData(" ", "string", 2023)]
        [InlineData(null, "string", 2023)]
        [InlineData("string", " ", 2023)]
        [InlineData("string", null, 2023)]
        [InlineData("string", "string", -1)]
        public void CadastrarBicicletaComDadosInvalidos(string marca, string modelo, int anoFabricacao)
        {
            //arrange
            var options = new DbContextOptionsBuilder<EquipamentoContext>().UseInMemoryDatabase("EquipamentoContext").Options;
            var context = new EquipamentoContext(options);
            var controladorBicicleta = new BicicletaController(context, Mapper);

            var bicicleta = new CreateBicicletaDto();
            bicicleta.Marca = marca;
            bicicleta.Modelo = modelo;
            bicicleta.AnoFabricacao = anoFabricacao;

            //act
            var erros = ValidateModel(bicicleta);
            IActionResult resultado;
            if (erros.Count == 0)
            {
                resultado = controladorBicicleta.CadastrarBicicicleta(bicicleta);
            }
            else
            {
                resultado = new UnprocessableEntityResult();
            }

            //assert
            Assert.IsType<UnprocessableEntityResult>(resultado);

        }

    }
}
