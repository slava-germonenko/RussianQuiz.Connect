using Microsoft.AspNetCore.Mvc;


namespace RussianQuiz.Connect.Functions.Triggers
{
    public class BaseTrigger
    {
        protected OkObjectResult Ok(object value) => new OkObjectResult(value);

        protected BadRequestObjectResult BadRequest(object value) => new BadRequestObjectResult(value);

        protected NotFoundObjectResult NotFound(object value) => new NotFoundObjectResult(value);

        protected NoContentResult NoContent() => new NoContentResult();
    }
}