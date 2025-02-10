using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace FightingGame
{
    public class EnvironmentPool
    {
        [Inject]
        DiContainer      Container  { get; }
        [Inject]
        GlobalDataAccess DataAccess { get; }
        
        public Dictionary<string, MemoryPool<Environment>> Pools { get; } = new();

        public Environment Spawn(string enviromentId)
        {
            return GetPool(enviromentId).Spawn();
        }

        public void Despawn(Environment character)
        {
            GetPool(character.name).Despawn(character);
        }

        private MemoryPool<Environment> GetPool(string environmentId)
        {
            if (Pools.TryGetValue(environmentId, out var pool))
            {
                return pool;
            }
            
            var prefab = DataAccess.GetAsset<GameObject>(environmentId);
            
            Container.BindMemoryPool<Environment, Pool>()
                .WithId(environmentId)
                .WithFactoryArguments(environmentId)
                .FromComponentInNewPrefab(prefab)
                .AsCached();

            pool = Container.ResolveId<Pool>(environmentId);

            Pools.Add(environmentId, pool);

            return pool;
        }

        protected class Pool : MemoryPool<Environment>
        {
            public Pool(string id)
            {
                DespawnRoot = new GameObject(id + "Root").transform;
            }

            public Transform DespawnRoot { get; }

            [Inject]
            public GlobalDataAccess DataAccess{ get; }

            protected override void Reinitialize(Environment environment)
            {
                environment.gameObject.SetActive(true);

                environment.transform.SetParent(null);
            }

            protected override void OnDespawned(Environment environment)
            {
                environment.gameObject.SetActive(false);

                environment.transform.SetParent(DespawnRoot);
            }
        }
    }
}
