using System;
using System.Collections.Generic;
using System.Reflection;

namespace Rhyous.Odata.Csdl
{
    public interface ICsdlBuilder<TInput,TResult>
        where TInput : MemberInfo
    {
        TResult Build(TInput tInput);
    }
}