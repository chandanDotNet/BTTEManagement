using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace POS.API.Helpers
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'UnprocessableEntityObjectResult'
    public class UnprocessableEntityObjectResult : ObjectResult
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'UnprocessableEntityObjectResult'
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'UnprocessableEntityObjectResult.UnprocessableEntityObjectResult(ModelStateDictionary)'
        public UnprocessableEntityObjectResult(ModelStateDictionary modelState)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'UnprocessableEntityObjectResult.UnprocessableEntityObjectResult(ModelStateDictionary)'
            : base(new SerializableError(modelState))
        {
            if (modelState == null)
            {
                throw new ArgumentNullException(nameof(modelState));
            }
            StatusCode = 422;
        }
    }
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'UnAuthorizedEntityObjectResult'
    public class UnAuthorizedEntityObjectResult : ObjectResult
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'UnAuthorizedEntityObjectResult'
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'UnAuthorizedEntityObjectResult.UnAuthorizedEntityObjectResult(ModelStateDictionary)'
        public UnAuthorizedEntityObjectResult(ModelStateDictionary modelState)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'UnAuthorizedEntityObjectResult.UnAuthorizedEntityObjectResult(ModelStateDictionary)'
            : base(new SerializableError(modelState))
        {
            if (modelState == null)
            {
                throw new ArgumentNullException(nameof(modelState));
            }
            StatusCode = 403;
        }
    }
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'UnAuthenticationEntityObjectResult'
    public class UnAuthenticationEntityObjectResult : ObjectResult
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'UnAuthenticationEntityObjectResult'
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'UnAuthenticationEntityObjectResult.UnAuthenticationEntityObjectResult(ModelStateDictionary)'
        public UnAuthenticationEntityObjectResult(ModelStateDictionary modelState)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'UnAuthenticationEntityObjectResult.UnAuthenticationEntityObjectResult(ModelStateDictionary)'
            : base(new SerializableError(modelState))
        {
            if (modelState == null)
            {
                throw new ArgumentNullException(nameof(modelState));
            }
            StatusCode = 401;
        }
    }
    

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'BadRequestEntityObjectResult'
    public class BadRequestEntityObjectResult: ObjectResult
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'BadRequestEntityObjectResult'
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'BadRequestEntityObjectResult.BadRequestEntityObjectResult(ModelStateDictionary)'
        public BadRequestEntityObjectResult(ModelStateDictionary modelState)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'BadRequestEntityObjectResult.BadRequestEntityObjectResult(ModelStateDictionary)'
        : base(new SerializableError(modelState))
        {
            if (modelState == null)
            {
                throw new ArgumentNullException(nameof(modelState));
            }
            StatusCode = 500;
        }
    }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'AlreadyExistEntityObjectResult'
    public class AlreadyExistEntityObjectResult : ObjectResult
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'AlreadyExistEntityObjectResult'
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'AlreadyExistEntityObjectResult.AlreadyExistEntityObjectResult(string)'
        public AlreadyExistEntityObjectResult(String message)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'AlreadyExistEntityObjectResult.AlreadyExistEntityObjectResult(string)'
            : base(message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }
            StatusCode = 409;
        }
    }

   
}
