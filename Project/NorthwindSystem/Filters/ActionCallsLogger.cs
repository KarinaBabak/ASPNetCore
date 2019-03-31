using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Text;

namespace NorthwindSystem.Filters
{
    public class ActionCallsLogger : IActionFilter
    {
        private readonly ILogger _logger;
        private bool _actionLogOn = false;
        private readonly string _actionParamsLogOnConfigSetting = "ActionParamsOn";

        public ActionCallsLogger(ILoggerFactory loggerFactory, IConfiguration config)
        {
            _logger = loggerFactory.CreateLogger<ActionCallsLogger>();
            _actionLogOn = config.GetChildren().Any(item => item.Key == _actionParamsLogOnConfigSetting) && config.GetValue<bool>(_actionParamsLogOnConfigSetting);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            Log("Action is Finished", RouteDataInfo(context.RouteData));
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            string routeInfo = RouteDataInfo(context.RouteData);
            string paramsInfo = string.Empty;
            if (_actionLogOn)
            {
                paramsInfo = LogParams(context);
            }

            Log("Action is Started", $"{routeInfo} {paramsInfo}");
        }

        private void Log(string actionName, string logInfo)
        {
            string resultInfo = $"{actionName} - {logInfo}";
            _logger.LogInformation(logInfo);
        }

        private string RouteDataInfo(RouteData routeData)
        {
            var controllerName = routeData.Values["controller"];
            var actionName = routeData.Values["action"];
            return $"Controller: {controllerName} action: {actionName}";
        }

        private string LogParams(ActionExecutingContext context)
        {
            var actionParams = context.ActionArguments;
            if (actionParams.Any())
            {
                var stringBuilder = new StringBuilder("Action Parameters: ");
                foreach (var parameter in actionParams)
                {
                    stringBuilder.Append($"{parameter.Key} = {parameter.Value}");
                }
                return stringBuilder.ToString();
            }
            return null;
        }
    }
}
