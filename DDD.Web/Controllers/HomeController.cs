using DDD.Applaction.AdminUsers.Interfaces;
using DDD.Domain.AdminUsers;
using DDD.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DDD.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAdminUserAppService adminUserAppService;

        public HomeController(ILogger<HomeController> logger, IAdminUserAppService adminUserAppService)
        {
            _logger = logger;
            this.adminUserAppService = adminUserAppService;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var res = await adminUserAppService.GetListAsync(10, "", CancellationToken.None);

            var user = new AdminUserCommand.CreateCommand() { };

            var vaild = user.IsVaild();

            var errors = user.GetErrors();

            var createRes = await adminUserAppService.CreateAsync(user, CancellationToken.None);

            return View();
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
}
