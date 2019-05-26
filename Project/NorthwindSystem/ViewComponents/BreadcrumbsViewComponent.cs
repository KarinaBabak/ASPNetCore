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
            var controller = ViewContext.RouteData.Values["controller"]?.ToString() ?? string.Empty;
            var action = ViewContext.RouteData.Values["action"]?.ToString() ?? string.Empty;
            if (controller.Equals("Home"))
            {
                return breadcrumbs;
            }
            
            var title = string.IsNullOrEmpty(controller) ? GetLastUrlPath() : string.Empty;

            breadcrumbs.Add(
                new BreadcrumbItemViewModel()
                {
                    ControllerName = controller,
                    Title = title,
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

        private string GetLastUrlPath()
        {
            var pathElements = HttpContext.Request.Path.ToString().Split('/');
            return pathElements != null && pathElements.Length > 0 ? pathElements[pathElements.Length - 1] : string.Empty;
        }
    }
}
