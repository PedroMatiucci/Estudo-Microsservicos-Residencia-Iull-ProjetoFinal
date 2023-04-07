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
using System;

namespace TesteEquipamento.UC10
{
    public class RemoverBicicletaTestes
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
        public async void RemoverBicicletaValido()
        {
            //arrange
            var options = new DbContextOptionsBuilder<EquipamentoContext>().UseInMemoryDatabase("EquipamentoContext").Options;
            var context = new EquipamentoContext(options);
            var controladorBicicleta = new BicicletaController(context, Mapper);

            var bicicleta = new CreateBicicletaDto();
            bicicleta.Marca = "string";
            bicicleta.Modelo = "string";
            bicicleta.AnoFabricacao = 2023;

            var actionResultBicicleta = controladorBicicleta.CadastrarBicicicleta(bicicleta);

            var okResult = actionResultBicicleta as OkObjectResult;
            var actualConfigurationJStr = JsonConvert.SerializeObject(okResult.Value);
            var bicicletaRetornada = JsonConvert.DeserializeObject<Bicicleta>(actualConfigurationJStr);
            int idBicicleta = bicicletaRetornada.Id;

            controladorBicicleta.AtualizarStatus(idBicicleta, "APOSENTADA");

            //act
            var resultado = controladorBicicleta.DeletarBicicleta(idBicicleta);


            //assert
            Assert.IsType<OkResult>(resultado);
        }


        [Fact]
        public async void RemoverBicicletaInexistente()
        {
            //arrange
            var options = new DbContextOptionsBuilder<EquipamentoContext>().UseInMemoryDatabase("EquipamentoContext").Options;
            var context = new EquipamentoContext(options);
            var controladorBicicleta = new BicicletaController(context, Mapper);

            //act
            var resultado = controladorBicicleta.DeletarBicicleta(1);


            //assert
            Assert.IsType<NotFoundResult>(resultado);
        }



        [Fact]
        public async void RemoverBicicletaTrancaInexistente()
        {
            //arrange
            var options = new DbContextOptionsBuilder<EquipamentoContext>().UseInMemoryDatabase("EquipamentoContext").Options;
            var context = new EquipamentoContext(options);
            var controladorBicicleta = new BicicletaController(context, Mapper);

            var bicicleta = new CreateBicicletaDto();
            bicicleta.Marca = "string";
            bicicleta.Modelo = "string";
            bicicleta.AnoFabricacao = 2023;


            var actionResultBicicleta = controladorBicicleta.CadastrarBicicicleta(bicicleta);


            var okResult = actionResultBicicleta as OkObjectResult;
            var actualConfigurationJStr = JsonConvert.SerializeObject(okResult.Value);
            var bicicletaRetornada = JsonConvert.DeserializeObject<Bicicleta>(actualConfigurationJStr);
            int idBicicleta = bicicletaRetornada.Id;


            //act
            var resultado = controladorBicicleta.DeletarBicicleta(idBicicleta);


            //assert
            Assert.IsType<NotFoundResult>(resultado);
        }

        [Fact]
        public async void RemoverBicicletaComStatusInvalido()
        {
            //arrange
            var options = new DbContextOptionsBuilder<EquipamentoContext>().UseInMemoryDatabase("EquipamentoContext").Options;
            var context = new EquipamentoContext(options);
            var controladorBicicleta = new BicicletaController(context, Mapper);
            var controladorTranca = new TrancaController(context, MapperTranca);

            var bicicleta = new CreateBicicletaDto();
            bicicleta.Marca = "string";
            bicicleta.Modelo = "string";
            bicicleta.AnoFabricacao = 2023;

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
                IdTranca = idTranca,
                IdFuncionario = 1,
            };

            controladorTranca.AlterarStatusTranca(idTranca, "Livre");
            await controladorBicicleta.IntegrarBicicletaNaRede(integrarBicicleta);
            controladorBicicleta.AtualizarStatus(idBicicleta, "NOVA");

            //act
            var resultado = controladorBicicleta.DeletarBicicleta(idBicicleta);


            //assert
            Assert.IsType<NotFoundResult>(resultado);
        }
    }
}
