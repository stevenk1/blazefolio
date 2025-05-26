using BlazeFolio.Domain.Providers;
using CSharpFunctionalExtensions;
using MediatR;

namespace BlazeFolio.Application.Wallets.Features.Queries;

public record GetCurrentMarketPrices(IEnumerable<string> Symbols) : IRequest<Result<Dictionary<string, decimal?>>>;

public class GetCurrentMarketPricesHandler : IRequestHandler<GetCurrentMarketPrices, Result<Dictionary<string, decimal?>>>
{
    private readonly IMarketPriceProvider _marketPriceProvider;

    public GetCurrentMarketPricesHandler(IMarketPriceProvider marketPriceProvider)
    {
        _marketPriceProvider = marketPriceProvider;
    }

    public async Task<Result<Dictionary<string, decimal?>>> Handle(GetCurrentMarketPrices request, CancellationToken cancellationToken)
    {
        try
        {
            var currentPrices = await _marketPriceProvider.GetCurrentPricesAsync(request.Symbols);
            return Result.Success(currentPrices);
        }
        catch (Exception ex)
        {
            return Result.Failure<Dictionary<string, decimal?>>($"Failed to get current prices: {ex.Message}");
        }
    }
}
