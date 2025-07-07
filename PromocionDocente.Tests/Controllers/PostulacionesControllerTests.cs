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
    public class PostulacionesControllerTests
    {
        private PromocionDocenteContext _context;
        private PostulacionesController _controller;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<PromocionDocenteContext>()
                .UseInMemoryDatabase("TestDb_Postulaciones")
                .Options;

            _context = new PromocionDocenteContext(options);
            _controller = new PostulacionesController(_context);

            _context.Postulaciones.Add(new Postulacione
            {
                IdPos = 1,
                CedDoc = "1234567890",
                FecPos = DateOnly.FromDateTime(DateTime.Today),
                Revisor = "admin@uta.edu.ec"
            });

            _context.SaveChanges();
        }

        [TestMethod]
        public async Task GetPostulaciones_ReturnsAll()
        {
            var result = await _controller.GetPostulaciones();
            Assert.IsNotNull(result.Value);
            Assert.AreEqual(1, result.Value.Count);
        }

        [TestMethod]
        public async Task GetPostulacione_ReturnsById()
        {
            var result = await _controller.GetPostulacione(1);
            Assert.IsNotNull(result.Value);
            Assert.AreEqual("1234567890", result.Value.CedDoc);
        }

        [TestMethod]
        public async Task GetPostulacione_ReturnsNotFound_WhenMissing()
        {
            var result = await _controller.GetPostulacione(99);
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }
    }
}
