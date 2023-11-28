using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

using Service;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Pal.Web.Extensions
{
    public class CheckUserAttribute : IAsyncActionFilter
    {
        private readonly INotesService _notesService;

        public CheckUserAttribute(INotesService notesService)
        {
            _notesService = notesService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var noteId = Convert.ToInt32(context.ActionArguments["id"]);
            var belongsToUser = await _notesService.IsNoteBelongsToUserAsync(noteId);
            if (!belongsToUser)
                context.Result = new UnauthorizedResult();
            

            await next();
        }
    }
}
