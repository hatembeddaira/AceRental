using AceRental.Application.Reservations.Dtos;
using Microsoft.OData.ModelBuilder;
using Asp.Versioning;
using Asp.Versioning.OData;
using AceRental.Domain.Enum;
using AceRental.Application.Reservations.Command;

namespace AceRental.Api.Configuration.OData
{
    public class ReservationsConfiguration : IModelConfiguration
    {
        public void Apply(ODataModelBuilder builder, ApiVersion apiVersion, string? routePrefix)
        {
            if (builder is null)
            {
                return;
            }

            if (apiVersion == ApiVersions.V1)
            {
                var dto = builder.EntitySet<ReservationDetailsDto>("Reservations").EntityType;
                dto.HasKey(e => e.Id);
                dto.HasMany(r => r.Equipments).Name = "Equipments";
                dto.HasMany(r => r.Packs).Name = "Packs";
                dto.HasMany(r => r.Services).Name = "Services";
                dto.HasMany(r => r.Invoices).Name = "Invoices";
                dto.HasMany(r => r.Quotes).Name = "Quotes";
                dto.HasMany(r => r.Payments).Name = "Payments";
                dto.HasRequired(r => r.Client);
                var getTimeline = dto.Collection.Function("Timeline");
                getTimeline.ReturnsCollection<ReservationTimelineDto>();

                // // Action pour le statut logistique
                // var logisticAction = dto.Action("LogisticStatus");
                // logisticAction.EntityParameter<ChangeLogisticStatusCommand>("command"); 
                // logisticAction.Returns<bool>();

                // // Action pour le statut financier
                // var financialAction = dto.Action("FinancialStatus");
                // financialAction.Parameter<FinancialStatus>("Status");
                // financialAction.Returns<bool>();

                // var quoteAction = dto.Collection.Function("Quote");                 
                // quoteAction.Parameter<Guid>("key");
                // quoteAction.Returns<Guid>();

            }


        }
    }
}