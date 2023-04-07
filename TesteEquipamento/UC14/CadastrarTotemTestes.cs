using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using scb_equipamentos.Controllers;
using scb_equipamentos.Data.TotemDto;
using scb_equipamentos.Db;
using scb_equipamentos.Models;
using scb_equipamentos.Profiles;
using System.ComponentModel.DataAnnotations;

namespace TesteEquipamento.UC14
{
    public class CadastrarTotemTestes
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

        [Theory(DisplayName = "Cadastro de Totem Válido")]
        [InlineData("Rio de Janeiro")]
        [InlineData("São Paulo")]
        public void CadastrarTotemValido(string localizacao)
        {
            //arrange
            var options = new DbContextOptionsBuilder<EquipamentoContext>().UseInMemoryDatabase("EquipamentoContext").Options;
            var context = new EquipamentoContext(options);
            var controladorTotem = new TotemController(context, Mapper);

            var totem = new CreateTotemDto();
            totem.Localizacao = localizacao;

            //act
            var erros = ValidateModel(totem);
            IActionResult resultado;
            if (erros.Count == 0)
            {
                resultado = controladorTotem.CadastrarTotem(totem);
            }
            else
            {
                resultado = new UnprocessableEntityResult();
            }


            //assert
            Assert.IsType<OkObjectResult>(resultado);

        }

        [Theory(DisplayName = "Cadastro de Totem inválido")]
        [InlineData(" ")]
        [InlineData("@Rio de Janeiro")]
        public void CadastrarTotemInvalido(string localizacao)
        {
            //arrange
            var options = new DbContextOptionsBuilder<EquipamentoContext>().UseInMemoryDatabase("EquipamentoContext").Options;
            var context = new EquipamentoContext(options);
            var controladorTotem = new TotemController(context, Mapper);

            var totem = new CreateTotemDto();
            totem.Localizacao = localizacao;

            //act
            var erros = ValidateModel(totem);
            IActionResult resultado;
            if (erros.Count == 0)
            {
                resultado = controladorTotem.CadastrarTotem(totem);
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
