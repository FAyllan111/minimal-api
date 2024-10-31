using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Test.Mocks
{
    public class VeiculoServicoMock : IVeiculoServico
    {
        private static readonly List<Veiculo> veiculos = new(){
            new Veiculo{
                Id = 1,
                Ano = 2024,
                Marca = "Honda",
                Nome = "Civic Type R"
            },            
            new Veiculo{
                Id = 2,
                Ano = 2001,
                Marca = "Toyota",
                Nome = "Corola"
            }
        };



        public void Apagar(Veiculo veiculo)
        {
            veiculos.Remove(veiculo);
        }

        public void Atualizar(Veiculo veiculo)
        {
            veiculos[veiculo.Id] = veiculo;

        }

        public Veiculo? BuscaPorId(int id)
        {
            return veiculos.Find(veiculo => veiculo.Id == id);
        }

        public void Incluir(Veiculo veiculo)
        {
            veiculo.Id = veiculos.Count + 1;
            veiculos.Add(veiculo);

        }

        public List<Veiculo> Todos(int? pagina = 1, string? nome = null, string? marca = null)
        {
            return veiculos;
        }
    }