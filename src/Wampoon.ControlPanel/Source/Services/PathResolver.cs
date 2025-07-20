using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wampoon.ControlPanel.Interfaces;
using Wampoon.ControlPanel.Services;

namespace Wampoon.ControlPanel.Services
{
    public class PathResolver : IPathResolver
    {
        private readonly ServerPathResolver _serverPathResolver;

        public PathResolver()
        {
            var fileOps = new FileOperations();
            _serverPathResolver = new ServerPathResolver(fileOps);
        }

        public string GetPackageDirectory(string packageName)
        {
            return _serverPathResolver.GetServerBaseDirectory(packageName);
        }
    }
}