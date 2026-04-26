using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using MyAssistant.Apis.Expenses.Api.FeatureFlags;
using Xunit;

namespace Investments.Tests;

public class FeatureFlagFilterTests
{
    private static ActionExecutingContext BuildContext()
    {
        var actionContext = new ActionContext(
            new DefaultHttpContext(),
            new RouteData(),
            new ActionDescriptor());

        return new ActionExecutingContext(
            actionContext,
            new List<IFilterMetadata>(),
            new Dictionary<string, object?>(),
            controller: new object());
    }

    private static FeatureFlagFilter BuildFilter(bool enablePortfolioCreation, string featureName)
    {
        var settings = new FeatureFlagSettings { EnablePortfolioCreation = enablePortfolioCreation };
        var options = Options.Create(settings);
        return new FeatureFlagFilter(options, featureName);
    }

    [Fact]
    public void OnActionExecuting_WhenFlagEnabled_DoesNotSetResult()
    {
        var filter = BuildFilter(enablePortfolioCreation: true, nameof(FeatureFlagSettings.EnablePortfolioCreation));
        var context = BuildContext();

        filter.OnActionExecuting(context);

        Assert.Null(context.Result);
    }

    [Fact]
    public void OnActionExecuting_WhenFlagDisabled_SetsNotFoundResult()
    {
        var filter = BuildFilter(enablePortfolioCreation: false, nameof(FeatureFlagSettings.EnablePortfolioCreation));
        var context = BuildContext();

        filter.OnActionExecuting(context);

        Assert.IsType<NotFoundResult>(context.Result);
    }

    [Fact]
    public void OnActionExecuting_UnknownFeatureName_DoesNotSetResult()
    {
        var filter = BuildFilter(enablePortfolioCreation: false, featureName: "NonExistentFeature");
        var context = BuildContext();

        filter.OnActionExecuting(context);

        Assert.Null(context.Result);
    }

    [Fact]
    public void OnActionExecuted_DoesNothing()
    {
        var filter = BuildFilter(enablePortfolioCreation: false, nameof(FeatureFlagSettings.EnablePortfolioCreation));
        var actionContext = new ActionContext(new DefaultHttpContext(), new RouteData(), new ActionDescriptor());
        var context = new ActionExecutedContext(actionContext, new List<IFilterMetadata>(), controller: new object());

        var ex = Record.Exception(() => filter.OnActionExecuted(context));

        Assert.Null(ex);
    }
}
