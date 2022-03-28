using Rhyous.Odata.Csdl;

namespace Rhyous.Odata.Tests
{
    public class EntityWithStringType
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [CsdlStringProperty(StringType = CsdlStringTypes.TextArea)]
        public string Desciption { get; set; }

        [CsdlStringProperty(StringType = CsdlStringTypes.Href)]
        public string Link { get; set; }
    }

    public interface IEntityWithStringTypeInInterface
    {
        int Id { get; set; }
        string Name { get; set; }
        [CsdlStringProperty(StringType = CsdlStringTypes.Href)]
        string Link { get; set; }
    }

    public class EntityWithStringTypeInInterface : IEntityWithStringTypeInInterface, IDescription
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Desciption { get; set; }

        public string Link { get; set; }
    }

    public interface IDescription
    {
        [CsdlStringProperty(StringType = CsdlStringTypes.TextArea)]
        string Desciption { get; set; }
    }

    public interface IEntityWithStringTypeInSubInterface : IDescription
    {
        int Id { get; set; }
        string Name { get; set; }
        [CsdlStringProperty(StringType = CsdlStringTypes.Href)]
        string Link { get; set; }
    }

    public class EntityWithStringTypeInSubInterface : IEntityWithStringTypeInSubInterface
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Desciption { get; set; }
        [CsdlStringProperty(StringType = CsdlStringTypes.Href)]
        public string Link { get; set; }
    }
}
