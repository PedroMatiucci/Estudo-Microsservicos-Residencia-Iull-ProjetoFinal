using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using scb_equipamentos.Controllers;
using scb_equipamentos.Data.TotemDto;
using scb_equipamentos.Db;
using scb_equipamentos.Models;
using scb_equipamentos.Profiles;
using System.ComponentModel.DataAnnotations;

namespace TesteEquipamento.UC14
{
    public class EditarTotemTestes
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
                        mc.AddProfile(new TotemProfile());
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

        [Theory(DisplayName = "Editar Totem Válido")]
        [InlineData("Rio de Janeiro")]
        [InlineData("Espírito Santo")]
        public void EditarTotemValido(string localizacao)
        {
            //arrange
            var options = new DbContextOptionsBuilder<EquipamentoContext>().UseInMemoryDatabase("EquipamentoContext").Options;
            var context = new EquipamentoContext(options);
            var controladorTotem = new TotemController(context, Mapper);

            var totem = new CreateTotemDto();
            totem.Localizacao = "string";

            var totemEditar = new UpdateTotemDto()
            {
                Localizacao = localizacao,
            };

            var actionResult = controladorTotem.CadastrarTotem(totem);

            var okResult = actionResult as OkObjectResult;
            var actualConfigurationJStr = JsonConvert.SerializeObject(okResult.Value);
            var totemRetornada = JsonConvert.DeserializeObject<Totem>(actualConfigurationJStr);
            int numeroTotem = totemRetornada.Numero;

            //act
            var erros = ValidateModel(totemEditar);
            IActionResult resultado;
            if (erros.Count == 0)
            {
                resultado = controladorTotem.AlterarTotem(totemEditar, numeroTotem);
            }
            else
            {
                resultado = new UnprocessableEntityResult();
            }


            //assert
            Assert.IsType<OkObjectResult>(resultado);

        }

        [Theory(DisplayName = "Editar Totem inválido")]
        [InlineData(" ")]
        [InlineData("@Rio de Janeiro")]
        public void EditarTotemInValido(string localizacao)
        {
            //arrange
            var options = new DbContextOptionsBuilder<EquipamentoContext>().UseInMemoryDatabase("EquipamentoContext").Options;
            var context = new EquipamentoContext(options);
            var controladorTotem = new TotemController(context, Mapper);

            var totem = new CreateTotemDto();
            totem.Localizacao = "string";

            var totemEditar = new UpdateTotemDto()
            {
                Localizacao = localizacao,
            };

            var actionResult = controladorTotem.CadastrarTotem(totem);

            var okResult = actionResult as OkObjectResult;
            var actualConfigurationJStr = JsonConvert.SerializeObject(okResult.Value);
            var totemRetornada = JsonConvert.DeserializeObject<Totem>(actualConfigurationJStr);
            int numeroTotem = totemRetornada.Numero;

            //act
            var erros = ValidateModel(totemEditar);
            IActionResult resultado;
            if (erros.Count == 0)
            {
                resultado = controladorTotem.AlterarTotem(totemEditar, numeroTotem);
            }
            else
            {
                resultado = new UnprocessableEntityResult();
            }


            //assert
            Assert.IsType<UnprocessableEntityResult>(resultado);

        }

        [Fact(DisplayName = "Editar Totem inexistente")]
        public void EditarTotemInexistente()
        {
            //arrange
            var options = new DbContextOptionsBuilder<EquipamentoContext>().UseInMemoryDatabase("EquipamentoContext").Options;
            var context = new EquipamentoContext(options);
            var controladorTotem = new TotemController(context, Mapper);

            var totemEditar = new UpdateTotemDto()
            {
                Localizacao = "string",
            };

            //act
            var erros = ValidateModel(totemEditar);
            IActionResult resultado;
            if (erros.Count == 0)
            {
                resultado = controladorTotem.AlterarTotem(totemEditar, 10);
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
