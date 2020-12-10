using System;
namespace KidsList.Api.Exceptions
{
    public class UserMissingException : Exception
    {
        public UserMissingException(string message) : base(message)
        {
        }
    }
}
