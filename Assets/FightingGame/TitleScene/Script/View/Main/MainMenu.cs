using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Loyufei.MVP;

namespace FightingGame.TitleScene
{
    internal class MainMenu : FadeOutView
    {
        [SerializeField]
        private Transform _MainContent;

        public override object ViewId => GroupUI.Main;

        [Inject]
        public ListenerPool ListenerPool { get; }

        public override ILayout Layout()
        {
            for (var i = 10016; i <= 10019; i++)
            {
                var listener = ListenerPool.Spawn("Menu_Listener");

                listener.transform.SetParent(_MainContent);
                listener.EventId = i.ToString();
                listener.GroupId = Group.UI;
            }

            return base.Layout();
        }
    }
}
