using System;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;

namespace WebCRUD.vNext.Infrastructure.Localization
{
    public class SingleResourceFileLocalizerFactory : HtmlLocalizerFactory
    {
        private readonly IStringLocalizerFactory _factory;

        public SingleResourceFileLocalizerFactory(IStringLocalizerFactory localizerFactory)
            : base(localizerFactory)
        {
            _factory = localizerFactory;
        }

        public override IHtmlLocalizer Create(Type resourceSource)
        {
            return base.Create(resourceSource);
        }

        public override IHtmlLocalizer Create(string baseName, string location)
        {
            if (baseName == null)
            {
                throw new ArgumentNullException(nameof(baseName));
            }

            if (location == null)
            {
                throw new ArgumentNullException(nameof(location));
            }

            var localizer = _factory.Create("Resources.Labels", location);
            return new HtmlLocalizer(localizer);
        }
    }
}
