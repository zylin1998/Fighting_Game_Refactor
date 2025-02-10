using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Loyufei.MVP;
using System.Collections;
using Loyufei;

namespace FightingGame
{
    [RequireComponent(typeof(CanvasGroup))]
    public class FadeOutView : MonoViewBase
    {
        [SerializeField]
        protected CanvasGroup _CanvasGroup;

        public override IEnumerator ChangeState(bool isOn)
        {
            if (isOn) gameObject.SetActive(true);

            _CanvasGroup.alpha = isOn ? 0f : 1f;

            var from = isOn ? 0f : _FadeDuration;
            var to = isOn ? _FadeDuration : 0;

            for (; from != to;)
            {
                yield return from;

                var delta = Time.unscaledDeltaTime;

                from = (from + (isOn ? delta : -delta)).Clamp(0, _FadeDuration);

                _CanvasGroup.alpha = from / _FadeDuration;
            }

            if (!isOn) { gameObject.SetActive(false); }
        }
    }
}
