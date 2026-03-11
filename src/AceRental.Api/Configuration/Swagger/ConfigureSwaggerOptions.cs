using Asp.Versioning.ApiExplorer;
using Asp.Versioning;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi;

namespace AceRental.Api.Configuration.Swagger
{
    /// <summary>
    /// Configuration Swagger
    /// </summary>
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider provider;

        /// <summary>
        /// Option de documentation
        /// </summary>
        /// <param name="provider"></param>
        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) => this.provider = provider;

        /// <summary>
        /// Configuration de la documentation
        /// </summary>
        /// <param name="options"></param>
        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(
                    description.GroupName,
                    CreateInfoForApiVersion(description));
            }
        }

        private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var text = new StringBuilder("Documentation");
            var info = new OpenApiInfo
            {
                Title = "Api",
                Version = description.ApiVersion.ToString(),
                Contact = new OpenApiContact { Name = "Ace Sound", Email = "support@ace-sound.com" },
            };

            if (description.IsDeprecated)
            {
                text.Append(" This version is outdated.");
            }

            text = HandleSunsetPolicy(description, text);

            text.Append("<h4>Additional informations</h4>");
            info.Description = text.ToString();

            return info;
        }

        private static StringBuilder HandleSunsetPolicy(ApiVersionDescription description, StringBuilder text)
        {
            if (description.SunsetPolicy is { } policy)
            {
                text = HandleDateSunsetPolicy(text, policy);

                text = HandleLinkSunsetPolicy(text, policy);
            }

            return text;
        }

        private static StringBuilder HandleDateSunsetPolicy(StringBuilder text, SunsetPolicy policy)
        {
            if (policy.Date is { } when)
            {
                text.Append(" Available the ")
                    .Append(when.Date.ToShortDateString())
                    .Append('.');
            }

            return text;
        }

        private static StringBuilder HandleLinkSunsetPolicy(StringBuilder text, SunsetPolicy policy)
        {
            if (policy.HasLinks)
            {
                text.AppendLine();

                var rendered = false;

                for (var i = 0; i < policy.Links.Count; i++)
                {
                    var link = policy.Links[i];

                    if (link.Type != "text/html")
                    {
                        continue;
                    }

                    if (!rendered)
                    {
                        text.Append("<h4>Links</h4><ul>");
                        rendered = true;
                    }

                    text.Append("<li><a href=\"");
                    text.Append(link.LinkTarget.OriginalString);
                    text.Append("\">");
                    text.Append(
                        StringSegment.IsNullOrEmpty(link.Title)
                        ? link.LinkTarget.OriginalString
                        : link.Title.ToString());
                    text.Append("</a></li>");
                }

                if (rendered)
                {
                    text.Append("</ul>");
                }
            }

            return text;
        }
    }
}
