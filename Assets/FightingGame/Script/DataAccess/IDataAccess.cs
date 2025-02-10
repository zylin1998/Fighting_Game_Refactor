using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FightingGame
{
    public interface IDataAccess
    {
        public AssetMonitor    Assets     { get; }
        public PropertyMonitor Properties { get; }
        public ObjectMonitor   Objects    { get; }
    }
}
