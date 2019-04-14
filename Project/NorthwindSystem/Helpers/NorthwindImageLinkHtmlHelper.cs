using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace NorthwindSystem.Helpers
{
    public static class NorthwindImageLinkHtmlHelper
    {
        public static IHtmlContent NorthwindImageLink(this IHtmlHelper helper, int imageId, string linkText)
        {
            return new HtmlString($"<a href='/images/{imageId}'>{linkText}</a>");
        }
    }
}
