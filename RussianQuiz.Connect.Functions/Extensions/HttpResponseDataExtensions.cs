using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.Azure.Functions.Worker.Http;


namespace RussianQuiz.Connect.Functions.Extensions
{
    public static class HttpResponseDataExtensions
    {
        public static HttpResponseData CreateResponse(this HttpRequestData request, HttpStatusCode statusCode, object body)
        {
            return request.CreateResponse(statusCode, JsonSerializer.Serialize(body));
        }

        public static HttpResponseData CreateResponse(this HttpRequestData request, HttpStatusCode statusCode, string body)
        {
            var response = request.CreateResponse(statusCode);
            response.SetBody(body);
            return response;
        }

        public static TBody GetBody<TBody>(this HttpRequestData request, bool caseInsensitive = true)
        {
            using var streamReader = new StreamReader(request.Body);
            string body = streamReader.ReadToEnd();

            return JsonSerializer.Deserialize<TBody>(body, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = caseInsensitive,
            });
        }

        public static void SetBody(this HttpResponseData response, object body)
        {
            response.SetBody(JsonSerializer.Serialize(body));
        }

        public static void SetBody(this HttpResponseData response, string body)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(body);
            response.Body.Write(bytes, 0 , bytes.Length);
        }

        public static async Task SetBodyAsync(this HttpResponseData response, object body)
        {
            var serializedBody = JsonSerializer.Serialize(body);
            await response.SetBodyAsync(serializedBody);
        }

        public static async Task SetBodyAsync(this HttpResponseData response, string body)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(body);
            await response.Body.WriteAsync(bytes, 0 , bytes.Length);
        }

        public static bool TryGetBody<TBody>(this HttpRequestData request, out TBody  body, bool caseInsensitive = true) where TBody : class
        {
            try
            {
                body = request.GetBody<TBody>(caseInsensitive);
                return true;
            }
            catch
            {
                body = null;
                return false;
            }
        }
    }
}