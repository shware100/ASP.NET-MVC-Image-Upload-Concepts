using Antlr.Runtime.Misc;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

public static class HtmlHelperExtensions
{
    // this routine was taken from one of many examples available online - not sure which one...
    public static MvcHtmlString Image(this HtmlHelper helper, string url,  string altText, object htmlAttributes = null)
    {
        TagBuilder builder = new TagBuilder("img");
        builder.Attributes.Add("src", System.Web.VirtualPathUtility.ToAbsolute(url));
        builder.Attributes.Add("alt", altText);
        if (htmlAttributes != null)
          builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
        return MvcHtmlString.Create(builder.ToString(TagRenderMode.Normal));
    }
    // following routines taken from: http://www.c-sharpcorner.com/Blogs/8622/Asp-Net-mvc-imagefor-htmlhelper.aspx
    public static MvcHtmlString ImageFor<TModel, TProperty>(
               this HtmlHelper<TModel> htmlHelper,
               Expression<Func<TModel, TProperty>> expression)
    {
        var imgUrl = expression.Compile()(htmlHelper.ViewData.Model);
        return BuildImageTag(imgUrl.ToString(), null);
    }
    public static MvcHtmlString ImageFor<TModel, TProperty>(
        this HtmlHelper<TModel> htmlHelper,
        Expression<Func<TModel, TProperty>> expression,
        object htmlAttributes)
    {
        var imgUrl = expression.Compile()(htmlHelper.ViewData.Model);
        return BuildImageTag(imgUrl.ToString(), htmlAttributes);
    }

    private static MvcHtmlString BuildImageTag(string imgUrl, object htmlAttributes)
    {
        TagBuilder tag = new TagBuilder("img");

        tag.Attributes.Add("src", imgUrl);
        if (htmlAttributes != null)
            tag.MergeAttributes(new RouteValueDictionary(htmlAttributes));

        return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
    }
}

