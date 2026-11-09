using Microsoft.AspNetCore.Mvc;
using SliceByAlice.Services;

namespace SliceByAlice.Controllers
{
    public class SearchController : Controller
    {
        private readonly ISearchService _searchService;

        public SearchController(ISearchService searchService)
        {
            _searchService = searchService;
        }

        [HttpGet("/search")]
        public IActionResult Search([FromQuery] string q)
        {
            var results = _searchService.Search(q);

            return Ok(new
            {
                total = results.Count,
                results
            });
        }
    }
}