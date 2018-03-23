using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureShare.WebApi.Wrapper.Models;
using SecureShare.WebApi.Wrapper.Services.Interfaces;
using Microsoft.EntityFrameworkCore.Migrations.Internal;
using SecureShare.Webapp.Data;

namespace SecureShare.Website.Controllers
{
    [Authorize]
    public class DebugController : Controller
    {
        private readonly IUserService _userService;
        private ApplicationDbContext _context;

        public DebugController(IUserService userService, ApplicationDbContext context)
        {
            _userService = userService;
            _context = context;
        }

        public async Task<IActionResult> DisplayMyClaims()
        {
            var claims = User.Claims;
            return View(claims);
        }

        public async Task<IActionResult> AddMeToDatabase()
        {
            await _userService.AddUserAsync(new User()
            {
                DisplayName = User.Claims.Single(e => e.Type.Equals("name")).Value,
                UserId = new Guid(User.Claims.Single(e => e.Type.Equals("http://schemas.microsoft.com/identity/claims/objectidentifier")).Value)
            });
            return Ok();
        }

        [AllowAnonymous]
        public async Task<IActionResult> ApplyMigrations()
        {
            //_context.Database.EnsureCreated();
            _context.Database.EnsureDeleted();
            _context.Database.Migrate();
            return NotFound();
        }
    }
}