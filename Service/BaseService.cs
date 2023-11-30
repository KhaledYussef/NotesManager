using Core.Domains;

using Data.DbContext;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

using Services.Extensions;
using Services.LoggerService;

using System.Net;
using System.Security.Claims;

namespace Services
{
    public abstract class BaseService<T> where T : class
    {
        private readonly AppDbContext _dbContext;
        private readonly ILoggerService<T> _logger;
        private readonly HttpContext _httpContext;
        public BaseService(AppDbContext dbContext, ILoggerService<T> logger, IHttpContextAccessor httpAccessor)
        {
            _dbContext = dbContext;
            _logger = logger;
            _httpContext = httpAccessor.HttpContext;
        }


        #region Props

        /// <summary>
        /// Current UserId
        /// </summary>
        internal string UserId
        {
            get
            {

                var customerId = _httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(customerId))
                    return null;

                return customerId;
            }
        }
        #endregion

    
        #region Error

        //============//============//============//============//============
        internal ResponseResult Error(Exception ex)
        {
            _logger.LogErrorAsync(typeof(T).ToString(), ex);

            return new ResponseResult
            {
                IsSuccess = false,
                Errors = new List<string> { ex.GetError() },
                StatusCode = HttpStatusCode.InternalServerError,
            };
        }

        //============//============//============//============//============
        internal ResponseResult Error(Exception ex, HttpStatusCode httpStatus)
        {
            _logger.LogErrorAsync(typeof(T).Name, ex);
            return new ResponseResult
            {
                IsSuccess = false,
                Errors = new List<string> { ex.GetError() },
                StatusCode = httpStatus,
            };
        }

        //============//============//============//============//============
        internal ResponseResult Error(string errorMsg)
        {
            _logger.LogErrorAsync(typeof(T).Name, errorMsg);
            return new ResponseResult
            {
                IsSuccess = false,
                Errors = new List<string> { errorMsg },
                StatusCode = HttpStatusCode.BadRequest,
            };
        }

        //============//============//============//============//============
        internal ResponseResult Error(string errorMsg, HttpStatusCode httpStatus)
        {
            _logger.LogErrorAsync(typeof(T).Name, errorMsg);
            return new ResponseResult
            {
                IsSuccess = false,
                Errors = new List<string> { errorMsg },
                StatusCode = httpStatus,
            };
        }

        //============//============//============//============//============
        internal ResponseResult Error(List<string> errorMsgs)
        {
            return new ResponseResult
            {
                IsSuccess = false,
                Errors = errorMsgs,
                StatusCode = System.Net.HttpStatusCode.InternalServerError,
            };
        }

        //============//============//============//============//============
        internal ResponseResult Error(List<string> errorMsgs, HttpStatusCode statusCode)
        {
            return new ResponseResult
            {
                IsSuccess = false,
                Errors = errorMsgs,
                StatusCode = statusCode,
            };
        }
        #endregion


        #region Success
        //============//============//============//============//============
        internal ResponseResult Success()
        {
            return new ResponseResult
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
            };
        }

        //============//============//============//============//============
        internal ResponseResult Success(HttpStatusCode statusCode)
        {
            return new ResponseResult
            {
                IsSuccess = true,
                StatusCode = statusCode,
            };
        }

        //============//============//============//============//============
        internal ResponseResult Success(object data)
        {
            return new ResponseResult
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Data = data,
            };
        }

        //============//============//============//============//============
        internal ResponseResult Success(object data, HttpStatusCode statusCode)
        {
            return new ResponseResult
            {
                IsSuccess = true,
                StatusCode = statusCode,
                Data = data,
            };
        }

        #endregion


        #region NotFound
        internal ResponseResult NotFound()
        {
            return new ResponseResult
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.NotFound,
            };
        }
        #endregion

    }


}
