namespace System.IO
{
    public class PathNotFoundException : FileNotFoundException
    {
        public PathNotFoundException()
        {
        }

        public PathNotFoundException(string message)
            : base(message)
        {
        }
    }
}