using System;
using System.Net;

namespace HB.Case.Api.Exceptions
{
    public interface IHbException
    {
        int StatusCode { get; }
    }

    public abstract class HbException : Exception, IHbException
    {
        public abstract int StatusCode { get; }
        public abstract string ResponseMessage { get; }
    }

    public class NotFoundException : HbException
    {
        public override int StatusCode => (int)HttpStatusCode.NotFound;

        public override string ResponseMessage => " not found";
    }

    public class BadRequestException : HbException
    {
        public override int StatusCode => (int)HttpStatusCode.BadRequest;
        public override string ResponseMessage => " bad request";
    }

    public class ConflictException : HbException
    {
        public override int StatusCode => (int)HttpStatusCode.Conflict;
        public override string ResponseMessage => "id already exists";

    }
}
