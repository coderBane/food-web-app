using AutoMapper;
using Foody.Data.Interfaces;
using Foody.Data.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Foody.WebApi.Controllers.v1
{
    public class AuthBaseController : BaseController
    {
        protected readonly UserManager<IdentityUser> _userManager;

        protected readonly RoleManager<IdentityRole> _roleManager;

        public AuthBaseController(IUnitofWork unitofWork, IMapper mapper, ICacheService cacheService,
            UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
            : base(unitofWork, mapper, cacheService)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
    }
}

