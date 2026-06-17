using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using eVote360_Pro.Core.Application.Interfaces;
using eVote360_Pro.WebApp.Models.Account;

namespace eVote360_Pro.WebApp.Controllers;

public class HomeController : Controller
{
    private readonly IDashboardService _dashboardService;

    public HomeController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    public async Task<IActionResult> Index()
    {
        // Si el usuario es Administrador, cargamos las estadísticas para el Dashboard Ejecutivo
        if (User.Identity?.IsAuthenticated == true && User.IsInRole("Administrator"))
        {
            var dashboardData = await _dashboardService.GetDashboardDataAsync();
            return View(dashboardData);
        }

        // De lo contrario (visitante o dirigente), retornamos la vista regular sin modelo de dashboard
        return View(null);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
