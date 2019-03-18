using System;
using System.Collections.Generic;

namespace Rhyous.Odata.Csdl
{
    public interface ICsdlNavigationPropertyBuilder<TAttribute, TResult>
        where TAttribute : Attribute
    {
        TResult Build(TAttribute attribute, string schemaOrAlias = Constants.DefaultSchemaOrAlias);
    }
}