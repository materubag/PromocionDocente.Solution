using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using PromocionDocente.API.Controllers;
using PromocionDocente.Models.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace PromocionDocente.Tests.Controllers
{
    [TestClass]
    public class InvestigacionesControllerTests
    {
        private PromocionDocenteContext _context;
        private InvestigacionesController _controller;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<PromocionDocenteContext>()
                .UseInMemoryDatabase("TestDb_Investigaciones")
                .Options;

            _context = new PromocionDocenteContext(options);
            _controller = new InvestigacionesController(_context, null); // El servicio puede ir null por ahora

            _context.Investigaciones.Add(new Investigacione
            {
                IdInvestigacion = 1,
                CedDoc = "1234567890",
                TituloInvestigacion = "IA aplicada a la educación",
                FechaInicio = DateOnly.FromDateTime(DateTime.Today.AddMonths(-3)),
                FechaFin = DateOnly.FromDateTime(DateTime.Today),
                Estado = "PENDIENTE",
                DuracionMeses = 3
            });

            _context.SaveChanges();
        }

        [TestMethod]
        public async Task GetInvestigaciones_ReturnsAll()
        {
            var result = await _controller.GetInvestigaciones();
            Assert.IsNotNull(result.Value);
            Assert.AreEqual(1, result.Value.Count);
        }

        [TestMethod]
        public async Task GetInvestigacione_ReturnsCorrectItem()
        {
            var result = await _controller.GetInvestigacione(1);
            Assert.IsNotNull(result.Value);
            Assert.AreEqual("IA aplicada a la educación", result.Value.TituloInvestigacion);
        }

        [TestMethod]
        public async Task GetInvestigacione_ReturnsNotFound_WhenNotExists()
        {
            var result = await _controller.GetInvestigacione(999);
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task DeleteInvestigacione_RemovesItem()
        {
            var result = await _controller.DeleteInvestigacione(1);
            Assert.IsInstanceOfType(result, typeof(NoContentResult));

            var investigacion = await _context.Investigaciones.FindAsync(1);
            Assert.IsNull(investigacion);
        }
    }
}
