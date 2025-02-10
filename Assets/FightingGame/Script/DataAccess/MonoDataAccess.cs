using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FightingGame
{
    public class MonoDataAccess : MonoBehaviour, IDataAccess
    {
        public void Awake()
        {
            Assets     = new GameObject("Asset Monitor")     .AddComponent<AssetMonitor>();
            Properties = new GameObject("Properties Monitor").AddComponent<PropertyMonitor>();
            Objects    = new GameObject("Object Monitor")    .AddComponent<ObjectMonitor>();

            Assets    .transform.SetParent(transform);
            Properties.transform.SetParent(transform);
            Objects   .transform.SetParent(transform);
        }

        public AssetMonitor    Assets     { get; private set; }
        public PropertyMonitor Properties { get; private set; }
        public ObjectMonitor   Objects    { get; private set; }
    }
}
