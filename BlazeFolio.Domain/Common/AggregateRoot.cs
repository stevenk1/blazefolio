using Books.Domain.Common;

namespace BlazeFolio.Domain.Common
{
    public abstract class AggregateRoot<TId> : Entity<TId> where TId : notnull
    {
        protected AggregateRoot(TId id) : base(id) { }
        protected AggregateRoot() { }
    }
}
