using Castle.Core;
using Castle.DynamicProxy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace WinsorCore.Controllers
{
    [Interceptor(typeof(TestInterceptor))]
    public class HomeController : Controller
    {        
        public virtual IActionResult Index()
        {
            throw new NotImplementedException();

            return View();
        }
    }

    public class TestInterceptor : IInterceptor
    {
        private readonly ILogger _logger;
        public TestInterceptor(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public void Intercept(IInvocation invocation)
        {
            try
            {
                invocation.Proceed();
            }
            catch (Exception e)
            {
                _logger.LogDebug($"**********************************Opps:{e.Message}****************************************");
            }
        }
    }
}