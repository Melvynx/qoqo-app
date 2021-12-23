using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace qoqo.Services;

public static class JsonService
{
    public static string Serialize(object obj)
    {
        var contractResolver = new DefaultContractResolver
        {
            NamingStrategy = new CamelCaseNamingStrategy()
        };

        return JsonConvert.SerializeObject(obj,
            new JsonSerializerSettings
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
    }

    public static T? Deserialize<T>(string msg)
    {
        var contractResolver = new DefaultContractResolver
        {
            NamingStrategy = new CamelCaseNamingStrategy()
        };

        return JsonConvert.DeserializeObject<T>(msg,
            new JsonSerializerSettings
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                DateTimeZoneHandling = DateTimeZoneHandling.Local
            });
    }
}