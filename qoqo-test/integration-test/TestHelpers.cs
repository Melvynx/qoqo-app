using System.Net.Http;
using qoqo.Services;

namespace qoqo_test.integration_test;

public static class TestHelpers
{
    public static T? GetBody<T>(HttpResponseMessage response)
    {
        var content = response.Content.ReadAsStringAsync().Result;
        return JsonService.Deserialize<T>(content);
    }
}