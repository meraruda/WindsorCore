using Castle.DynamicProxy;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.MsDependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace WinsorCore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            _container = new WindsorContainer();
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        static IWindsorContainer _container;

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {

            services
                .AddMvc()
                .AddControllersAsServices(); //-----¡õ¡õ¡õ¡õ¡õ¡õ
            /*
            By default, ASP.NET Core will resolve the controller parameters from the container but doesn¡¦t actually resolve the controller from the container. 
            This usually isn¡¦t an issue but it does mean:
            The lifecycle of the controller is handled by the framework, not the request lifetime.
            The lifecycle of controller constructor parameters is handled by the request lifetime.
            Special wiring that you may have done during registration of the controller (like setting up property injection) won¡¦t work.
             */

            var assembly = Assembly.GetEntryAssembly();

            _container.Register(
                Classes.FromAssembly(assembly)
                .IncludeNonPublicTypes()
                .BasedOn<IInterceptor>()
                .LifestyleTransient()
                );     

            return WindsorRegistrationHelper.CreateServiceProvider(_container, services);

            //return new WindsorServiceProvider(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }     

            app.UseStaticFiles();
                   
            app.UseMvc(r =>
            {
                //r.MapRoute(
                //        name: "default",
                //        template: "{controller=Home}/{action=Index}/{id?}"
                //    );
                r.MapRoute(
                        name: "default",
                        template: "{controller}/{action}/{id?}",
                        defaults: new { controller = "Home", action = "Index" }
                    );
            });
            //app.UseMvcWithDefaultRoute();
        }
    } 
}
