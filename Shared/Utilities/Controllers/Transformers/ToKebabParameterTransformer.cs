using Microsoft.AspNetCore.Routing;
using System.Text.RegularExpressions;

namespace SharedLibrary.Utilities.Controller.Transformers
{
    /// <summary>
    /// Transforms a controller name to kebab-case for use in route tokens.
    /// For example, converts <c>ProductCreationState</c> to <c>product-creation-state</c>.
    /// 
    /// This is useful for configuring dependency injection with kebab-case route naming conventions.
    /// To apply this transformer, use the following configuration:
    /// <code>
    /// builder.Services.AddControllers(opts =>
    ///     opts.Conventions.Add(new RouteTokenTransformerConvention(new ToKebabParameterTransformer())));
    /// </code>
    /// </summary>
    public partial class ToKebabParameterTransformer : IOutboundParameterTransformer
    {
        public string? TransformOutbound(object? value) => value != null
            ? MyRegex().Replace(value.ToString() ?? string.Empty, "$1-$2").ToLower() // to kebab 
            : null;

        [GeneratedRegex("([a-z])([A-Z])")]
        public static partial Regex MyRegex();
    }
}
