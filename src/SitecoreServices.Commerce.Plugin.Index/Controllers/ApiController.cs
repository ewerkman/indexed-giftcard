using System;
using System.Threading.Tasks;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OData.Edm;
using Sample.Commerce.Plugin.Index.Commands;
using Sitecore.Commerce.Core;

namespace Sample.Commerce.Plugin.Index.Controllers
{
    public class ApiController : CommerceODataController
    {
        private const string RequiredErrorMessage = "This value is required";
        private const string MalformedJsonErrorMessage = "This value is malformed and can't be deserialized to";
        
        public ApiController(IServiceProvider serviceProvider, CommerceEnvironment globalEnvironment) 
            : base(serviceProvider, globalEnvironment)
        {
        }
        
        [HttpPost]
        [ODataRoute("GetGiftCards()", RouteName = CoreConstants.CommerceApi)]
        public async Task<IActionResult> GetGiftCards([FromBody] ODataActionParameters parameters)
        {
            
            if (!parameters.ContainsKey("startDate"))
            {
                parameters.Add("startDate", RequiredErrorMessage);
                return new BadRequestObjectResult(parameters);
            }

            if (!parameters.ContainsKey("endDate"))
            {
                parameters.Add("endDate", RequiredErrorMessage);
                return new BadRequestObjectResult(parameters);
            }

            var startDate = (DateTimeOffset) parameters["startDate"];
            var endDate =  (DateTimeOffset) parameters["endDate"];
            
            var result = await Command<GetGiftCardsCommand>().Process(CurrentContext, startDate, endDate);
            
            return new ObjectResult(result);
        }
    }
}