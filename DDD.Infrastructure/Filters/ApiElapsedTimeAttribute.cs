using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;
using XUCore.NetCore;

namespace DDD.Infrastructure.Filters
{

    /// <summary>
    /// API查询时间
    /// </summary>
    public class ApiElapsedTimeAttribute : ActionFilterAttribute
    {
        private Stopwatch stopwatch = new Stopwatch();

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (stopwatch == null)
                stopwatch = new Stopwatch();
            stopwatch.Reset();
            stopwatch.Restart();
            base.OnActionExecuting(context);
        }

        public override void OnActionExecuted(ActionExecutedContext actionExecutedContext)
        {
            if (stopwatch == null)
                stopwatch = new Stopwatch();
            base.OnActionExecuted(actionExecutedContext);
            stopwatch.Stop();

            var reType = actionExecutedContext.Result?.GetType();

            if (reType == typeof(Result))
            {
                var res = (Result)actionExecutedContext.Result;
                if (res != null)
                {
                    res.elapsedTime = stopwatch.ElapsedMilliseconds;
                    //res.Value?.GetType().GetProperty("elapsedTime").SetValue(res.Value, stopwatch.ElapsedMilliseconds);

                    actionExecutedContext.Result = res;
                }
            }
            else if (reType == typeof(ObjectResult))
            {
                var res = (ObjectResult)actionExecutedContext.Result;
                if (res != null)
                {
                    res.Value?.GetType().GetProperty("elapsedTime").SetValue(res.Value, stopwatch.ElapsedMilliseconds);

                    actionExecutedContext.Result = res;
                }
            }
        }
    }
}
