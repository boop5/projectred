namespace Red.Core.Application.Interfaces
{
    public interface ISlugBuilder
    {
        /// <summary>
        ///     Creates a http conform uri.
        /// </summary>
        /// <param name="input">The string to convert. Can be null.</param>
        string? Build(string? input);
    }
}