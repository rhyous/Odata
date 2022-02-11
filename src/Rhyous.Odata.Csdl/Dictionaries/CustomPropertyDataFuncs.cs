namespace Rhyous.Odata.Csdl
{
    /// <summary>
    /// A class to store funcs that create additional metadata, such as 
    /// annotations, inside a property.
    /// </summary>
    public class CustomPropertyDataFuncs : FuncList<string, string>, ICustomPropertyDataFuncs
    {
    }
}