namespace BlogPost.Api.src.Core.Exceptions;

public class BlogPostServiceException : Exception
{
    public BlogPostServiceException() : base() { }

    public BlogPostServiceException(string message) : base(message) { }

    public BlogPostServiceException(string message, Exception innerException) : base(message, innerException) { }
}