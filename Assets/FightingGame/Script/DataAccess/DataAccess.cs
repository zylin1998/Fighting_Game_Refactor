using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FightingGame
{
    public class DataAccess : IDataAccess
    {
        public DataAccess() 
        {
            Init();
        }

        protected virtual void Init() 
        {
            Assets     = new GameObject("Asset Monitor")     .AddComponent<AssetMonitor>();
            Properties = new GameObject("Properties Monitor").AddComponent<PropertyMonitor>();
            Objects    = new GameObject("Object Monitor")    .AddComponent<ObjectMonitor>();
        }

        public AssetMonitor    Assets     { get; private set; }
        public PropertyMonitor Properties { get; private set; }
        public ObjectMonitor   Objects    { get; private set; }
    }
}
