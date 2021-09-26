using System;
using System.Net;
using System.Security.Authentication;
using System.Threading.Tasks;

using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Middleware;

using RussianQuiz.Connect.Core.Exceptions;
using RussianQuiz.Connect.Functions.Extensions;
using RussianQuiz.Connect.Functions.Models.Responses;


namespace RussianQuiz.Connect.Functions.Middleware
{
    public class ExceptionsMiddleware : IFunctionsWorkerMiddleware
    {
        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            try
            {
                await UnwrapAzureInnerException(context, next);
            }
            catch (AuthenticationException ex)
            {
                SetErrorResult(context, HttpStatusCode.Unauthorized, ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                SetErrorResult(context, HttpStatusCode.Forbidden, ex.Message);
            }
            catch (NotFoundException ex)
            {
                SetErrorResult(context, HttpStatusCode.NotFound, ex.Message);
            }
        }

        // Need to "unwrap" exception thrown by azure functions runtime
        // since it wraps all exceptions thrown by out code into AggregateException exception
        private async Task UnwrapAzureInnerException(FunctionContext context, FunctionExecutionDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (AggregateException e)
            {
                throw e.InnerException ?? e;
            }
        }

        private void SetErrorResult(FunctionContext context, HttpStatusCode statusCode, string message)
        {
            if (context.TryGetHttpRequestData(out var request))
            {
                var response = request.CreateResponse(statusCode, new BaseErrorResponse(message));
                context.SetHttpResponse(response);
            }
        }
    }
}