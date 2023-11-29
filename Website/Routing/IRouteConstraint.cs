using Service;

namespace Website.Routing
{
    public class NoteShareRouteConstraint : IRouteConstraint
    {
        
        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            var shareLink = values[routeKey]?.ToString();
            if (string.IsNullOrEmpty(shareLink))
                return false;


            var notesService = httpContext.RequestServices.GetService(typeof(INotesService)) as INotesService;
            var isShared = notesService.IsNoteSharedAsync(shareLink).Result;
            return isShared;
            
        }
    }
}
