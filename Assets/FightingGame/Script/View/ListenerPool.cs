using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace FightingGame
{
    public class ListenerPool
    {
        [Inject]
        DiContainer      Container { get; }
        [Inject]
        GlobalDataAccess DataAccess { get; }

        public Dictionary<string, MemoryPool<DynamicPoster>> Pools { get; } = new();

        public DynamicPoster Spawn(string listenerId)
        {
            return GetPool(listenerId).Spawn();
        }

        public void Despawn(DynamicPoster listener)
        {
            GetPool(listener.name).Despawn(listener);
        }

        private MemoryPool<DynamicPoster> GetPool(string listenerId)
        {
            if (Pools.TryGetValue(listenerId, out var pool))
            {
                return pool;
            }

            var prefab = DataAccess.GetAsset<GameObject>(listenerId);
            
            Container.BindMemoryPool<DynamicPoster, Pool>()
                .WithId(listenerId)
                .WithFactoryArguments(listenerId)
                .FromComponentInNewPrefab(prefab)
                .AsCached();
            
            pool = Container.ResolveId<Pool>(listenerId);

            Pools.Add(listenerId, pool);

            return pool;
        }

        protected class Pool : MemoryPool<DynamicPoster>
        {
            public Pool(string id)
            {
                Id = id;

                DespawnRoot = new GameObject(id + "Root").transform;
            }

            public string Id { get; }

            public Transform DespawnRoot { get; }

            [Inject]
            public GlobalDataAccess DataAccess { get; }

            protected override void Reinitialize(DynamicPoster listener)
            {
                listener.gameObject.SetActive(true);

                listener.transform.SetParent(null);
            }

            protected override void OnDespawned(DynamicPoster listener)
            {
                listener.gameObject.SetActive(false);

                listener.transform.SetParent(DespawnRoot);
            }

            protected override void OnCreated(DynamicPoster listener)
            {
                listener.name = Id;
            }
        }
    }
}
