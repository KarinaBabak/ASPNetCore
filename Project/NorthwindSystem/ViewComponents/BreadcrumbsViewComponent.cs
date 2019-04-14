using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using NorthwindSystem.Models;


namespace NorthwindSystem.ViewComponents
{
    public class Breadcrumbs : ViewComponent
    {
        private readonly string _indexActionName = "Index";
        public IViewComponentResult Invoke()
        {
            var currentBreadcrumbs = GetCurrentBreadcrumbTrail();
            return View(currentBreadcrumbs);
        }

        private IEnumerable<BreadcrumbItemViewModel> GetCurrentBreadcrumbTrail()
        {
            var breadcrumbs = new List<BreadcrumbItemViewModel>();
            var controller = ViewContext.RouteData.Values["controller"].ToString();
            var action = ViewContext.RouteData.Values["action"].ToString();

            if (controller.Equals("Home"))
            {
                return breadcrumbs;
            }

            breadcrumbs.Add(
                new BreadcrumbItemViewModel()
                {
                    ControllerName = controller,
                    Title = controller,
                    Path = _indexActionName,
                });

            if (!action.Equals(_indexActionName))
            {
                breadcrumbs.Add(new BreadcrumbItemViewModel()
                {
                    ControllerName = controller,
                    Title = action,
                    Path = $"{HttpContext.Request.Path}{HttpContext.Request.QueryString}",
                });
            }

            return breadcrumbs;
        }
    }
}
