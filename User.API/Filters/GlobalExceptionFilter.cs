﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User.API.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        /// <summary>
        /// hosting 环境 返回堆栈信息
        /// </summary>
        private readonly IHostingEnvironment _env;
        private ILogger<GlobalExceptionFilter> _logger;
        public GlobalExceptionFilter(IHostingEnvironment env, ILogger<GlobalExceptionFilter> logger)
        {
            _env = env;
            _logger = logger;
        }
        public void OnException(ExceptionContext context)
        {
            var json = new JsonErrorResponse() ;
            if (context.Exception.GetType() == typeof(UserOperationException))
            {
                json.Message = context.Exception.Message;
                context.Result = new BadRequestObjectResult(json);
            }
            else
            {
                json.Message = "网络错误";
                if (_env.IsDevelopment())  //测试环境，记录堆栈信息
                {
                    json.DevelopMessage = context.Exception.StackTrace;
                }
                context.Result = new InternalServerErrorObjectResult(json);
            }
            _logger.LogError(context.Exception, context.Exception.Message);
            context.ExceptionHandled = true;
        }
    }
    public class InternalServerErrorObjectResult : ObjectResult
    {
        public InternalServerErrorObjectResult(object error) : base(error)
        {
            //返回异常http state code
            StatusCode = StatusCodes.Status500InternalServerError;
        }
    }

}
