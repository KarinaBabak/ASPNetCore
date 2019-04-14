using Microsoft.AspNetCore.Razor.TagHelpers;


namespace NorthwindSystem.Helpers
{
    [HtmlTargetElement("a", Attributes = "northwind-image-id")]
    public class NorthwindImageLinkTagHelper : TagHelper
    {
        public int NorthwindImageId { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.SetAttribute("href", $"/images/{NorthwindImageId}");
        }
    }
}
