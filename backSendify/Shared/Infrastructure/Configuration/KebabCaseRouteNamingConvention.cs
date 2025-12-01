using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace backSendify.Shared.Infrastructure.Configuration;

public class KebabCaseRouteNamingConvention : IControllerModelConvention
{
    private static readonly Regex KebabCaseRegex = new("(?<!^)([A-Z])", RegexOptions.Compiled);

    public void Apply(ControllerModel controller)
    {
        foreach (var selector in controller.Selectors)
        {
            if (selector.AttributeRouteModel is { Template: { } template })
            {
                selector.AttributeRouteModel.Template = ToKebabCase(template);
            }
        }

        foreach (var action in controller.Actions)
        {
            foreach (var selector in action.Selectors)
            {
                if (selector.AttributeRouteModel is { Template: { } template })
                {
                    selector.AttributeRouteModel.Template = ToKebabCase(template);
                }
            }
        }
    }

    private static string ToKebabCase(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return value;
        }

        var transformed = KebabCaseRegex.Replace(value, "-$1");
        transformed = transformed.Replace("--", "-");
        return transformed.ToLowerInvariant();
    }
}
