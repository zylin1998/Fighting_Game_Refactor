using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
using Zenject;

namespace FightingGame
{
    public class GlobalDataAccess : DataAccess
    {
        public Transform   Root      { get; private set; }
        public DiContainer Container { get; }

        protected override void Init()
        {
            base.Init();

            Root = new GameObject("Global DataAccess").transform;

            Assets    .transform.SetParent(Root);
            Properties.transform.SetParent(Root);
            Objects   .transform.SetParent(Root);
        }

        public AsyncOperationHandle<IList<UnityEngine.Object>> LoadObjects(IEnumerable keys) 
        {
            return Addressables.LoadAssetsAsync<UnityEngine.Object>(keys, (r) => Assets.Add(r), Addressables.MergeMode.Union);
        }
    }
}
