using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Loyufei.MVP;

namespace FightingGame
{
    public class CutSceneMenu : MonoViewBase
    {
        [SerializeField]
        private Transform _Left;
        [SerializeField]
        private Transform _Right;

        public override object ViewId => "CutScene";

        public override IEnumerator ChangeState(bool isOn)
        {
            if (isOn == gameObject.activeSelf) { yield break; }

            if (isOn) { gameObject.SetActive(true); }

            var wait = _FadeDuration;

            var side = (isOn ? -2000 : 2000);

            var right = _Right.position + Vector3.right * side;
            var left  = _Left .position + Vector3.left  * side;
            
            var speed = Mathf.Abs(1 / _FadeDuration * side);

            for (; wait >= 0f;)
            {
                yield return wait;

                var deltaTime = Time.unscaledDeltaTime;
                
                wait -= deltaTime;
                
                _Right.transform.position = Vector3.MoveTowards(_Right.transform.position, right, speed * deltaTime);
                _Left .transform.position = Vector3.MoveTowards(_Left .transform.position,  left, speed * deltaTime);
            }

            if (!isOn) { gameObject.SetActive(false); }
        }
    }
}
