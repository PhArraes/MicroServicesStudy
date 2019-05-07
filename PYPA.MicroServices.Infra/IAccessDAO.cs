using System.Collections.Generic;
using PYPA.MicroServices.Core;

namespace PYPA.MicroServices.Infra
{
    public interface IAccessDAO
    {
        List<AccessModel> List(int take, int skip);
    }
}