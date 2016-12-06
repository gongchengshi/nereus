using System.Web.Mvc;
using Glimpse.Core.Extensibility;
using Ninject.Web.Mvc.FilterBindingSyntax;
using Nereus.Filters;
using Nereus.Models;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Nereus.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(Nereus.App_Start.NinjectWebCommon), "Stop")]

namespace Nereus.App_Start
{
   using System;
   using System.Web;

   using Microsoft.Web.Infrastructure.DynamicModuleHelper;

   using Ninject;
   using Ninject.Web.Common;

   public static class NinjectWebCommon
   {
      private static readonly Bootstrapper _bootstrapper = new Bootstrapper();
      private static StandardKernel _kernel;

      /// <summary>
      /// Starts the application
      /// </summary>
      public static void Start()
      {
         DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
         DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
         _bootstrapper.Initialize(CreateKernel);
      }

      /// <summary>
      /// Stops the application.
      /// </summary>
      public static void Stop()
      {
         _bootstrapper.ShutDown();
      }

      /// <summary>
      /// Creates the kernel that will manage your application.
      /// </summary>
      /// <returns>The created kernel.</returns>
      private static IKernel CreateKernel()
      {
         _kernel = new StandardKernel();
         try
         {
            _kernel.Bind<NereusDb>().ToSelf().InRequestScope();
            _kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            _kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
            _kernel.BindFilter<CreateUserAttribute>(FilterScope.Controller, 0);

            RegisterServices(_kernel);
            return _kernel;
         }
         catch
         {
            _kernel.Dispose();
            throw;
         }
      }

      /// <summary>
      /// Load your modules or register your services here!
      /// </summary>
      /// <param name="kernel">The kernel.</param>
      private static void RegisterServices(IKernel kernel)
      {
      }
   }
}
