using AutoMapper;
using Foody.Data.Interfaces;
using Foody.Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Foody.WebApi.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Produces("application/json")]
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

        internal static Error AddError(int code, string title, string message, ModelStateDictionary? modelState = default)
        {
            string? error = modelState is not null ?
                modelState.Values.SelectMany(v => v.Errors)
                                 .Select(v => v.ErrorMessage)
                                 .Aggregate((k, v) => $"{k} \n {v}") : default;

            return new Error
            {
                Code = code,
                Title = title,
                Message = error ?? message
            };
        }

        internal static async Task Upload<T>(T entity, IFormFile file) where T : Item
        {
            if (file is not null)
            {
                using var ms = new MemoryStream();
                await file.CopyToAsync(ms);

                entity.Image = new AppFile
                {
                    Content = ms.ToArray(),
                    UntrustedName = file.FileName,
                    Size = file.Length,
                    FileExtension = Path.GetExtension(file.FileName)
                };

                entity.ImageUri = (Guid.NewGuid() + "-" + file.FileName).Replace(" ", "");

                ms.Close();
                ms.Dispose();
            }
        }
    }
}

