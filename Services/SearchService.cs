using Examine;
using Examine.Search;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models.PublishedContent;
using uTPro.Feature.SearchPlus.Services;
using Umbraco.Cms.Core.Models;
using Umbraco.Extensions;
using Umbraco.Cms.Core.Services;
using SliceByAlice.Models;

namespace SliceByAlice.Services
{
    public interface ISearchService
    {
        IReadOnlyList<SearchResultDto> Search(string query, int take = 20);
    }
    public class SearchService : ISearchService
{
    private readonly IExamineManager _examineManager;
    private readonly ISynonymProvider _synonyms;
    private readonly IPublishedContentQuery _publishedContentQuery;

    public SearchService(
        IExamineManager examineManager,
        ISynonymProvider synonyms,
        IPublishedContentQuery publishedContentQuery)
    {
        _examineManager = examineManager;
        _synonyms = synonyms;
        _publishedContentQuery = publishedContentQuery;
    }

    public IReadOnlyList<SearchResultDto> Search(string query, int take = 20)
    {
        if (string.IsNullOrWhiteSpace(query))
            return [];

        if (!_examineManager.TryGetIndex(
                Constants.UmbracoIndexes.ExternalIndexName,
                out var index))
            return [];

        var terms = _synonyms.Expand(query.Trim());
        if (terms.Count == 0)
            return [];

        var searchQuery = index.Searcher.CreateQuery("content");
        var operation = searchQuery.ManagedQuery(terms[0]);

        for (var indexNumber = 1; indexNumber < terms.Count; indexNumber++)
            operation = operation.Or().ManagedQuery(terms[indexNumber]);

        return operation
            .Execute(QueryOptions.SkipTake(0, take))
            .Select(MapResult)
            .ToList();
    }

    private SearchResultDto MapResult(ISearchResult result)
    {
        var id = result.Values.TryGetValue("__NodeId", out var nodeId) &&
                 int.TryParse(nodeId, out var parsedId)
            ? parsedId
            : 0;

        var content = id > 0
            ? _publishedContentQuery.Content(id)
            : null;

        var image =
            content?.Value<MediaWithCrops>("searchImage") ??
            content?.Value<MediaWithCrops>("image");

        string name = "";

        if (!string.IsNullOrWhiteSpace(content?.Value<string>("searchHeading")))
            name = content.Value<string>("searchHeading") ?? "";
        else if (!string.IsNullOrWhiteSpace(content?.Value<string>("heading")))
            name = content.Value<string>("heading") ?? "";
        else if (!string.IsNullOrWhiteSpace(content?.Name))
            name = content.Name ?? "";

        string summary = "";
        if (!string.IsNullOrWhiteSpace(content?.Value<string>("searchDescription")))
            summary = content.Value<string>("searchDescription") ?? "";
        else if (!string.IsNullOrWhiteSpace(content?.Value<string>("preamble")))
            summary = content.Value<string>("preamble") ?? ""; 

        return new SearchResultDto
        {
            Id = id,
            ContentType = content?.ContentType.Alias ?? string.Empty,
            Name = name,
            Url = content?.Url() ?? string.Empty,
            Summary = summary,
            ImageUrl = image?.GetCropUrl(
                width: 400,
                height: 400,
                quality: 80,
                furtherOptions: "&format=webp") ?? string.Empty
        };
    }
}
}