using System;
using System.Net;
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
            catch (NotFoundException ex)
            {
                if (context.TryGetHttpRequestData(out var request))
                {
                    var response = request.CreateResponse(HttpStatusCode.NotFound, new BaseErrorResponse(ex.Message));
                    context.SetHttpResponse(response);
                }
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
    }
}