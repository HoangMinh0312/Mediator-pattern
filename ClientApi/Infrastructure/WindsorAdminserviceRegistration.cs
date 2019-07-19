using Castle.MicroKernel.Registration;
using Castle.Windsor;
using LowellMediator.Implements;
using LowellMediator.Interfaces;
using Microsoft.Extensions.PlatformAbstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ClientApi.Infrastructure
{
    public static class WindsorAdminserviceRegistration
    {
        public static IWindsorContainer AddMediator(this IWindsorContainer container)
        {
            container.Register(Component.For<IMediator>().ImplementedBy<Mediator>().LifestyleTransient());
            container.Register(Component.For<InnerInstanceFactory>()
                .UsingFactoryMethod<InnerInstanceFactory>(k => t => k.Resolve(t)));
            container.Register(Component.For<PreInstanceFactory>()
                .UsingFactoryMethod<PreInstanceFactory>(k => t =>
                {
                    if (k.HasComponent(t))
                    {
                        return k.Resolve(t);
                    }

                    return null;
                }));
            container.Register(Component.For<PostInstanceFactory>()
              .UsingFactoryMethod<PostInstanceFactory>(k => t =>
              {
                  if (k.HasComponent(t))
                  {
                      return k.Resolve(t);
                  }

                  return null;
              }));

            var filterByAssembly = new AssemblyFilter(Path.Combine(PlatformServices.Default.Application.ApplicationBasePath));

            var assembly = Classes.FromAssemblyInDirectory(filterByAssembly);

            container.Register(assembly.BasedOn(typeof(IPreProcessor<>))
                .WithServiceAllInterfaces().LifestyleTransient());

            container.Register(
                assembly.BasedOn(typeof(IPostProcessor<>)).WithServiceAllInterfaces().LifestyleTransient());

            container.Register(assembly
                .BasedOn(typeof(IAsyncCommandHandler<,>))
                .Configure(f => f.IsFallback())
                .WithService.AllInterfaces()
                .LifestyleTransient());

            container.Register(assembly
                .BasedOn(typeof(IAsyncQueryHandler<,>))
                .Configure(f => f.IsFallback())
                .WithService.AllInterfaces()
                .LifestyleTransient());



            return container;
        }

        public static IWindsorContainer AddService(this IWindsorContainer container)
        {
            var filterByAssembly = new AssemblyFilter(Path.Combine(PlatformServices.Default.Application.ApplicationBasePath));
            container.Register(Classes.FromAssemblyNamed("Service")
             .Where(t => t.Name.EndsWith("Service")).WithServiceAllInterfaces().LifestyleScoped());
            return container;
        }

    }
}
