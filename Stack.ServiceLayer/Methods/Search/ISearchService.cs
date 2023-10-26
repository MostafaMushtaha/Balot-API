using Stack.DTOs;
using Stack.DTOs.Requests.Search;
using Stack.DTOs.Responses.Search;

namespace Stack.ServiceLayer.Methods.users
{
  public interface ISearchService
    {
        public Task<ApiResponse<List<SearchResultsModel>>> SearchUsers(SearchModel model);
    }
}