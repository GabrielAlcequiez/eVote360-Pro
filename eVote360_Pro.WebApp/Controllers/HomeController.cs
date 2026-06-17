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

    public async Task<IActionResult> Index(int? year)
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            if (User.IsInRole("Administrator"))
            {
                var dashboardData = await _dashboardService.GetDashboardDataAsync(year);
                return View(dashboardData);
            }
            else if (User.IsInRole("PoliticalLeader"))
            {
                var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (Guid.TryParse(userIdString, out Guid userId))
                {
                    try
                    {
                        var leaderDashboardData = await _dashboardService.GetLeaderDashboardDataAsync(userId);
                        ViewBag.LeaderDashboard = leaderDashboardData;
                    }
                    catch (Exception)
                    {
                        // Silently handle if party association is not found
                    }
                }
            }
        }

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
