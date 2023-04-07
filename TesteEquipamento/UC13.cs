using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using scb_equipamentos.Controllers;
using scb_equipamentos.Data.TrancaDto;
using scb_equipamentos.Db;
using scb_equipamentos.Profiles;
using System.ComponentModel.DataAnnotations;
using TesteEquipamento.Dados;

namespace TesteEquipamento
{
    public class UC13
    {
        private TrancaController trancaController;

        public UC13()
        {
            var options = new DbContextOptionsBuilder<EquipamentoContext>().UseInMemoryDatabase("EquipamentoContext").Options;
            var context = new EquipamentoContext(options);

            IMapper mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new TrancaProfile())));

            trancaController = new TrancaController(context, mapper);
        }

        //https://stackoverflow.com/a/4331964
        private IList<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var ctx = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, ctx, validationResults, true);
            return validationResults;
        }

        [Fact(DisplayName = "CT01")]
        public void IncluiTrancaValida()
        {
            //Arrange
            CreateTrancaDto tranca = new CreateTrancaDto()
            {
                Numero = 1,
                Localizacao = "Rio de Janeiro",
                AnoFabricacao = 2020,
                Modelo = "Teste"
            };

            //Act
            var erros = ValidateModel(tranca);
            IActionResult result = trancaController.CadastrarTranca(tranca);

            //Assert
            Assert.Equal(0, erros.Count);
            Assert.IsType<OkObjectResult>(result);
        }

        [Theory(DisplayName = "CT02")]
        [MemberData(nameof(TrancaDados.IncluirTrancasInvalidasDados), MemberType = typeof(TrancaDados))]
        public void IncluirTrancasInvalidas(int numero, string localizacao, int ano, string modelo)
        {
            //Arrange
            CreateTrancaDto tranca = new CreateTrancaDto()
            {
                Numero = numero,
                Localizacao = localizacao,
                AnoFabricacao = ano,
                Modelo = modelo
            };

            //Act
            var erros = ValidateModel(tranca);
            IActionResult result;
            if (erros.Count == 0)
            {
                result = trancaController.CadastrarTranca(tranca);
            }
            else
            {
                result = new BadRequestResult();
            }

            //Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact(DisplayName = "CT03")]
        public void EditarTrancaValida()
        {
            //Arrange
            UpdateTrancaDto tranca = new UpdateTrancaDto()
            {
                Numero = 1,
                Localizacao = "Rio de Janeiro",
                AnoFabricacao = 2020,
                Modelo = "Teste",
                Status = "NOVA"
            };

            //Act
            IncluiTrancaValida();

            var erros = ValidateModel(tranca);
            IActionResult result = trancaController.AlterarTranca(tranca, 1);

            //Assert
            Assert.Equal(0, erros.Count);
            Assert.IsType<OkObjectResult>(result);
        }

        [Theory(DisplayName = "CT04")]
        [MemberData(nameof(TrancaDados.EditarTrancasInvalidasDados), MemberType = typeof(TrancaDados))]
        public void EditarTrancasInvalidas(int numero, string localizacao, int ano, string modelo, string status)
        {
            //Arrange
            UpdateTrancaDto tranca = new UpdateTrancaDto()
            {
                Numero = numero,
                Localizacao = localizacao,
                AnoFabricacao = ano,
                Modelo = modelo,
                Status = status
            };

            //Act
            IncluiTrancaValida();

            var erros = ValidateModel(tranca);
            IActionResult result;
            if (erros.Count == 0)
            {
                result = trancaController.AlterarTranca(tranca, 1);
            }
            else
            {
                result = new BadRequestResult();
            }

            //Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact(DisplayName = "CT05")]
        public void EditarTrancaPassandoStatusExcluida()
        {
            //Arrange
            UpdateTrancaDto tranca = new UpdateTrancaDto()
            {
                Numero = 1,
                Localizacao = "Rio de Janeiro",
                AnoFabricacao = 2020,
                Modelo = "Teste",
                Status = "EXCLUIDA"
            };

            //Act
            IncluiTrancaValida();

            var erros = ValidateModel(tranca);
            IActionResult result = trancaController.AlterarTranca(tranca, 1);

            //Assert
            Assert.Equal(0, erros.Count);
            Assert.IsType<UnprocessableEntityResult>(result);
        }

        [Fact(DisplayName = "CT06")]
        public void RemoverTrancaValida()
        {
            //Arrange e Act
            IncluiTrancaValida();
            IActionResult result = trancaController.RemoverTranca(1);

            //Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact(DisplayName = "CT07")]
        public void RemoverTrancaInvalida()
        {
            //Arrange e Act
            IActionResult result = trancaController.RemoverTranca(1);

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
