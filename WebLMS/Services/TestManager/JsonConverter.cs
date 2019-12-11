using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestRunner.CommonTypes.Implementations;
using TestRunner.CommonTypes.Interfaces;
using WebLMS.Models;

namespace WebLMS.Services.TestManager
{
    public static class JsonConverter
    {
        public static ITestInfo ConvertToCommonTest(CodingTest codingTest)
        {
            return new MethodTestInfo()
            {
                Id = codingTest.Id,
                InputParameters = ConvertJson(codingTest.InputParameters),
                ExpectedResult = ConvertJson(codingTest.ExpectedResult)[0]
            };
        }

        private static object[] ConvertJson(string param)
        {
            var json = JObject.Parse(param);
            var result = new List<object>();
            var types = json["types"].Values().ToArray();
            var values = json["values"].ToArray();
            for (int i = 0; i < types.Length; i++)
            {
                string type = types[i].Value<string>();
                if (type == "int")
                {
                    result.Add(Convert.ToInt32(values[i].Value<string>()));
                }
                else if (type == "bool")
                {
                    result.Add(Convert.ToBoolean(values[i].Value<string>()));
                }
            }
            return result.ToArray();
        }
    }
}
