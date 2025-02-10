using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Loyufei.MVP;

namespace FightingGame.TitleScene
{
    internal class ConfigMenu : FadeOutView
    {
        [SerializeField]
        private Transform _ConfigContent;
        [SerializeField]
        private Transform _BackContent;

        public override object ViewId => GroupUI.Config;

        [Inject]
        public ListenerPool ListenerPool { get; }

        public override ILayout Layout()
        {
            for (var i = 10001; i <= 10009; i++)
            {
                var name     = i <= 10005 ? "Integer_Listener" : "Float_Listener";
                var listener = ListenerPool.Spawn(name);

                listener.transform.SetParent(_ConfigContent);
                listener.EventId = i.ToString();
                listener.GroupId = Group.System;
            }

            var back = ListenerPool.Spawn("Button_Listener");

            back.transform.SetParent(_BackContent);
            back.EventId = Notes.Back;
            back.GroupId = Group.UI;
            return base.Layout();
        }
    }
}
