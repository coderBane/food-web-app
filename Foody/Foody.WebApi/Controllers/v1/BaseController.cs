using AutoMapper;
using Foody.Data.Interfaces;
using Foody.Entities.Models;
using Microsoft.AspNetCore.Mvc;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Foody.WebApi.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/[controller]")]
    public class BaseController : ControllerBase
    {
        protected IUnitofWork _unitofWork;

        public readonly IMapper _mapper;

        public BaseController(IUnitofWork unitofWork, IMapper mapper)
        {
            _unitofWork = unitofWork;
            _mapper = mapper;
        }

        [NonAction]
        public static async Task Upload<T>(T entity, IFormFile file) where T : Item
        {
            if (file is null) return;

            using var ms = new MemoryStream();
            await file.CopyToAsync(ms);

            entity.ImageUri = Guid.NewGuid() + "-" + file.FileName;
            entity.ImageData = ms.ToArray();

            ms.Close();
            ms.Dispose();
        }
    }
}

