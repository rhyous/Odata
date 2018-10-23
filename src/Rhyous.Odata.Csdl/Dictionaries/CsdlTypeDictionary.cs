using System;
using System.Collections.Generic;

namespace Rhyous.Odata.Csdl
{
    /// <summary>
    /// Dictionary to map system types to json types for csdl.
    /// </summary>
    /// <remarks>Needs to be rewritten accordin to this spec: http://docs.oasis-open.org/odata/odata-csdl-json/v4.01/odata-csdl-json-v4.01.html 
    /// </remarks>
    public class CsdlTypeDictionary : Dictionary<string, string>
    {
        private static readonly Lazy<CsdlTypeDictionary> Lazy = new Lazy<CsdlTypeDictionary>(() => new CsdlTypeDictionary());

        public static CsdlTypeDictionary Instance => Lazy.Value;

        internal CsdlTypeDictionary()
        {
            Init();
        }

        public void Init()
        {
            Add("System.Binary", "Edm.Binary");
            Add("System.Boolean", "Edm.Boolean");
            Add("System.Byte", "Edm.Byte");
            Add("System.DateTime", "Edm.Date");
            Add("System.DateTimeOffset", "Edm.DateTimeOffset");
            Add("System.Decimal", "Edm.Decimal");
            Add("System.Double", "Edm.Double");
            Add("System.Duration", "Edm.Duration");
            Add("System.Enum", "Edm.Enum");
            Add("System.Guid", "Edm.Guid");
            Add("System.Int16", "Edm.Int16");
            Add("System.Int32", "Edm.Int32");
            Add("System.Int64", "Edm.Int64");
            Add("System.SByte", "Edm.SByte");
            Add("System.Single", "Edm.Single");
            Add("System.Stream", "Edm.Stream");
            Add("System.String", "Edm.String");
            Add("System.TimeOfDay", "Edm.TimeOfDay");
            Add("System.Geography", "Edm.Geography");
            Add("System.GeographyPoint", "Edm.GeographyPoint");
            Add("System.GeographyLineString", "Edm.GeographyLineString");
            Add("System.GeographyPolygon", "Edm.GeographyPolygon");
            Add("System.GeographyMultiPoint", "Edm.GeographyMultiPoint");
            Add("System.GeographyMultiLineString", "Edm.GeographyMultiLineString");
            Add("System.GeographyMultiPolygon", "Edm.GeographyMultiPolygon");
            Add("System.GeographyCollection", "Edm.GeographyCollection");
            Add("System.Geometry", "Edm.Geometry");
            Add("System.GeometryPoint", "Edm.GeometryPoint");
            Add("System.GeometryLineString", "Edm.GeometryLineString");
            Add("System.GeometryPolygon", "Edm.GeometryPolygon");
            Add("System.GeometryMultiPoint", "Edm.GeometryMultiPoint");
            Add("System.GeometryMultiLineString", "Edm.GeometryMultiLineString");
            Add("System.GeometryMultiPolygon", "Edm.GeometryMultiPolygon");
            Add("System.GeometryCollection", "Edm.GeometryCollection");
        }
    }
}