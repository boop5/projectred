namespace Red.Core.Application.Interfaces
{
    public interface ISlugBuilder
    {
        /// <summary>
        ///     Creates a http conform uri.
        /// </summary>
        /// <param name="input">The string to conform</param>
        string Build(string input);
    }

    public interface IEshopSlugBuilder : ISlugBuilder
    {

    }
}