using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using MinimalApi.Dominio.Entidades;
using MinimalApi.Dominio.ModelViews;
using MinimalApi.DTOs;
using Test.Helpers;

namespace Test.Requests;

[TestClass]
public class VeiculoRequestTest
{
    [ClassInitialize]
    public static void ClassInit(TestContext testContext)
    {
        Setup.ClassInit(testContext);
    }

    [ClassCleanup]
    public static void ClassCleanup()
    {
        Setup.ClassCleanup();
    }

    [TestMethod]
    public async Task TestIncluirVeiculo()
    {
        // Arrange
        VeiculoDTO veiculoDTO = new()
        {
            Ano = 2024,
            Marca = "Honda",
            Nome = "Civic",
        };
        var loginDTO = new LoginDTO{
            Email = "adm@teste.com",
            Senha = "123456"
        };
        var contentLogin = new StringContent(JsonSerializer.Serialize(loginDTO), Encoding.UTF8,  "Application/json");
        var responseLogin = await Setup.client.PostAsync("/administradores/login", contentLogin);
        var result = await responseLogin.Content.ReadAsStringAsync();
        var admLogado = JsonSerializer.Deserialize<AdministradorLogado>(result, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        var request = new HttpRequestMessage(HttpMethod.Post, "/veiculos")
        {
            Content = new StringContent(JsonSerializer.Serialize(veiculoDTO), Encoding.UTF8, "application/json")
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", admLogado?.Token);
        // Act
        var response = await Setup.client.SendAsync(request);
        // Assert
        Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

    }
    [TestMethod]
    public async Task TestGetVeiculoPorId(){
        var loginDTO = new LoginDTO{
            Email = "adm@teste.com",
            Senha = "123456"
        };
        var contentLogin = new StringContent(JsonSerializer.Serialize(loginDTO), Encoding.UTF8,  "Application/json");
        var responseLogin = await Setup.client.PostAsync("/administradores/login", contentLogin);
        var result = await responseLogin.Content.ReadAsStringAsync();
        var admLogado = JsonSerializer.Deserialize<AdministradorLogado>(result, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        var request = new HttpRequestMessage(HttpMethod.Get, $"/veiculos/{1}");

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", admLogado?.Token);

        // Act
        var response = await Setup.client.SendAsync(request);
        // Assert
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
        else
        {
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var veiculo = JsonSerializer.Deserialize<Veiculo>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            Assert.IsNotNull(veiculo);
            Assert.AreEqual(1, veiculo.Id);
        }
    }

    [TestMethod]
    public async Task TestAtualizarVeiculo(){
        var loginDTO = new LoginDTO{
            Email = "adm@teste.com",
            Senha = "123456"
        };
        VeiculoDTO veiculoDTO = new()
        {
            Ano = 2024,
            Marca = "Honda",
            Nome = "Civic",
        };
        var contentLogin = new StringContent(JsonSerializer.Serialize(loginDTO), Encoding.UTF8,  "Application/json");
        var responseLogin = await Setup.client.PostAsync("/administradores/login", contentLogin);
        var result = await responseLogin.Content.ReadAsStringAsync();
        var admLogado = JsonSerializer.Deserialize<AdministradorLogado>(result, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        var request = new HttpRequestMessage(HttpMethod.Put, $"/veiculos/{1}")
        {
            Content = new StringContent(JsonSerializer.Serialize(veiculoDTO), Encoding.UTF8, "application/json")
        };

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", admLogado?.Token);

        // Act
        var response = await Setup.client.SendAsync(request);
        // Assert
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        else
        {
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var veiculoAtualizado = JsonSerializer.Deserialize<Veiculo>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            Assert.IsNotNull(veiculoAtualizado);
            Assert.AreEqual(veiculoDTO.Nome, veiculoAtualizado.Nome);
            Assert.AreEqual(veiculoDTO.Marca, veiculoAtualizado.Marca);
            Assert.AreEqual(veiculoDTO.Ano, veiculoAtualizado.Ano);
        }
    }

    [TestMethod]
    public async Task TestApagarVeiculo()
    {
        // Arrange
        int veiculoId = 1; // Substitua pelo ID do veículo que deseja testar

        var loginDTO = new LoginDTO
        {
            Email = "adm@teste.com",
            Senha = "123456"
        };

        var contentLogin = new StringContent(JsonSerializer.Serialize(loginDTO), Encoding.UTF8, "application/json");

        var responseLogin = await Setup.client.PostAsync("/administradores/login", contentLogin);
        var result = await responseLogin.Content.ReadAsStringAsync();
        var admLogado = JsonSerializer.Deserialize<AdministradorLogado>(result, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        // Adicione o token ao cabeçalho da requisição
        var request = new HttpRequestMessage(HttpMethod.Delete, $"/veiculos/{veiculoId}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", admLogado?.Token);

        // Act
        var response = await Setup.client.SendAsync(request);

        // Assert
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
        else
        {
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
        }
    }

}