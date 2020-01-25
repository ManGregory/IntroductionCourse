﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using TestRunner.CommonTypes.Implementations;
using TestRunner.CommonTypes.Interfaces;
using WebLMS.Models;

namespace WebLMS.Services.TestManager
{
    public static class JsonTypesConverter
    {
        class MethodTestJsonDTO
        {
            public string[] Types { get; set; }
            public string[] Values { get; set; }
        }

        class ConsoleTestJsonDTO
        {
            public string[] Types { get; set; }
            public string[] Values { get; set; }
            public IEnumerable<ConsoleStep> Steps { get; set; }
        }        

        public static ITestInfo ConvertToMethodTest(CodingTest codingTest)
        {
            var inputParams = JsonConvert.DeserializeObject<MethodTestJsonDTO>(codingTest.InputParameters);
            var expectedResult = JsonConvert.DeserializeObject<MethodTestJsonDTO>(codingTest.ExpectedResult);
            return new MethodTestInfo()
            {
                Id = codingTest.Id,
                Name = codingTest.Name,
                InputParameters = ConvertTypeValueArray(inputParams),
                ExpectedResult = ConvertTypeValueArray(expectedResult)[0]
            };
        }

        public static ITestInfo ConvertToConsoleTest(CodingTest codingTest)
        {
            var consoleTest = JsonConvert.DeserializeObject<ConsoleTestJsonDTO>(codingTest.InputParameters);
            return new ConsoleTestInfo()
            {
                Id = codingTest.Id,
                Name = codingTest.Name,
                ConsoleTest = ConvertToConsoleTest(consoleTest)
            };
        }

        private static ConsoleTest ConvertToConsoleTest(ConsoleTestJsonDTO consoleTestJsonDTO)
        {
            return new ConsoleTest()
            {
                MethodInputParameteres = ConvertTypeValueArray(new MethodTestJsonDTO() 
                { 
                    Types = consoleTestJsonDTO.Types,
                    Values = consoleTestJsonDTO.Values
                }),
                ConsoleSteps = consoleTestJsonDTO.Steps
            };
        }

        private static object[] ConvertTypeValueArray(MethodTestJsonDTO method)
        {
            var result = new List<object>();
            for (int i = 0; i < method.Types.Length; i++)
            {
                string type = method.Types[i];
                if (type == "int")
                {
                    result.Add(Convert.ToInt32(method.Values[i]));
                }
                else if (type == "bool")
                {
                    result.Add(Convert.ToBoolean(method.Values[i]));
                }
                else if (type == "decimal")
                {
                    result.Add(Convert.ToDecimal(NormalizeNumberValue(method.Values[i])));
                }
                else if (type == "double")
                {
                    result.Add(Convert.ToDouble(NormalizeNumberValue(method.Values[i])));
                }
                else if (type == "string")
                {
                    result.Add(method.Values[i]);
                }
            }
            return result.ToArray();
        }

        private static string NormalizeNumberValue(string number)
        {
            string separator = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            return number.Replace(".", separator).Replace(",", separator);
        }
    }
}