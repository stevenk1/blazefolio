using BlazeFolio.Domain.Models;
using BlazeFolio.Domain.Providers;
using CSharpFunctionalExtensions;
using MediatR;

namespace BlazeFolio.Application.Wallets.Features.Queries;

public record GetSymbolMetadata(IEnumerable<string> Symbols) : IRequest<Result<Dictionary<string, MarketMetadata?>>>;

public class GetSymbolMetadataHandler : IRequestHandler<GetSymbolMetadata, Result<Dictionary<string, MarketMetadata?>>>
{
    private readonly IMarketMetadataProvider _metadataProvider;

    public GetSymbolMetadataHandler(IMarketMetadataProvider metadataProvider)
    {
        _metadataProvider = metadataProvider;
    }

    public async Task<Result<Dictionary<string, MarketMetadata?>>> Handle(GetSymbolMetadata request, CancellationToken cancellationToken)
    {
        try
        {
            var metadata = await _metadataProvider.GetMetadataAsync(request.Symbols);
            return Result.Success(metadata);
        }
        catch (Exception ex)
        {
            return Result.Failure<Dictionary<string, MarketMetadata?>>($"Failed to get symbol metadata: {ex.Message}");
        }
    }
}
