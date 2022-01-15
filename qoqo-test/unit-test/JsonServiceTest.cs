using System;
using System.Text.RegularExpressions;
using qoqo.Services;
using Xunit;

namespace qoqo_test.unit_test;

public class JsonServiceTest
{
    [Fact]
    public void SerializeTest()
    {
        var json = JsonService.Serialize(new {
            a = 4, 
            b = "test", 
            c = DateTime.MinValue,
            d = new { e = "test" },
            f = new[] { 1, 2, 3 }
        });
        
        const string expected = "{  \"a\": 4,  \"b\": \"test\",  \"c\": \"0001-01-01T00:00:00\",  \"d\": {    \"e\": \"test\"  },  \"f\": [    1,    2,    3  ]}";
        Assert.Equal(expected, json.Replace("\n", ""));
    }

    [Fact]
    public void DeserializeTest()
    {
        var obj = JsonService.Deserialize<Test>("{ \"a\": 4, \"b\": \"test\" }");
        Assert.Equal(4, obj.A);
        Assert.Equal("test", obj.B);
    }

    private class Test
    {
        public int A { get; set; }
        public string B { get; set; }

        public bool CheckPassword(string psw)
        {
            // use a regex to check password format: 1-9, a-z, A-Z, can include @#$%^&* or not and minimum 8 characters
            return new Regex("^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[@#$%^&*])(?=.{8,})").IsMatch(psw);
        }
    }
}