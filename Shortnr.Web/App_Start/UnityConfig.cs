using Microsoft.Practices.Unity;
using Shortnr.Web.Business;
using Shortnr.Web.Business.Implementations;
using System;
using System.Web.Http;
using Unity.WebApi;

namespace Shortnr.Web
{
	public static class UnityConfig
	{
		private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
		{
			var container = new UnityContainer();
			RegisterTypes(container);
			return container;
		});

		public static IUnityContainer GetConfiguredContainer()
		{
			return container.Value;
		}

		public static void RegisterTypes(IUnityContainer container)
		{
			container.RegisterType<IUrlManager, UrlManager>();
			GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
		}
	}
}