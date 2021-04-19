using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Core.Commands;
using Sitecore.Commerce.Plugin.GiftCards;
using Sitecore.Commerce.Plugin.Search;

namespace Sample.Commerce.Plugin.Index.Commands
{
    public class GetGiftCardsCommand : CommerceCommand
    {
        private readonly CommerceCommander _commander;

        public GetGiftCardsCommand(CommerceCommander commander)
        {
            _commander = commander;
        }

        public virtual async Task<IEnumerable<GiftCard>> Process(CommerceContext commerceContext,
            DateTimeOffset startDate, DateTimeOffset endDate)
        {
            // Filter order on activation date
            var filterQuery = new FilterQuery(new AndFilterNode(new LessThanFilterNode(
                    new FieldNameFilterNode("activationdate"),
                    new FieldValueFilterNode(endDate
                            .ToString(SearchConstants.DateTimeSearchFormat, CultureInfo.InvariantCulture),
                        FilterNodeValueType.Date)),
                new GreaterThanFilterNode(new FieldNameFilterNode("activationdate"),
                    new FieldValueFilterNode(
                        startDate
                            .ToString(SearchConstants.DateTimeSearchFormat, CultureInfo.InvariantCulture),
                        FilterNodeValueType.Date))));

            var scope = SearchScopePolicy.GetPolicyByType(commerceContext, commerceContext.Environment,
                typeof(GiftCard));

            var searchResults = await _commander.Command<SearchEntitiesCommand>()
                .Process<GiftCard>(commerceContext, scope.Name, new SearchQuery(), filterQuery).ConfigureAwait(false);

            return searchResults.OrderByDescending(g => g.ActivationDate);
        }
    }
}