using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Host;


namespace RussianQuiz.Connect.Functions.Attributes
{
    /// <summary>
    /// Placeholder attribute which is used to marks HTTP endpoints
    /// that require authorization token validation
    /// </summary>
    public class RequireTokenAuthorizationAttribute : FunctionInvocationFilterAttribute
    {
        public override Task OnExecutingAsync(FunctionExecutingContext executingContext, CancellationToken cancellationToken)
        {
            if (!executingContext.Arguments.Any(arg => arg is HttpRequestData))
            {
                return Task.CompletedTask;
            }

            var request = executingContext.Arguments
                .FirstOrDefault(prop => prop.Value is HttpRequestData).Value
                as HttpRequestData;

            return Task.CompletedTask;
        }
    }
}