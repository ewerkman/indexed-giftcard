using System;
using System.Linq;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Commerce.Plugin.GiftCards;
using Sitecore.Commerce.Plugin.Search;
using Sitecore.Framework.Conditions;
using Sitecore.Framework.Pipelines;

namespace Sample.Commerce.Plugin.Index.Pipelines.Blocks
{
    [PipelineDisplayName("InitializeGiftcardsIndexingViewBlock")]
    public class InitializeGiftcardsIndexingViewBlock : SyncPipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
    {
          /// <summary>
        /// Runs the specified argument.
        /// </summary>
        /// <param name="arg">The argument.</param>
        /// <param name="context">The context.</param>
        /// <returns>An <see cref="EntityView"/></returns>
        public override EntityView Run(EntityView arg, CommercePipelineExecutionContext context)
        {
            Condition.Requires(arg).IsNotNull($"{Name}: argument cannot be null.");

            var argument = context.CommerceContext.GetObjects<SearchIndexMinionArgument>().FirstOrDefault();
            if (string.IsNullOrEmpty(argument?.Policy?.Name))
            {
                return arg;
            }

            var giftCards = argument.Entities?.OfType<GiftCard>().ToList();
            if (giftCards == null || !giftCards.Any())
            {
                return arg;
            }

            var searchViewNames = context.GetPolicy<KnownSearchViewsPolicy>();

            giftCards.ForEach(
                giftCard =>
                {
                    var documentView = arg.ChildViews.Cast<EntityView>()
                        .FirstOrDefault(v => v.EntityId.Equals(giftCard.Id, StringComparison.OrdinalIgnoreCase) && v.Name.Equals(searchViewNames.Document, StringComparison.OrdinalIgnoreCase));
                    if (documentView == null)
                    {
                        documentView = new EntityView
                        {
                            Name = context.GetPolicy<KnownSearchViewsPolicy>().Document,
                            EntityId = giftCard.Id
                        };
                        arg.ChildViews.Add(documentView);
                    }

                    AddIndexedProperties(context, documentView, giftCard);
                });

            return arg;
        }

          private void AddIndexedProperties(CommercePipelineExecutionContext context, EntityView documentView, GiftCard giftCard)
          {
              documentView.Properties.Add(new ViewProperty
              {
                  Name = "EntityUniqueId",
                  RawValue = giftCard.UniqueId.ToString()
              });
              documentView.Properties.Add(new ViewProperty
              {
                  Name = "EntityId",
                  RawValue = giftCard.Id
              });
              documentView.Properties.Add(new ViewProperty
              {
                  Name = "GiftCardCode",
                  RawValue = giftCard.GiftCardCode
              });
              documentView.Properties.Add(new ViewProperty
              {
                  Name = "ActivationDate",
                  RawValue = giftCard.ActivationDate
              });
              documentView.Properties.Add(new ViewProperty
              {
                  Name = "Balance",
                  RawValue = giftCard.Balance.Amount
              });
              documentView.Properties.Add(new ViewProperty
              {
                  Name = "OriginalAmount",
                  RawValue = giftCard.OriginalAmount.Amount
              });
              documentView.Properties.Add(new ViewProperty
              {
                  Name = "DateCreated",
                  RawValue = giftCard.DateCreated
              });
              documentView.Properties.Add(new ViewProperty
              {
                  Name = "DateUpdated",
                  RawValue = giftCard.DateUpdated
              });
              documentView.Properties.Add(new ViewProperty
              {
                  Name = "ArtifactStoreId",
                  RawValue = context.CommerceContext.Environment.ArtifactStoreId
              });
          }
    }
}