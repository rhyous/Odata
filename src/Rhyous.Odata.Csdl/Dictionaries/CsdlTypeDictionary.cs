using System;
using System.Collections.Concurrent;

namespace Rhyous.Odata.Csdl
{
    /// <summary>
    /// Dictionary to map system types to json types for csdl.
    /// </summary>
    /// <remarks>Needs to be rewritten accordin to this spec: http://docs.oasis-open.org/odata/odata-csdl-json/v4.01/odata-csdl-json-v4.01.html 
    /// </remarks>
    public class CsdlTypeDictionary : ConcurrentDictionary<string, string>
    {
        public CsdlTypeDictionary()
        {
            Init();
        }

        public void Init()
        {
            GetOrAdd("System.Binary", "Edm.Binary");
            GetOrAdd("System.Boolean", "Edm.Boolean");
            GetOrAdd("System.Byte", "Edm.Byte");
            GetOrAdd("System.DateTime", "Edm.Date");
            GetOrAdd("System.DateTimeOffset", "Edm.DateTimeOffset");
            GetOrAdd("System.Decimal", "Edm.Decimal");
            GetOrAdd("System.Double", "Edm.Double");
            GetOrAdd("System.Duration", "Edm.Duration");
            GetOrAdd("System.Enum", "Edm.Enum");
            GetOrAdd("System.Guid", "Edm.Guid");
            GetOrAdd("System.Int16", "Edm.Int16");
            GetOrAdd("System.Int32", "Edm.Int32");
            GetOrAdd("System.Int64", "Edm.Int64");
            GetOrAdd("System.SByte", "Edm.SByte");
            GetOrAdd("System.Single", "Edm.Single");
            GetOrAdd("System.Stream", "Edm.Stream");
            GetOrAdd("System.String", "Edm.String");
            GetOrAdd("System.TimeOfDay", "Edm.TimeOfDay");
            GetOrAdd("System.Geography", "Edm.Geography");
            GetOrAdd("System.GeographyPoint", "Edm.GeographyPoint");
            GetOrAdd("System.GeographyLineString", "Edm.GeographyLineString");
            GetOrAdd("System.GeographyPolygon", "Edm.GeographyPolygon");
            GetOrAdd("System.GeographyMultiPoint", "Edm.GeographyMultiPoint");
            GetOrAdd("System.GeographyMultiLineString", "Edm.GeographyMultiLineString");
            GetOrAdd("System.GeographyMultiPolygon", "Edm.GeographyMultiPolygon");
            GetOrAdd("System.GeographyCollection", "Edm.GeographyCollection");
            GetOrAdd("System.Geometry", "Edm.Geometry");
            GetOrAdd("System.GeometryPoint", "Edm.GeometryPoint");
            GetOrAdd("System.GeometryLineString", "Edm.GeometryLineString");
            GetOrAdd("System.GeometryPolygon", "Edm.GeometryPolygon");
            GetOrAdd("System.GeometryMultiPoint", "Edm.GeometryMultiPoint");
            GetOrAdd("System.GeometryMultiLineString", "Edm.GeometryMultiLineString");
            GetOrAdd("System.GeometryMultiPolygon", "Edm.GeometryMultiPolygon");
            GetOrAdd("System.GeometryCollection", "Edm.GeometryCollection");
        }
    }
}