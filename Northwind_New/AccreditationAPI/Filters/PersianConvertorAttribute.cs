using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace KnowledgeManagementAPI.Filters
{
    public class PersianConvertorAttribute : Attribute, IActionFilter
    {
        readonly string ParameterName;
        public PersianConvertorAttribute(string parameterName)
        => ParameterName = parameterName;

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var DtoProperties = context.ActionArguments[ParameterName].GetType().GetProperties();
            if (DtoProperties != null)
                foreach (var item in DtoProperties)
                    if (item.PropertyType == typeof(System.String))
                    {
                        object paramValue = item.GetValue(context.ActionArguments[ParameterName]);
                        if (paramValue != null)
                            item.SetValue(context.ActionArguments[ParameterName], paramValue.ToString().ToPersianString());
                    }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        { }
    }
}
