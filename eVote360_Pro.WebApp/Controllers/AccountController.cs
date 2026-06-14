using System.Security.Claims;
using eVote360_Pro.Core.Application.Interfaces;
using eVote360_Pro.Core.Domain.Common.Enums;
using eVote360_Pro.Core.Domain.Entities;
using eVote360_Pro.Core.Domain.Interfaces;
using eVote360_Pro.WebApp.Models.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eVote360_Pro.WebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;
        private readonly IBaseRepository<PartyLeader> _partyLeaderRepository;

        public AccountController(IUserService userService, IUserRepository userRepository, IBaseRepository<PartyLeader> partyLeaderRepository)
        {
            _userService = userService;
            _userRepository = userRepository;
            _partyLeaderRepository = partyLeaderRepository;
        }


        [HttpGet]
        public IActionResult Login()
        {

            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToUserHome();
            }
            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userService.LoginAsync(model.Username, model.Password);
            if (user == null)
            {
                var dbUser = await _userRepository.GetByUsernameAsync(model.Username.Trim());
                if (dbUser != null && !dbUser.IsActive)
                {
                    ModelState.AddModelError(string.Empty, "El Usuario esta inactivo");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Los datos de acceso son invalidos");
                }
                return View(model);
            }

            if (user.Role == Role.PoliticalLeader)
            {
                var leaderInfo = await _partyLeaderRepository.AsQueryable()
                    .Include(x => x.PoliticalParty)
                    .FirstOrDefaultAsync(x => x.UserId == user.Id);

                if (leaderInfo == null)
                {
                    ModelState.AddModelError(string.Empty, "No tiene un partido político asignado, por lo tanto no puede iniciar sesión. Por favor, póngase en contacto con un administrador.");

                    return View(model);
                }
                if (!leaderInfo.PoliticalParty.IsActive)
                {
                    ModelState.AddModelError(string.Empty, "El partido político asignado a este usuario se encuentra inactivo.");
                    return View(model);
                }
            }

            var claims = new List<Claim>
                {
                    new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new(ClaimTypes.Name, user.Username),
                    new(ClaimTypes.Role, user.Role.ToString()),
                    new("FullName", user.FullName)
                };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(2)
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
            return RedirectToUserHome(user.Role.ToString());
        }

        [HttpPost]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");

        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        private IActionResult RedirectToUserHome(string? role = null)
        {
            var roleClaim = role ?? User.FindFirst(ClaimTypes.Role)?.Value;
            if (roleClaim == Role.Administrator.ToString())
            {
                return RedirectToAction("Index", "Home"); // TODO Panel ADmin
            }
            else if (roleClaim == Role.PoliticalLeader.ToString())
            {
                return RedirectToAction("Index", "Home"); // Todo HOme dirigente
            }

            return RedirectToAction("Index", "Home");
        }
    }
}