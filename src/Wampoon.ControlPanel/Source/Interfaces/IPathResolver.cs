using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wampoon.ControlPanel.Interfaces
{
    public interface IPathResolver
    {
        string GetPackageDirectory(string packageName);
    }
}