using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace WebCRUD.vNext.Infrastructure.Web.TagHelpers
{
    /// <summary>
    /// Html helper extensions to work with collections of objects
    /// For details see: http://blog.stevensanderson.com/2010/01/28/editing-a-variable-length-list-aspnet-mvc-2-style/
    /// 
    /// Rewritten as MVC6 TagHelper
    /// 
    /// Usage:
    /// <code>
    /// <tr collection-item="Collection.Items">
    ///   <td></td>
    ///   <td></td>
    /// </tr>
    /// </code>
    /// 
    /// Renders as:
    /// <code>
    /// <tr>
    ///   <td></td>
    ///   <td></td>
    ///   <input type="hidden" name="Collection.Items.index" autocomplete="off" value="{GUID}" />
    /// </tr>
    /// </code>
    /// </summary>
    [HtmlTargetElement("tr", Attributes = CollectionItemAttribute)]
    public class CollectionItemTagHelper : TagHelper
    {
        private const string CollectionItemAttribute = "collection-item";
        private const string IdsToReuseKey = "__htmlPrefixScopeExtensions_IdsToReuse_";   

        [HtmlAttributeName(CollectionItemAttribute)]
        public string CollectionName { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (output == null)
                throw new ArgumentNullException(nameof(output));

            if (String.IsNullOrWhiteSpace(CollectionName))
                throw new ArgumentNullException(nameof(CollectionName));

            var idsToReuse = GetIdsToReuse(ViewContext.HttpContext, CollectionName);
            string itemIndex = idsToReuse.Count > 0 ? idsToReuse.Dequeue() : Guid.NewGuid().ToString();

            // autocomplete="off" is needed to work around a very annoying Chrome behaviour whereby it reuses old values after the user clicks "Back", which causes the xyz.index and xyz[...] values to get out of sync.
            string indexElement = String.Format("<input type=\"hidden\" name=\"{0}.index\" autocomplete=\"off\" value=\"{1}\" />", CollectionName,  System.Net.WebUtility.HtmlEncode(itemIndex));

            output.PostContent.AppendHtml(indexElement);
            output.TagName = "tr";

            new HtmlFieldPrefixScope(ViewContext.ViewData.TemplateInfo, string.Format("{0}[{1}]", CollectionName, itemIndex));
        }

        private static Queue<string> GetIdsToReuse(HttpContext httpContext, string collectionName)
        {
            // We need to use the same sequence of IDs following a server-side validation failure,  
            // otherwise the framework won't render the validation error messages next to each item.
            string key = IdsToReuseKey + collectionName;
            var queue = (Queue<string>)httpContext.Items[key];
            if (queue == null)
            {
                httpContext.Items[key] = queue = new Queue<string>();
                if (httpContext.Request.HasFormContentType)
                {
                    var previouslyUsedIds = httpContext.Request.Form[collectionName + ".index"];
                    if (!string.IsNullOrEmpty(previouslyUsedIds))
                        foreach (string previouslyUsedId in previouslyUsedIds)
                            queue.Enqueue(previouslyUsedId);
                }
            }
            return queue;
        }

        private class HtmlFieldPrefixScope : IDisposable
        {
            private readonly TemplateInfo templateInfo;
            private readonly string previousHtmlFieldPrefix;

            public HtmlFieldPrefixScope(TemplateInfo templateInfo, string htmlFieldPrefix)
            {
                this.templateInfo = templateInfo;

                previousHtmlFieldPrefix = templateInfo.HtmlFieldPrefix;
                templateInfo.HtmlFieldPrefix = htmlFieldPrefix;
            }

            public void Dispose()
            {
                templateInfo.HtmlFieldPrefix = previousHtmlFieldPrefix;
            }
        }
    }
}