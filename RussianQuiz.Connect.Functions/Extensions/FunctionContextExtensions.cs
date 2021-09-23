using System.Collections.Generic;
using System.Linq;

using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;


namespace RussianQuiz.Connect.Functions.Extensions
{
    public static class FunctionContextExtensions
    {
        public static bool TryGetHttpRequestData(this FunctionContext context, out HttpRequestData httpContext)
        {
            try
            {
                httpContext = context.GetHttpRequestData();
                return httpContext is not null;
            }
            catch
            {
                httpContext = null;
                return false;
            }
        }

        public static HttpRequestData GetHttpRequestData(this FunctionContext context)
        {
            var functionBindingsFeature = context.GetBindingFeature();

            var functionInputData = functionBindingsFeature.GetType()
                .GetProperties()
                .Single(p => p.Name == "InputData")
                .GetValue(functionBindingsFeature)
                as IReadOnlyDictionary<string, object>;

            return functionInputData?.Values.SingleOrDefault(o => o is HttpRequestData) as HttpRequestData;
        }

        public static bool TrySetHttpResponse(this FunctionContext context, HttpResponseData response)
        {
            try
            {
                context.SetHttpResponse(response);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void SetHttpResponse(this FunctionContext context, HttpResponseData response)
        {
            var functionBindingsFeature = context.GetBindingFeature();
            var invocationResultProperty = functionBindingsFeature
                .GetType()
                .GetProperties()
                .Single(p => p.Name == "InvocationResult");

            invocationResultProperty.SetValue(functionBindingsFeature, response);
        }

        private static object GetBindingFeature(this FunctionContext context)
        {
            return context.Features
                .SingleOrDefault(f => f.Key.Name == "IFunctionBindingsFeature")
                .Value;
        }
    }
}