using INFO_TRACK_WEB_SCRAPER.Interfaces;
using INFO_TRACK_WEB_SCRAPER.ViewModels;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;

namespace INFO_TRACK_WEB_SCRAPER
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            container.RegisterType<IWebScraperViewModel, WebScraperViewModel>();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}