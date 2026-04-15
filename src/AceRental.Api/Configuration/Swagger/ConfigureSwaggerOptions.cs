using Asp.Versioning.ApiExplorer;
using Asp.Versioning;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;

namespace AceRental.Api.Configuration.Swagger
{
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider provider;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
            => this.provider = provider;

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
                Title = "AceRental API",
                Version = description.ApiVersion.ToString(),
                Contact = new OpenApiContact
                {
                    Name = "Ace Sound",
                    Email = "support@ace-sound.com"
                },
            };

            if (description.IsDeprecated)
                text.Append(" Cette version est obsolète.");

            text = HandleSunsetPolicy(description, text);
            text.Append("<h4>Informations complémentaires</h4>");
            info.Description = text.ToString();

            return info;
        }

        private static StringBuilder HandleSunsetPolicy(ApiVersionDescription description, StringBuilder text)
        {
            if (description.SunsetPolicy is { } policy)
            {
                if (policy.Date is { } when)
                {
                    text.Append(" Disponible jusqu'au ")
                        .Append(when.Date.ToShortDateString())
                        .Append('.');
                }

                if (policy.HasLinks)
                {
                    text.AppendLine();
                    var rendered = false;

                    foreach (var link in policy.Links)
                    {
                        if (link.Type != "text/html") continue;

                        if (!rendered)
                        {
                            text.Append("<h4>Liens</h4><ul>");
                            rendered = true;
                        }

                        text.Append("<li><a href=\"")
                            .Append(link.LinkTarget.OriginalString)
                            .Append("\">")
                            .Append(StringSegment.IsNullOrEmpty(link.Title)
                                ? link.LinkTarget.OriginalString
                                : link.Title.ToString())
                            .Append("</a></li>");
                    }

                    if (rendered)
                        text.Append("</ul>");
                }
            }

            return text;
        }
    }
}