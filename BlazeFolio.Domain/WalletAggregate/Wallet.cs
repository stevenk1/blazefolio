using BlazeFolio.Domain.Common;
using BlazeFolio.Domain.WalletAggregate.ValueObjects;

namespace BlazeFolio.Domain.WalletAggregate;

public class Wallet(WalletId id, string name, Picture picture) : AggregateRoot<WalletId>(id)
{
    public string Name { get; private set; } = name;
    public Picture Picture { get; private set; } = picture;

    public static Wallet CreateNew(string name, byte[] pictureData)
    {
        var wallet= new Wallet(
            WalletId.CreateUnique(),
            name,
            Picture.Create(pictureData));
        return wallet;
        
    }
    
}