using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FightingGame
{
    public class CreateSpot : MonoBehaviour
    {
        [SerializeField, Range(-1f, 1f)]
        private float _Side;

        public void Set(Character character) 
        {
            character.gameObject.SetActive(true);

            character.tag = tag;
            character.gameObject.layer = transform.gameObject.layer;
            
            character.transform.SetParent(transform);
            character.transform.SetPositionAndRotation(transform.position, transform.rotation);

            var side  = _Side >= 0 ? 1f : -1f;
            var scale = character.transform.localScale;

            character.transform.localScale = new Vector3(scale.x * side, scale.y, scale.z);

            foreach (Component component in character.GetAssetAll<Component>())
            {
                component.tag = tag;
            }
        }
    }
}
