using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace MyAssistant.Apis.Expenses.Api.FeatureFlags
{
    public class FeatureFlagFilter : IActionFilter
    {
        private readonly FeatureFlagSettings _featureFlags;
        private readonly string _featureName;

        public FeatureFlagFilter(IOptions<FeatureFlagSettings> featureFlags, string featureName)
        {
            _featureFlags = featureFlags.Value;
            _featureName = featureName;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var enabled = _featureName switch
            {
                nameof(FeatureFlagSettings.EnablePortfolioCreation) => _featureFlags.EnablePortfolioCreation,
                _ => true
            };

            if (!enabled)
            {
                context.Result = new NotFoundResult();
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }

    public class RequireFeatureAttribute : TypeFilterAttribute
    {
        public RequireFeatureAttribute(string featureName) : base(typeof(FeatureFlagFilter))
        {
            Arguments = new object[] { featureName };
        }
    }
}
