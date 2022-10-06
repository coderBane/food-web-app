using AutoMapper;
using Foody.Data.Services;
using Foody.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Foody.WebApi.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [Route("v{version:apiVersion}/[controller]")]
    public class BaseController : ControllerBase
    {
        private readonly ICacheService _cacheService;

        protected IUnitofWork _unitofWork;

        protected string _cached = "";

        public readonly IMapper _mapper;

        public BaseController(IUnitofWork unitofWork, IMapper mapper, ICacheService cacheService)
        {
            _cacheService = cacheService;
            _unitofWork = unitofWork;
            _mapper = mapper;
        }

        internal T? GetCache<T>(string key)
        {
            var cacheData = _cacheService.GetData<T>(key);
            return cacheData is null ? default :
                cacheData is IEnumerable<object> en ? (en.Any() ? cacheData : default) : cacheData;
        }

        internal void SetCache<T>(string key, T data, string? collectionKey = default)
        {
            var expiryTime = DateTimeOffset.Now.AddMinutes(5);
            _cacheService.SetData(key, data, expiryTime);

            if (!string.IsNullOrEmpty(collectionKey))
                DeleteCache(collectionKey);
        }

        internal void DeleteCache(string key) => _cacheService.RemoveData(key);

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

        internal IActionResult? ValidateModel<T,D>(T entity, D dto)
        {
            var result = new Result<dynamic>();
            ModelState.ClearValidationState(nameof(dto));

            if (!TryValidateModel(entity!))
            {
                result.Error = AddError(422,
                    ErrorsMessage.Generic.ValidationError,
                    string.Empty,
                    ModelState);

                //WatchDog.WatchLogger.Log(result.Error.Title + " : " + result.Error.Message);

                return UnprocessableEntity(result);
            }

            return null;
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

