using System;
using qoqo.Services;
using Xunit;

namespace qoqo_test.unit_test;

public class JsonServiceTest
{
    [Fact]
    public void SerializeTest()
    {
        var json = JsonService.Serialize(new
        {
            A = 4,
            B = "test",
            C = DateTime.MinValue,
            D = new {E = "test"},
            F = new[] {1, 2, 3}
        });

        const string expected =
            "{  \"a\": 4,  \"b\": \"test\",  \"c\": \"0001-01-01T00:00:00\",  \"d\": {    \"e\": \"test\"  },  \"f\": [    1,    2,    3  ]}";
        Assert.Equal(expected, json.Replace("\n", ""));
    }

    [Fact]
    public void DeserializeTest()
    {
        var obj = JsonService.Deserialize<DeserializeObjectTest>("{ \"a\": 4, \"b\": \"test\" }");
        Assert.Equal(4, obj?.A);
        Assert.Equal("test", obj?.B);
    }

    private class DeserializeObjectTest
    {
        public int A { get; set; }
        public string B { get; set; }
    }
}