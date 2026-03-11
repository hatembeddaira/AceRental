using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Globalization;
using System.Text.Json;

namespace AceRental.Api.Configuration.Swagger
{
    /// <summary>
    /// Configuration des valeurs par defaut de Swagger
    /// </summary>
    public class SwaggerDefaultValues : IOperationFilter
    {
        /// <summary>
        /// Configuration
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="context"></param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation is not null && context is not null)
            {
                operation.Deprecated |= context.ApiDescription.IsDeprecated();

                HandleResponseType(operation, context);

                if (operation.Parameters == null)
                {
                    return;
                }

                HandleParameters(operation, context);
            }
        }

        private static void HandleResponseType(OpenApiOperation operation, OperationFilterContext context)
        {
            foreach (var responseType in context.ApiDescription.SupportedResponseTypes)
            {
                var responseKey = responseType.IsDefaultResponse
                                  ? "default"
                                  : responseType.StatusCode.ToString(CultureInfo.InvariantCulture);
                var response = operation.Responses![responseKey];

                foreach (var contentType in response.Content!.Keys)
                {
                    if (!responseType.ApiResponseFormats.Any(x => x.MediaType == contentType))
                    {
                        response.Content.Remove(contentType);
                    }
                }
            }
        }

        private static void HandleParameters(OpenApiOperation operation, OperationFilterContext context)
        {
            foreach (var parameter in operation.Parameters!)
            {
                var description = context.ApiDescription.ParameterDescriptions
                                                .First(p => p.Name == parameter.Name);

                parameter.Description ??= description.ModelMetadata?.Description;

                //if (parameter.Schema.Default == null && description.DefaultValue != null)
                //{
                //    var json = JsonSerializer.Serialize(
                //        description.DefaultValue,
                //        description.ModelMetadata!.ModelType);
                //    parameter.Schema.Default = OpenApiAnyFactory.CreateFromJson(json);
                //}

                //parameter.Required |= description.IsRequired;
            }
        }
    }
}
