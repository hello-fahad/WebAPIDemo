using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebAPIDemo.Data;
using WebAPIDemo.Models.Repositories;

namespace WebAPIDemo.Filters.ExceptionFilters
{
	public class Shirt_HandleUpdateExceptionsFilterAttribute: ExceptionFilterAttribute
	{
        public ApplicationDbContext Db { get; }
        public Shirt_HandleUpdateExceptionsFilterAttribute(ApplicationDbContext db)
        {
            Db = db;
        }

        public override void OnException(ExceptionContext context)
		{
			base.OnException(context);

			var strShirtId = context.RouteData.Values["id"] as string;

			if (int.TryParse(strShirtId, out int shirtId))
			{
				if (Db.Shirts.FirstOrDefault(x => x.ShirtId == shirtId) == null)
				{
					context.ModelState.AddModelError("ShirtId", "Shirt doesn't exist anymore.");
					var problemDetails = new ValidationProblemDetails(context.ModelState)
					{
						Status = StatusCodes.Status404NotFound
					};
					context.Result = new NotFoundObjectResult(problemDetails);

				}
			}


		}
	}
}
