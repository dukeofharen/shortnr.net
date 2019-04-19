using System.Linq;
using System.Web.Mvc;

using Unity.AspNet.Mvc;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Shortnr.Web.UnityMvcActivator), nameof(Shortnr.Web.UnityMvcActivator.Start))]
[assembly: WebActivatorEx.ApplicationShutdownMethod(typeof(Shortnr.Web.UnityMvcActivator), nameof(Shortnr.Web.UnityMvcActivator.Shutdown))]

namespace Shortnr.Web
{
    /// <summary>
    /// Provides the bootstrapping for integrating Unity with ASP.NET MVC.
    /// </summary>
    public static class UnityMvcActivator
    {
        /// <summary>
        /// Integrates Unity when the application starts.
        /// </summary>
        public static void Start() 
        {
            FilterProviders.Providers.Remove(FilterProviders.Providers.OfType<FilterAttributeFilterProvider>().First());
            FilterProviders.Providers.Add(new UnityFilterAttributeFilterProvider(UnityConfig.GetConfiguredContainer()));

            DependencyResolver.SetResolver(new UnityDependencyResolver(UnityConfig.GetConfiguredContainer()));
        }

        /// <summary>
        /// Disposes the Unity container when the application is shut down.
        /// </summary>
        public static void Shutdown()
        {
            UnityConfig.GetConfiguredContainer().Dispose();
        }
    }
}