using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Zenject;
using UnityEngine;

namespace FightingGame
{
    public class CharacterPool
    {
        public CharacterPool(CharacterConstructor constructor) 
        {
            Constructor = constructor;
        }

        [Inject]
        DiContainer           Container   { get; }
        [Inject]
        GlobalDataAccess      DataAccess  { get; }
        [Inject]
        CharacterRequireAsset Requires    { get; }
        [Inject]
        CharacterCollection   Collection  { get; }

        public virtual object Group { get; } = "Character";
        
        public CharacterConstructor Constructor { get; set; }

        public Dictionary<string, MemoryPool<Character>> Pools { get; } = new();

        public Character Spawn(string characterId, int guid) 
        {
            var character = GetPool(characterId).Spawn();

            character.GUID = guid;

            Collection.Add(Group, character);

            return character;
        }

        public void Despawn(Character character) 
        {
            Collection.Remove(Group, character);

            GetPool(character.Id).Despawn(character);
        }

        private MemoryPool<Character> GetPool(string character) 
        {
            if (Pools.TryGetValue(character, out var pool))
            {
                return pool;
            }

            var prefab = DataAccess.GetAsset<GameObject>(character);
            
            Container.BindMemoryPool<Character, Pool>()
                .WithId(character)
                .WithFactoryArguments(Requires[character], Constructor)
                .FromComponentInNewPrefab(prefab)
                .AsCached();

            pool = Container.ResolveId<Pool>(character);

            Pools.Add(character, pool);

            return pool;
        }

        protected class Pool : MemoryPool<Character> 
        {
            public Pool(CharacterRequire require, CharacterConstructor constructor) 
            {
                Require     = require;
                Constructor = constructor;

                DespawnRoot = new GameObject(require.Character + "Root").transform;
            }

            public CharacterRequire     Require     { get; }
            public CharacterConstructor Constructor { get; }
            public Transform            DespawnRoot { get; }

            [Inject]
            public GlobalDataAccess DataAccess { get; }

            protected override void Reinitialize(Character character)
            {
                Constructor.ConstructStats(character);

                Constructor.Reset(character);

                character.Rigidbody.isKinematic = false;
            }

            protected override void OnDespawned(Character character)
            {
                character.Disable();

                character.gameObject.SetActive(false);

                character.transform.SetParent(DespawnRoot);
                character.transform.localScale = Vector3.one;
            }

            protected override void OnCreated(Character character)
            {
                character.name = Require.Character;
                character.Id   = Require.Character;
                
                Constructor.ConstructProperty(character);

                Constructor.ConstructModel(character);

                Constructor.AssetInstalled(character, Require.All.Select(n =>
                {
                    var obj = DataAccess.GetAsset<UnityEngine.Object>(n);

                    if (obj is GameObject gameObject) 
                    {
                        var instantiate = Container.InstantiatePrefabForComponent<DynamicPoster>(gameObject, new[] { character });

                        instantiate.name = obj.name;

                        return instantiate;
                    }

                    return obj;
                }));

                character.CreateStateMachine();
            }
        }
    }
}
