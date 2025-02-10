using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using TMPro;
using Loyufei.MVP;

namespace FightingGame.TitleScene
{
    internal class ConfirmMenu : FadeOutView
    {
        [SerializeField]
        private Transform       _Content;
        [SerializeField]
        private TextMeshProUGUI _Message;

        public override object ViewId => GroupUI.Confirm;

        [Inject]
        public ListenerPool ListenerPool { get; }

        [Inject]
        public DataRefresher Refresher
        {
            set
            {
                value.Register(Notes.ShouldQuit, SetMessage);
            }
        }

        public override ILayout Layout()
        {
            for (var i = 10020; i <= 10021; i++)
            {
                var listener = ListenerPool.Spawn("Confirm_Listener");

                listener.transform.SetParent(_Content);
                listener.EventId = i.ToString();
                listener.GroupId = Group.UI;
            }

            return base.Layout();
        }

        private void SetMessage(object message) 
        {
            _Message.SetText(message.ToString());
        }
    }
}
