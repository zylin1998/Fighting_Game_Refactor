using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Loyufei;
using Loyufei.MVP;
using Zenject;

namespace FightingGame.QuestScene
{
    internal class PauseMenu : FadeOutView
    {
        [SerializeField]
        private Transform   _SystemContent;
        [SerializeField]
        private Transform   _SceneContent;

        [Inject]
        public ListenerPool ListenerPool { get; }

        public override object ViewId => GroupUI.Pause;

        public override ILayout Layout()
        {
            for(var i = 1; i <= 9; i++) 
            {
                var id = (10000 + i).ToString();

                var listener = default(DynamicPoster);

                if (i <= 5) { listener = ListenerPool.Spawn("Integer_Listener"); }

                else { listener = ListenerPool.Spawn("Float_Listener"); }
                
                listener.EventId = id;
                listener.transform.SetParent(_SystemContent);
            }

            for (var i = 10; i <= 12; i++)
            {
                var id = (10000 + i).ToString();

                var listener = ListenerPool.Spawn("Button_Listener");

                listener.EventId = id;
                listener.transform.SetParent(_SceneContent);
                listener.GroupId = Group.UI;
            }

            return base.Layout();
        }

        public override IEnumerator ChangeState(bool isOn)
        {
            if (isOn)
            {
                Time.timeScale = 0f;

                gameObject.SetActive(true);
            }

            _CanvasGroup.alpha = isOn ? 0f : 1f;

            var from = isOn ? 0f : _FadeDuration;
            var to   = isOn ? _FadeDuration : 0;

            for (; from != to;)
            {
                yield return from;

                var delta = Time.unscaledDeltaTime;

                from = (from + (isOn ? delta : -delta)).Clamp(0, _FadeDuration);

                _CanvasGroup.alpha = from / _FadeDuration;
            }

            if (!isOn) 
            {
                gameObject.SetActive(false); 

                Time.timeScale = 1f;
            }
        }
    }
}
