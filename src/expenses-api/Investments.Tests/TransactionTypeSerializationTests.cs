using System.Text.Json;
using System.Text.Json.Serialization;
using MyAssistant.Apis.Expenses.Api.Resources.Investments;
using Xunit;

namespace Investments.Tests;

public class TransactionTypeSerializationTests
{
    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() }
    };

    [Theory]
    [InlineData("\"Buy\"", TransactionType.Buy)]
    [InlineData("\"Sell\"", TransactionType.Sell)]
    public void Deserialize_StringValue_ReturnsCorrectEnum(string json, TransactionType expected)
    {
        var result = JsonSerializer.Deserialize<TransactionType>(json, Options);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(TransactionType.Buy, "\"Buy\"")]
    [InlineData(TransactionType.Sell, "\"Sell\"")]
    public void Serialize_EnumValue_ReturnsString(TransactionType value, string expected)
    {
        var result = JsonSerializer.Serialize(value, Options);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("0", TransactionType.Buy)]
    [InlineData("1", TransactionType.Sell)]
    public void Deserialize_IntegerValue_ReturnsCorrectEnum(string json, TransactionType expected)
    {
        // JsonStringEnumConverter accepts integers as a fallback
        var result = JsonSerializer.Deserialize<TransactionType>(json, Options);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void Deserialize_TransactionRequest_WithStringType_Succeeds()
    {
        var json = """{"portfolioId":"p1","symbol":"BTC","assetType":"Cryptocurrency","type":"Buy","quantity":1.5,"price":50000,"date":"2026-01-01T00:00:00"}""";

        var result = JsonSerializer.Deserialize<TransactionRequest>(json, Options);

        Assert.NotNull(result);
        Assert.Equal(TransactionType.Buy, result.Type);
    }

    [Fact]
    public void Deserialize_TransactionRequest_WithSellType_Succeeds()
    {
        var json = """{"portfolioId":"p1","symbol":"ETH","assetType":"Cryptocurrency","type":"Sell","quantity":2.0,"price":3000,"date":"2026-01-01T00:00:00"}""";

        var result = JsonSerializer.Deserialize<TransactionRequest>(json, Options);

        Assert.NotNull(result);
        Assert.Equal(TransactionType.Sell, result.Type);
    }
}
