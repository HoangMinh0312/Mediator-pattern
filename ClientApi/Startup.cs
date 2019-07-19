using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.Windsor;
using ClientApi.Infrastructure;
using Service.Clients;
using Service.ClientUsers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Castle.MicroKernel.Lifestyle;
using Castle.MicroKernel.Registration;

namespace ClientApi
{
    public class Startup
    {
        private readonly WindsorContainer container = new WindsorContainer();

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddScoped<IClientService, ClientService>();
            services.AddScoped<IClientUserService, ClientUserService>();
            services.AddScoped<IClientUserService, ClientUserService>();
            container.AddMediator().AddService();
            services.AddCustomControllerActivation(container.Resolve);
            services.AddRequestScopingMiddleware(container.BeginScope);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            container.Register(Component.For<IConfiguration>()
                          .UsingFactoryMethod<IConfiguration>(t => app.ApplicationServices.GetService<IConfiguration>()));
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            foreach (var controllerType in app.GetControllerTypes())
                container.Register(Component.For(controllerType).ImplementedBy(controllerType).LifestyleScoped());

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
