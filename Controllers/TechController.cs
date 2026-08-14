using Microsoft.AspNetCore.Mvc;

namespace TechChallenge.Controllers;

public class TechController : Controller
{
    public IActionResult Index()
    {
        ViewData["Title"] = "TechChallenge";
        ViewData["Message"] = "Bem-vindo à plataforma TechChallenge!";
        return View();
    }

    public IActionResult About()
    {
        ViewData["Title"] = "Sobre";
        ViewData["Message"] = "TechChallenge é uma plataforma para gestão de projetos estudantis.";
        return View();
    }

    public IActionResult Contact()
    {
        ViewData["Title"] = "Contato";
        ViewData["Message"] = "contato@techchallenge.com";
        return View();
    }

    public IActionResult Dashboard()
    {
        var desafios = new List<dynamic>
        {
            new { Nome = "Cidade Inteligente", Descricao = "Projete uma solução urbana conectada", Categoria = "Inovação", Pontuacao = 180 },
            new { Nome = "Laboratório Verde", Descricao = "Desenvolva um experimento com foco ambiental", Categoria = "Meio Ambiente", Pontuacao = 220 },
            new { Nome = "Plataforma de Estudos", Descricao = "Crie uma ferramenta digital para aprendizagem", Categoria = "Educação", Pontuacao = 160 },
            new { Nome = "Desafio Maker", Descricao = "Construa um protótipo funcional de baixo custo", Categoria = "Engenharia", Pontuacao = 270 },
            new { Nome = "App Sustentável", Descricao = "Crie um app sobre sustentabilidade", Categoria = "Tecnologia", Pontuacao = 100 },
            new { Nome = "Feira de Ciências", Descricao = "Monte um experimento inovador", Categoria = "Ciências", Pontuacao = 200 },
            new { Nome = "Game Educativo", Descricao = "Desenvolva um jogo educativo", Categoria = "Jogos", Pontuacao = 150 },
            new { Nome = "Robótica", Descricao = "Construa um robô com materiais recicláveis", Categoria = "Robótica", Pontuacao = 250 }

        };

        ViewBag.Desafios = desafios;
        return View();
    }
}