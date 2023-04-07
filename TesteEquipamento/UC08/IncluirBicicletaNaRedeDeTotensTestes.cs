using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using scb_equipamentos.Controllers;
using scb_equipamentos.Data.BicicletaDto;
using scb_equipamentos.Data.TrancaDto;
using scb_equipamentos.Db;
using scb_equipamentos.Models;
using scb_equipamentos.Profiles;

namespace TesteEquipamento.UC08
{
    public class IncluirBicicletaNaRedeDeTotensTestes
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

        [Fact]
        public async void IncluirBicicletaValidaNaRede()
        {
            //arrange
            var options = new DbContextOptionsBuilder<EquipamentoContext>().UseInMemoryDatabase("EquipamentoContext").Options;
            var context = new EquipamentoContext(options);
            var controladorBicicleta = new BicicletaController(context, Mapper);
            var controladorTranca = new TrancaController(context, MapperTranca);

            var bicicleta = new CreateBicicletaDto()
            {
                Marca = "string",
                Modelo = "string",
                AnoFabricacao = 2023,
            };
            var tranca = new CreateTrancaDto()
            {
                Numero = 1,
                AnoFabricacao = 2023,
                Modelo = "string",
                Localizacao = "string",
            };

            var actionResultBicicleta = controladorBicicleta.CadastrarBicicicleta(bicicleta);
            var actionResultTranca = controladorTranca.CadastrarTranca(tranca);

            var okResult = actionResultBicicleta as OkObjectResult;
            var actualConfigurationJStr = JsonConvert.SerializeObject(okResult.Value);
            var bicicletaRetornada = JsonConvert.DeserializeObject<Bicicleta>(actualConfigurationJStr);
            int idBicicleta = bicicletaRetornada.Id;

            var okResultTranca = actionResultTranca as OkObjectResult;
            var actualConfigurationJStrTranca = JsonConvert.SerializeObject(okResultTranca.Value);
            var trancaRetornada = JsonConvert.DeserializeObject<Tranca>(actualConfigurationJStrTranca);
            int idTranca = trancaRetornada.Id;


            controladorTranca.AlterarStatusTranca(idTranca, "LIVRE");
            var integrarBicicleta = new IntegrarBicicletaDto()
            {
                IdBicicleta = idBicicleta,
                IdFuncionario = 1,
                IdTranca = idTranca,
            };

            //act
            var resultado = await controladorBicicleta.IntegrarBicicletaNaRede(integrarBicicleta);

            //assert
            Assert.IsType<OkResult>(resultado);
        }

        [Fact]
        public async void IncluirBicicletaInexistenteNaRede()
        {
            //arrange
            var options = new DbContextOptionsBuilder<EquipamentoContext>().UseInMemoryDatabase("EquipamentoContext").Options;
            var context = new EquipamentoContext(options);
            var controladorBicicleta = new BicicletaController(context, Mapper);
            var controladorTranca = new TrancaController(context, MapperTranca);

            var tranca = new CreateTrancaDto()
            {
                Numero = 1,
                AnoFabricacao = 2023,
                Modelo = "string",
                Localizacao = "string",
            };

            var actionResultTranca = controladorTranca.CadastrarTranca(tranca);

            var okResultTranca = actionResultTranca as OkObjectResult;
            var actualConfigurationJStrTranca = JsonConvert.SerializeObject(okResultTranca.Value);
            var trancaRetornada = JsonConvert.DeserializeObject<Tranca>(actualConfigurationJStrTranca);
            int idTranca = trancaRetornada.Id;


            controladorTranca.AlterarStatusTranca(idTranca, "LIVRE");
            var integrarBicicleta = new IntegrarBicicletaDto()
            {
                IdBicicleta = 0,
                IdFuncionario = 1,
                IdTranca = idTranca,
            };

            //act
            var resultado = await controladorBicicleta.IntegrarBicicletaNaRede(integrarBicicleta);

            //assert
            Assert.IsType<UnprocessableEntityResult>(resultado);
        }

        [Fact]
        public async void IncluirBicicletaComTrancaInexistenteNaRede()
        {
            //arrange
            var options = new DbContextOptionsBuilder<EquipamentoContext>().UseInMemoryDatabase("EquipamentoContext").Options;
            var context = new EquipamentoContext(options);
            var controladorBicicleta = new BicicletaController(context, Mapper);
            var controladorTranca = new TrancaController(context, MapperTranca);

            var bicicleta = new CreateBicicletaDto()
            {
                Marca = "string",
                Modelo = "string",
                AnoFabricacao = 2023,
            };

            var actionResultBicicleta = controladorBicicleta.CadastrarBicicicleta(bicicleta);

            var okResult = actionResultBicicleta as OkObjectResult;
            var actualConfigurationJStr = JsonConvert.SerializeObject(okResult.Value);
            var bicicletaRetornada = JsonConvert.DeserializeObject<Bicicleta>(actualConfigurationJStr);
            int idBicicleta = bicicletaRetornada.Id;


            var integrarBicicleta = new IntegrarBicicletaDto()
            {
                IdBicicleta = idBicicleta,
                IdFuncionario = 1,
                IdTranca = 0,
            };

            //act
            var resultado = await controladorBicicleta.IntegrarBicicletaNaRede(integrarBicicleta);

            //assert
            Assert.IsType<UnprocessableEntityResult>(resultado);
        }

        [Fact]
        public async void IncluirBicicletaComStatusInvalidoNaRede()
        {
            //arrange
            var options = new DbContextOptionsBuilder<EquipamentoContext>().UseInMemoryDatabase("EquipamentoContext").Options;
            var context = new EquipamentoContext(options);
            var controladorBicicleta = new BicicletaController(context, Mapper);
            var controladorTranca = new TrancaController(context, MapperTranca);

            var bicicleta = new CreateBicicletaDto()
            {
                Marca = "string",
                Modelo = "string",
                AnoFabricacao = 2023,
            };
            var tranca = new CreateTrancaDto()
            {
                Numero = 1,
                AnoFabricacao = 2023,
                Modelo = "string",
                Localizacao = "string",
            };

            var actionResultBicicleta = controladorBicicleta.CadastrarBicicicleta(bicicleta);
            var actionResultTranca = controladorTranca.CadastrarTranca(tranca);

            var okResult = actionResultBicicleta as OkObjectResult;
            var actualConfigurationJStr = JsonConvert.SerializeObject(okResult.Value);
            var bicicletaRetornada = JsonConvert.DeserializeObject<Bicicleta>(actualConfigurationJStr);
            int idBicicleta = bicicletaRetornada.Id;

            var okResultTranca = actionResultTranca as OkObjectResult;
            var actualConfigurationJStrTranca = JsonConvert.SerializeObject(okResultTranca.Value);
            var trancaRetornada = JsonConvert.DeserializeObject<Tranca>(actualConfigurationJStrTranca);
            int idTranca = trancaRetornada.Id;


            controladorTranca.AlterarStatusTranca(idTranca, "LIVRE");
            controladorBicicleta.AtualizarStatus(idBicicleta, "DISPONIVEL");
            var integrarBicicleta = new IntegrarBicicletaDto()
            {
                IdBicicleta = idBicicleta,
                IdFuncionario = 1,
                IdTranca = idTranca,
            };

            //act
            var resultado = await controladorBicicleta.IntegrarBicicletaNaRede(integrarBicicleta);

            //assert
            Assert.IsType<UnprocessableEntityResult>(resultado);
        }

        [Fact]
        public async void IncluirBicicletaComTrancaComStatusInvalidoNaRede()
        {
            //arrange
            var options = new DbContextOptionsBuilder<EquipamentoContext>().UseInMemoryDatabase("EquipamentoContext").Options;
            var context = new EquipamentoContext(options);
            var controladorBicicleta = new BicicletaController(context, Mapper);
            var controladorTranca = new TrancaController(context, MapperTranca);

            var bicicleta = new CreateBicicletaDto()
            {
                Marca = "string",
                Modelo = "string",
                AnoFabricacao = 2023,
            };
            var tranca = new CreateTrancaDto()
            {
                Numero = 1,
                AnoFabricacao = 2023,
                Modelo = "string",
                Localizacao = "string",
            };

            var actionResultBicicleta = controladorBicicleta.CadastrarBicicicleta(bicicleta);
            var actionResultTranca = controladorTranca.CadastrarTranca(tranca);

            var okResult = actionResultBicicleta as OkObjectResult;
            var actualConfigurationJStr = JsonConvert.SerializeObject(okResult.Value);
            var bicicletaRetornada = JsonConvert.DeserializeObject<Bicicleta>(actualConfigurationJStr);
            int idBicicleta = bicicletaRetornada.Id;

            var okResultTranca = actionResultTranca as OkObjectResult;
            var actualConfigurationJStrTranca = JsonConvert.SerializeObject(okResultTranca.Value);
            var trancaRetornada = JsonConvert.DeserializeObject<Tranca>(actualConfigurationJStrTranca);
            int idTranca = trancaRetornada.Id;

            var integrarBicicleta = new IntegrarBicicletaDto()
            {
                IdBicicleta = idBicicleta,
                IdFuncionario = 1,
                IdTranca = idTranca,
            };

            //act
            var resultado = await controladorBicicleta.IntegrarBicicletaNaRede(integrarBicicleta);

            //assert
            Assert.IsType<UnprocessableEntityResult>(resultado);
        }

        [Fact]
        public async void IncluirBicicletaValidaComErroNoEnvioDoEmail()
        {
            //arrange
            var options = new DbContextOptionsBuilder<EquipamentoContext>().UseInMemoryDatabase("EquipamentoContext").Options;
            var context = new EquipamentoContext(options);
            var controladorBicicleta = new BicicletaController(context, Mapper);
            var controladorTranca = new TrancaController(context, MapperTranca);

            var bicicleta = new CreateBicicletaDto()
            {
                Marca = "string",
                Modelo = "string",
                AnoFabricacao = 2023,
            };
            var tranca = new CreateTrancaDto()
            {
                Numero = 1,
                AnoFabricacao = 2023,
                Modelo = "string",
                Localizacao = "string",
            };

            var actionResultBicicleta = controladorBicicleta.CadastrarBicicicleta(bicicleta);
            var actionResultTranca = controladorTranca.CadastrarTranca(tranca);

            var okResult = actionResultBicicleta as OkObjectResult;
            var actualConfigurationJStr = JsonConvert.SerializeObject(okResult.Value);
            var bicicletaRetornada = JsonConvert.DeserializeObject<Bicicleta>(actualConfigurationJStr);
            int idBicicleta = bicicletaRetornada.Id;

            var okResultTranca = actionResultTranca as OkObjectResult;
            var actualConfigurationJStrTranca = JsonConvert.SerializeObject(okResultTranca.Value);
            var trancaRetornada = JsonConvert.DeserializeObject<Tranca>(actualConfigurationJStrTranca);
            int idTranca = trancaRetornada.Id;


            controladorTranca.AlterarStatusTranca(idTranca, "LIVRE");
            var integrarBicicleta = new IntegrarBicicletaDto()
            {
                IdBicicleta = idBicicleta,
                IdFuncionario = idTranca,
                IdTranca = 0,
            };

            //act
            var resultado = await controladorBicicleta.IntegrarBicicletaNaRede(integrarBicicleta);

            //assert
            Assert.IsType<UnprocessableEntityResult>(resultado);
        }

    }
}
