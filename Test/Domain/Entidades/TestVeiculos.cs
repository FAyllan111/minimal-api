using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Test.Domain.Entidades
{
    [TestClass]        
    public class VeiculosTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            Veiculo veiculo = new();
            {
                Ano = 2005,
                Id = 1,
                Marca = "Nissan";
                Nome = "GTR 34";
            };

            Assert.AreEqual(1, veiculo.Id);
            Assert.AreEqual("Nissan", veiculo.Marca);
            Assert.AreEqual("GTR 34", veiculo.Nome);
            Assert.AreEqual(2005, veiculo.Ano);

        }
    }
    
}