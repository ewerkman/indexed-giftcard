using System;
using Microsoft.AspNet.OData.Builder;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.GiftCards;
using Sitecore.Framework.Pipelines;

namespace Sample.Commerce.Plugin.Index
{
    public class ConfigureServiceApiBlock : SyncPipelineBlock<ODataConventionModelBuilder, ODataConventionModelBuilder, CommercePipelineExecutionContext>
    {
        public override ODataConventionModelBuilder Run(ODataConventionModelBuilder arg, CommercePipelineExecutionContext context)
        {
            var getGiftCardsAction = arg.Action("GetGiftCards");
            getGiftCardsAction.Parameter<DateTimeOffset>("startDate");
            getGiftCardsAction.Parameter<DateTimeOffset>("endDate");
            getGiftCardsAction.ReturnsCollection<GiftCard>();
            
            return arg;
        }
    }
}