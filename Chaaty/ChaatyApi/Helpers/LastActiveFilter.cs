using ChaatyApi.Repos.interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ChaatyApi.Helpers
{
    public class LastActiveFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var result = await next();
            if (!result.HttpContext.User.Identity.IsAuthenticated)
            { return; }

            var userId =result.HttpContext.User.Identity.GetUserId();
            var UserRepo= result.HttpContext.RequestServices.GetService<IUserRepo>();
            var user= await UserRepo.GetByIdAsyncLight(userId);
            user.LastActive=DateTime.Now;
            await UserRepo.SaveChangesAsync();
            
        }
    }
}
