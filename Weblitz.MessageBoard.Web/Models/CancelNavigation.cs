namespace Weblitz.MessageBoard.Web.Models
{
    public class CancelNavigation
    {
        public CancelNavigation()
        {
        }

        public CancelNavigation(string actionName, string controllerName, object routeValues)
        {
            ActionName = actionName;
            ControllerName = controllerName;
            RouteValues = routeValues;
        }

        public object RouteValues { get; set; }

        public string ActionName { get; set; }

        public string ControllerName { get; set; }
    }
}