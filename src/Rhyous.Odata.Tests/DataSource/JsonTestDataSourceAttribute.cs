using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Rhyous.UnitTesting
{
    public interface IName
    {
        string Name { get; set; }
    }

    public class Row<T> : IName
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public T Value { get; set; }
        public T Expected { get; set; }
        public string Message { get; set; }
    }

    public class JsonTestDataSourceAttribute : Attribute, ITestDataSource
    {
        private readonly Type _Type;
        private readonly string _File;

        public JsonTestDataSourceAttribute(Type type, string file)
        {
            _Type = type;
            _File = file;
        }

        public IEnumerable<object[]> GetData(MethodInfo methodInfo)
        {
            var json = File.ReadAllText(_File);
            var rows = JsonConvert.DeserializeObject(json, _Type) as List<object>;
            return rows.Select(r => new object[] { r });
        }

        public string GetDisplayName(MethodInfo methodInfo, object[] data)
        {
            return (data[0] as IName).Name;
        }
    }
}
