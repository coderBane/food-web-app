using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Foody.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Foody.WebApi.Controllers.v1
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController : Controller
    {
        protected IUnitofWork _unitofWork;

        //public readonly IMapper _mapper;

        public BaseController(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }
    }
}

