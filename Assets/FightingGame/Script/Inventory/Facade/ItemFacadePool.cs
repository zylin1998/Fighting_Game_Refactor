using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace FightingGame
{
    public class ItemFacadePool
    {
        [Inject]
        DiContainer Container { get; }
        [Inject]
        GlobalDataAccess DataAccess { get; }

        public Dictionary<string, MemoryPool<ItemFacade>> Pools { get; } = new();

        public ItemFacade Spawn(string itemId)
        {
            return GetPool(itemId).Spawn();
        }

        public void Despawn(ItemFacade item)
        {
            GetPool(item.name).Despawn(item);
        }

        private MemoryPool<ItemFacade> GetPool(string itemId)
        {
            if (Pools.TryGetValue(itemId, out var pool))
            {
                return pool;
            }

            var prefab = DataAccess.GetAsset<GameObject>(itemId);

            Container.BindMemoryPool<ItemFacade, Pool>()
                .WithId(itemId)
                .WithFactoryArguments(itemId)
                .FromComponentInNewPrefab(prefab)
                .AsCached();

            pool = Container.ResolveId<Pool>(itemId);

            Pools.Add(itemId, pool);

            return pool;
        }

        protected class Pool : MemoryPool<ItemFacade>
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

            protected override void Reinitialize(ItemFacade item)
            {
                item.gameObject.SetActive(true);

                item.transform.SetParent(null);
            }

            protected override void OnDespawned(ItemFacade item)
            {
                item.Disable();

                item.gameObject.SetActive(false);

                item.transform.SetParent(DespawnRoot);
            }

            protected override void OnCreated(ItemFacade item)
            {
                item.name = Id;
            }
        }
    }
}
