using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using scb_equipamentos.Controllers;
using scb_equipamentos.Data.TotemDto;
using scb_equipamentos.Data.TrancaDto;
using scb_equipamentos.Db;
using scb_equipamentos.Models;
using scb_equipamentos.Profiles;
using System.ComponentModel.DataAnnotations;

namespace TesteEquipamento.UC14
{
    public class RemoverTotemTestes
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

        private static IMapper _mapperTranca;
        public static IMapper MapperTranca
        {
            get
            {
                if (_mapperTranca == null)
                {
                    // Auto Mapper Configurations
                    var mappingConfig = new MapperConfiguration(mc =>
                    {
                        mc.AddProfile(new TrancaProfile());
                    });
                    IMapper mapper = mappingConfig.CreateMapper();
                    _mapperTranca = mapper;
                }
                return _mapperTranca;
            }
        }

        private IList<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var ctx = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, ctx, validationResults, true);
            return validationResults;
        }

        [Fact(DisplayName = "Remover Totem Válido")]
        public void RemoverTotemValido()
        {
            //arrange
            var options = new DbContextOptionsBuilder<EquipamentoContext>().UseInMemoryDatabase("EquipamentoContext").Options;
            var context = new EquipamentoContext(options);
            var controladorTotem = new TotemController(context, Mapper);

            var totem = new CreateTotemDto();
            totem.Localizacao = "string";

            var actionResult = controladorTotem.CadastrarTotem(totem);

            var okResult = actionResult as OkObjectResult;
            var actualConfigurationJStr = JsonConvert.SerializeObject(okResult.Value);
            var totemRetornada = JsonConvert.DeserializeObject<Totem>(actualConfigurationJStr);
            int numeroTotem = totemRetornada.Numero;

            //act
            var resultado = controladorTotem.RemoverTotem(numeroTotem);

            //assert
            Assert.IsType<OkResult>(resultado);

        }

        [Fact(DisplayName = "Remover Totem inexistente")]
        public void RemoverTotemInexistente()
        {
            //arrange
            var options = new DbContextOptionsBuilder<EquipamentoContext>().UseInMemoryDatabase("EquipamentoContext").Options;
            var context = new EquipamentoContext(options);
            var controladorTotem = new TotemController(context, Mapper);

            //act
            var resultado = controladorTotem.RemoverTotem(1);

            //assert
            Assert.IsType<NotFoundResult>(resultado);

        }

        [Fact(DisplayName = "Remover Totem com Tranca")]
        public async void RemoverTotemComTranca()
        {
            //arrange
            var options = new DbContextOptionsBuilder<EquipamentoContext>().UseInMemoryDatabase("EquipamentoContext").Options;
            var context = new EquipamentoContext(options);
            var controladorTotem = new TotemController(context, Mapper);
            var controladorTranca = new TrancaController(context, MapperTranca);

            CreateTrancaDto tranca = new CreateTrancaDto()
            {
                Numero = 1,
                Localizacao = "Rio de Janeiro",
                AnoFabricacao = 2020,
                Modelo = "Teste"
            };

            var totem = new CreateTotemDto();
            totem.Localizacao = "string";

            var actionResultTotem = controladorTotem.CadastrarTotem(totem);
            var actionResultTranca = controladorTranca.CadastrarTranca(tranca);

            var okResult = actionResultTotem as OkObjectResult;
            var actualConfigurationJStr = JsonConvert.SerializeObject(okResult.Value);
            var totemRetornada = JsonConvert.DeserializeObject<Totem>(actualConfigurationJStr);
            int numeroTotem = totemRetornada.Numero;

            var okResultTranca = actionResultTranca as OkObjectResult;
            var actualConfigurationJStrTranca = JsonConvert.SerializeObject(okResultTranca.Value);
            var trancaRetornada = JsonConvert.DeserializeObject<Tranca>(actualConfigurationJStrTranca);
            int idTranca = trancaRetornada.Id;

            IntegrarTrancaDto integrarTranca = new IntegrarTrancaDto()
            {
                IdTotem = numeroTotem,
                IdTranca = idTranca,
                IdFuncionario = 1
            };

            //act
            await controladorTranca.IntegrarTranca(integrarTranca);
            var resultado = controladorTotem.RemoverTotem(numeroTotem);

            //assert
            Assert.IsType<NotFoundResult>(resultado);

        }
    }
}
