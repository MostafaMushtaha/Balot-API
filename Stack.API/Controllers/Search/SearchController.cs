using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stack.API.Controllers.Common;
using Stack.DTOs.Requests.Search;
using Stack.ServiceLayer.Methods.users;

namespace Stack.API.Controllers.Spotlight
{
    [Route("api/Search")]
    [ApiController]
    [Authorize]
    public class SearchController : BaseResultHandlerController<ISearchService>
    {
        public SearchController(ISearchService _service) : base(_service) { }



        [HttpPost("SearchUsers")]
        public async Task<IActionResult> SearchUsers(SearchModel model)
        {
            return await AddItemResponseHandler(async () => await service.SearchUsers(model));
        }

        
    }

}
