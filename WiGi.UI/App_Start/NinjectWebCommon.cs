using WiGi.UI;

[assembly: WebActivator.PreApplicationStartMethod(typeof(NinjectWebCommon), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(NinjectWebCommon), "Stop")]

namespace WiGi.UI
{
	using System;
	using System.Web;
	using WiGi.Data;
	using WiGi.Data.Account.Repositories;
	using WiGi.Data.Wiki.Repositories;
	using Microsoft.Web.Infrastructure.DynamicModuleHelper;
	using Ninject;
	using Ninject.Extensions.Conventions;
	using Ninject.Web.Common;
	using WiGi.Services;
	using WiGi.Wiki;
	using WiGi.Wiki.Services;

	public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
            
            RegisterServices(kernel);
            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
		{
			kernel.Bind<WiGiCtx>().ToSelf().InRequestScope();

			// bind account repositories
			kernel.Bind(x => x
				.FromAssemblyContaining<UserRepository>()
				.SelectAllClasses().InNamespaceOf<UserRepository>()
				.BindAllInterfaces()
				.Configure(c => c.InRequestScope())
			);

			// bind wiki repositories
			kernel.Bind(x => x
				.FromAssemblyContaining<PageRepository>()
				.SelectAllClasses().InNamespaceOf<PageRepository>()
				.BindAllInterfaces()
				.Configure(c => c.InRequestScope())
			);

	        kernel.Bind<IPageService>().To<PageService>();
	        kernel.Bind<IContentProvider>().ToMethod(ctx => new FileContentProvider()).InRequestScope();

			kernel.Bind<ISettingsProvider>().To<ConfigFileSettingsProvider>().InSingletonScope();

	        kernel.Get<WG>();
		}        
    }
}
