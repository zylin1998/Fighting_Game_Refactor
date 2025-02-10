using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FightingGame
{
    public class AssetMonitor : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject>       _GameObjects       = new();
        [SerializeField]
        private List<Component>        _Components        = new();
        [SerializeField]
        private List<ScriptableObject> _ScriptableObjects = new();
        [SerializeField]
        private List<Object>           _Objects           = new();
        
        public void Add(Object obj) 
        {
            if (obj is GameObject gameObject) 
            {
                _GameObjects.Add(gameObject);

                return;
            }

            else if (obj is Component component) 
            {
                _Components.Add(component);

                return;
            }

            else if (obj is ScriptableObject scriptableObject)
            {
                _ScriptableObjects.Add(scriptableObject);

                return;
            }

            _Objects.Add(obj);
        }

        public bool Remove(Object obj)
        {
            if (obj is GameObject gameObject) 
            {
                return _GameObjects.Remove(gameObject);
            }

            else if (obj is Component component)
            {
                return _Components.Remove(component);
            }

            else if (obj is ScriptableObject scriptableObject)
            {
                return _ScriptableObjects.Remove(scriptableObject);
            }

            return _Objects.Remove(obj);
        }

        public T Get<T>() 
        {
            foreach (var obj in _Components)
            {
                if (obj is T result) { return result; }
            }

            foreach (var obj in _ScriptableObjects)
            {
                if (obj is T result) { return result; }
            }

            foreach (var obj in _Objects)
            {
                if (obj is T result) { return result; }
            }

            foreach (var obj in _GameObjects) 
            {
                var result = obj.GetComponent<T>();

                if (result != null) { return result; }
            }

            return default(T);
        }

        public IEnumerable<T> GetAll<T>()
        {
            foreach (var obj in _Components)
            {
                if (obj is T result) { yield return result; }
            }

            foreach (var obj in _ScriptableObjects)
            {
                if (obj is T result) { yield return result; }
            }

            foreach (var obj in _Objects)
            {
                if (obj is T result) { yield return result; }
            }

            foreach (var obj in _GameObjects)
            {
                var result = obj.GetComponent<T>();

                if (result != null) { yield return result; }
            }
        }

        public T GetByName<T>(string name) 
        {
            foreach (var obj in _Components)
            {
                if (obj.name != name) { continue; }

                if (obj is T result) { return result; }
            }

            foreach (var obj in _ScriptableObjects)
            {
                if (obj.name != name) { continue; }

                if (obj is T result) { return result; }
            }

            foreach (var obj in _Objects)
            {
                if (obj.name != name) { continue; }
                
                if (obj is T result) { return result; }
            }

            foreach (var obj in _GameObjects)
            {
                if (obj.name != name) { continue; }

                if (obj is T go) 
                {
                    return go; 
                }

                var result = obj.GetComponent<T>();

                if (result != null) { return result; }
            }

            return default;
        }

        public void Clear() 
        {
            _Components       .Clear();
            _ScriptableObjects.Clear();
            _Objects          .Clear();
            _GameObjects      .Clear();
        }
    }
}