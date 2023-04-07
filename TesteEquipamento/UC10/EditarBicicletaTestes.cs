using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using scb_equipamentos.Controllers;
using scb_equipamentos.Data.BicicletaDto;
using scb_equipamentos.Db;
using scb_equipamentos.Models;
using scb_equipamentos.Profiles;
using System.ComponentModel.DataAnnotations;

namespace TesteEquipamento.UC10
{
    public class EditarBicicletaTestes
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
        public void EditarBicicletaValido(string marca, string modelo, int ano)
        {
            //arrange
            var options = new DbContextOptionsBuilder<EquipamentoContext>().UseInMemoryDatabase("EquipamentoContext").Options;
            var context = new EquipamentoContext(options);
            var controladorBicicleta = new BicicletaController(context, Mapper);

            var bicicleta = new CreateBicicletaDto();
            bicicleta.Marca = "string";
            bicicleta.Modelo = "string";
            bicicleta.AnoFabricacao = 2023;

            var bicicletaEditar = new UpdateBicicletaDto()
            {
                Marca = marca,
                Modelo = modelo,
                AnoFabricacao = ano,
            };

            var actionResult = controladorBicicleta.CadastrarBicicicleta(bicicleta);

            var okResult = actionResult as OkObjectResult;
            var actualConfigurationJStr = JsonConvert.SerializeObject(okResult.Value);
            var bicicletaRetornada = JsonConvert.DeserializeObject<Bicicleta>(actualConfigurationJStr);
            int idBicicleta = bicicletaRetornada.Id;

            //act
            var erros = ValidateModel(bicicletaEditar);
            IActionResult resultado;
            if (erros.Count == 0)
            {
                resultado = controladorBicicleta.AlterarBicicleta(idBicicleta, bicicletaEditar);
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
        public void EditarBicicletaValoresInvalidos(string marca, string modelo, int ano)
        {
            //arrange
            var options = new DbContextOptionsBuilder<EquipamentoContext>().UseInMemoryDatabase("EquipamentoContext").Options;
            var context = new EquipamentoContext(options);
            var controladorBicicleta = new BicicletaController(context, Mapper);

            var bicicleta = new CreateBicicletaDto();
            bicicleta.Marca = "string";
            bicicleta.Modelo = "string";
            bicicleta.AnoFabricacao = 2023;

            var bicicletaEditar = new UpdateBicicletaDto()
            {
                Marca = marca,
                Modelo = modelo,
                AnoFabricacao = ano,
            };

            var actionResult = controladorBicicleta.CadastrarBicicicleta(bicicleta);

            var okResult = actionResult as OkObjectResult;
            var actualConfigurationJStr = JsonConvert.SerializeObject(okResult.Value);
            var bicicletaRetornada = JsonConvert.DeserializeObject<Bicicleta>(actualConfigurationJStr);
            int idBicicleta = bicicletaRetornada.Id;

            //act
            var erros = ValidateModel(bicicletaEditar);
            IActionResult resultado;
            if (erros.Count == 0)
            {
                resultado = controladorBicicleta.AlterarBicicleta(idBicicleta, bicicletaEditar);
            }
            else
            {
                resultado = new UnprocessableEntityResult();
            }


            //assert
            Assert.IsType<UnprocessableEntityResult>(resultado);
        }
        [Fact]
        public void EditarBicicletaBicicletaInexistente()
        {
            //arrange
            var options = new DbContextOptionsBuilder<EquipamentoContext>().UseInMemoryDatabase("EquipamentoContext").Options;
            var context = new EquipamentoContext(options);
            var controladorBicicleta = new BicicletaController(context, Mapper);

            var bicicletaEditar = new UpdateBicicletaDto()
            {
                Marca = "string",
                Modelo = "string",
                AnoFabricacao = 2023,
            };

            //act
            var erros = ValidateModel(bicicletaEditar);
            IActionResult resultado;
            if (erros.Count == 0)
            {
                resultado = controladorBicicleta.AlterarBicicleta(20, bicicletaEditar);
            }
            else
            {
                resultado = new UnprocessableEntityResult();
            }


            //assert
            Assert.IsType<NotFoundResult>(resultado);
        }

    }
}
