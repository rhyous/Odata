using System.Collections.Concurrent;

namespace Rhyous.Odata.Csdl
{
    /// <summary>
    /// Dictionary to map system types to json types for csdl.
    /// </summary>
    /// <remarks>Needs to be rewritten accordin to this spec: http://docs.oasis-open.org/odata/odata-csdl-json/v4.01/odata-csdl-json-v4.01.html 
    /// </remarks>
    public class CsdlTypeDictionary : ConcurrentDictionary<string, string>, ICsdlTypeDictionary
    {
        public CsdlTypeDictionary()
        {
            Init();
        }

        public void Init()
        {
            AddBidirectionally("System.Binary", "Edm.Binary");
            AddBidirectionally("System.Boolean", "Edm.Boolean");
            AddBidirectionally("System.Byte", "Edm.Byte");
            AddBidirectionally("System.DateTime", "Edm.Date");
            AddBidirectionally("System.DateTimeOffset", "Edm.DateTimeOffset");
            AddBidirectionally("System.Decimal", "Edm.Decimal");
            AddBidirectionally("System.Double", "Edm.Double");
            AddBidirectionally("System.Duration", "Edm.Duration");
            AddBidirectionally("System.Enum", "Edm.Enum");
            AddBidirectionally("System.Guid", "Edm.Guid");
            AddBidirectionally("System.Int16", "Edm.Int16");
            AddBidirectionally("System.Int32", "Edm.Int32");
            AddBidirectionally("System.Int64", "Edm.Int64");
            AddBidirectionally("System.SByte", "Edm.SByte");
            AddBidirectionally("System.Single", "Edm.Single");
            AddBidirectionally("System.Stream", "Edm.Stream");
            AddBidirectionally("System.String", "Edm.String");
            AddBidirectionally("System.TimeOfDay", "Edm.TimeOfDay");
            AddBidirectionally("System.Geography", "Edm.Geography");
            AddBidirectionally("System.GeographyPoint", "Edm.GeographyPoint");
            AddBidirectionally("System.GeographyLineString", "Edm.GeographyLineString");
            AddBidirectionally("System.GeographyPolygon", "Edm.GeographyPolygon");
            AddBidirectionally("System.GeographyMultiPoint", "Edm.GeographyMultiPoint");
            AddBidirectionally("System.GeographyMultiLineString", "Edm.GeographyMultiLineString");
            AddBidirectionally("System.GeographyMultiPolygon", "Edm.GeographyMultiPolygon");
            AddBidirectionally("System.GeographyCollection", "Edm.GeographyCollection");
            AddBidirectionally("System.Geometry", "Edm.Geometry");
            AddBidirectionally("System.GeometryPoint", "Edm.GeometryPoint");
            AddBidirectionally("System.GeometryLineString", "Edm.GeometryLineString");
            AddBidirectionally("System.GeometryPolygon", "Edm.GeometryPolygon");
            AddBidirectionally("System.GeometryMultiPoint", "Edm.GeometryMultiPoint");
            AddBidirectionally("System.GeometryMultiLineString", "Edm.GeometryMultiLineString");
            AddBidirectionally("System.GeometryMultiPolygon", "Edm.GeometryMultiPolygon");
            AddBidirectionally("System.GeometryCollection", "Edm.GeometryCollection");
        }

        /// <summary>
        /// This method make the dictionary bidirectional. 
        /// 1. The string a is a Key with a value of string b.
        /// 1. The string b is a Key with a value of string a.
        /// </summary>
        /// <param name="a">The first string.</param>
        /// <param name="b">The second string.</param>
        public void AddBidirectionally(string a, string b)
        {
            GetOrAdd(a, b);
            GetOrAdd(b, a);
        }
    }
}