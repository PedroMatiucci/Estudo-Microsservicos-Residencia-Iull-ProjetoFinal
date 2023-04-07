using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using scb_equipamentos.Controllers;
using scb_equipamentos.Data.TotemDto;
using scb_equipamentos.Data.TrancaDto;
using scb_equipamentos.Db;
using scb_equipamentos.Profiles;

namespace TesteEquipamento
{
    public class UC11
    {
        private TrancaController trancaController;
        private TotemController totemController;

        public UC11() {
            var options = new DbContextOptionsBuilder<EquipamentoContext>().UseInMemoryDatabase("EquipamentoContext").Options;
            var context = new EquipamentoContext(options);

            IMapper mapperTranca = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new TrancaProfile())));
            IMapper mapperTotem = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new TotemProfile())));

            trancaController = new TrancaController(context, mapperTranca);
            totemController = new TotemController(context, mapperTotem);
        }

        [Fact(DisplayName = "CT01")]
        public async void IncluiTrancaEmTotem() {
            //Arrange
            CreateTrancaDto criarTranca = new CreateTrancaDto() 
            {
                Numero = 1,
                Localizacao = "Rio de Janeiro",
                AnoFabricacao = 2020,
                Modelo = "Teste"
            };

            CreateTotemDto criarTotem = new CreateTotemDto()
            {
                Localizacao = "Rio de Janeiro"
            };

            IntegrarTrancaDto integrarTranca = new IntegrarTrancaDto() 
            { 
                IdTotem = 1,
                IdTranca = 1,
                IdFuncionario = 1
            };

            //Act
            trancaController.CadastrarTranca(criarTranca);
            totemController.CadastrarTotem(criarTotem);
            IActionResult result = await trancaController.IntegrarTranca(integrarTranca);

            //Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact(DisplayName = "CT02")]
        public async void IncluiTrancaEmTotemComTotemInexistente()
        {
            //Arrange
            CreateTrancaDto criarTranca = new CreateTrancaDto()
            {
                Numero = 1,
                Localizacao = "Rio de Janeiro",
                AnoFabricacao = 2020,
                Modelo = "Teste"
            };

            IntegrarTrancaDto integrarTranca = new IntegrarTrancaDto()
            {
                IdTotem = 1,
                IdTranca = 1,
                IdFuncionario = 1
            };

            //Act
            trancaController.CadastrarTranca(criarTranca);
            IActionResult result = await trancaController.IntegrarTranca(integrarTranca);

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact(DisplayName = "CT03")]
        public async void IncluiTrancaEmTotemComTrancaInexistente()
        {
            //Arrange
            CreateTotemDto criarTotem = new CreateTotemDto()
            {
                Localizacao = "Rio de Janeiro"
            };

            IntegrarTrancaDto integrarTranca = new IntegrarTrancaDto()
            {
                IdTotem = 1,
                IdTranca = 1,
                IdFuncionario = 1
            };

            //Act
            totemController.CadastrarTotem(criarTotem);
            IActionResult result = await trancaController.IntegrarTranca(integrarTranca);

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact(DisplayName = "CT04")]
        public async void IncluiTrancaEmTotemComAmbosInexistentes()
        {
            //Arrange
            IntegrarTrancaDto integrarTranca = new IntegrarTrancaDto()
            {
                IdTotem = 1,
                IdTranca = 1,
                IdFuncionario = 1
            };

            //Act
            IActionResult result = await trancaController.IntegrarTranca(integrarTranca);

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact(DisplayName = "CT05")]
        public async void IncluiTrancaEmTotemComFuncionariosInexistente()
        {
            //Arrange
            CreateTrancaDto criarTranca = new CreateTrancaDto()
            {
                Numero = 1,
                Localizacao = "Rio de Janeiro",
                AnoFabricacao = 2020,
                Modelo = "Teste"
            };

            CreateTotemDto criarTotem = new CreateTotemDto()
            {
                Localizacao = "Rio de Janeiro"
            };

            IntegrarTrancaDto integrarTranca = new IntegrarTrancaDto()
            {
                IdTotem = 1,
                IdTranca = 1,
                IdFuncionario = 100
            };

            //Act
            trancaController.CadastrarTranca(criarTranca);
            totemController.CadastrarTotem(criarTotem);
            IActionResult result = await trancaController.IntegrarTranca(integrarTranca);

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact(DisplayName = "CT06")]
        public async void IncluiTrancaEmTotemComTrancaEmReparo()
        {
            //Arrange
            CreateTrancaDto criarTranca = new CreateTrancaDto()
            {
                Numero = 1,
                Localizacao = "Rio de Janeiro",
                AnoFabricacao = 2020,
                Modelo = "Teste"
            };

            CreateTotemDto criarTotem = new CreateTotemDto()
            {
                Localizacao = "Rio de Janeiro"
            };

            IntegrarTrancaDto integrarTranca = new IntegrarTrancaDto()
            {
                IdTotem = 1,
                IdTranca = 1,
                IdFuncionario = 1
            };

            //Act
            trancaController.CadastrarTranca(criarTranca);
            totemController.CadastrarTotem(criarTotem);
            trancaController.AlterarStatusTranca(1, "EM_REPARO");
            IActionResult result = await trancaController.IntegrarTranca(integrarTranca);

            //Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact(DisplayName = "CT07")]
        public async void IncluiTrancaEmTotemComTudoInexistente() {
            //Arrange
            IntegrarTrancaDto integrarTranca = new IntegrarTrancaDto()
            {
                IdTotem = 1,
                IdTranca = 1,
                IdFuncionario = 100
            };

            //Act
            IActionResult result = await trancaController.IntegrarTranca(integrarTranca);

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact(DisplayName = "CT08")]
        public async void IncluiTrancaEmTotemComTrancaLivre()
        {
            //Arrange
            CreateTrancaDto criarTranca = new CreateTrancaDto()
            {
                Numero = 1,
                Localizacao = "Rio de Janeiro",
                AnoFabricacao = 2020,
                Modelo = "Teste"
            };

            CreateTotemDto criarTotem = new CreateTotemDto()
            {
                Localizacao = "Rio de Janeiro"
            };

            IntegrarTrancaDto integrarTranca = new IntegrarTrancaDto()
            {
                IdTotem = 1,
                IdTranca = 1,
                IdFuncionario = 1
            };

            //Act
            trancaController.CadastrarTranca(criarTranca);
            totemController.CadastrarTotem(criarTotem);
            trancaController.AlterarStatusTranca(1, "LIVRE");
            IActionResult result = await trancaController.IntegrarTranca(integrarTranca);

            //Assert
            Assert.IsType<UnprocessableEntityResult>(result);
        }

        [Fact(DisplayName = "CT09")]
        public async void IncluiTrancaEmTotemComTrancaOcupada()
        {
            //Arrange
            CreateTrancaDto criarTranca = new CreateTrancaDto()
            {
                Numero = 1,
                Localizacao = "Rio de Janeiro",
                AnoFabricacao = 2020,
                Modelo = "Teste"
            };

            CreateTotemDto criarTotem = new CreateTotemDto()
            {
                Localizacao = "Rio de Janeiro"
            };

            IntegrarTrancaDto integrarTranca = new IntegrarTrancaDto()
            {
                IdTotem = 1,
                IdTranca = 1,
                IdFuncionario = 1
            };

            //Act
            trancaController.CadastrarTranca(criarTranca);
            totemController.CadastrarTotem(criarTotem);
            trancaController.AlterarStatusTranca(1, "OCUPADA");
            IActionResult result = await trancaController.IntegrarTranca(integrarTranca);

            //Assert
            Assert.IsType<UnprocessableEntityResult>(result);
        }
    }
}
