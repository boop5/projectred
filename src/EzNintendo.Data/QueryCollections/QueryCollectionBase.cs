namespace EzNintendo.Data.QueryCollections
{
    public abstract class QueryCollectionBase : IQueryCollection
    {
        protected ApplicationDbContext Ctx { get; private set; }

        public void SetContext(ApplicationDbContext ctx)
        {
            Ctx = ctx;
        }
    }
}