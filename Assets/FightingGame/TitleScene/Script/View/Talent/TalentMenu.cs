using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Zenject;
using Loyufei.MVP;

namespace FightingGame.TitleScene
{
    internal class TalentMenu : FadeOutView
    {
        [SerializeField]
        private Transform       _TalentContent;
        [SerializeField]
        private Transform       _BackContent;
        [SerializeField]
        private TextMeshProUGUI _Currency;

        [Inject]
        public ListenerPool  ListenerPool { get; }
        [Inject]
        public TradeMonitor  TradeMonitor { get; }
        [Inject]
        public DataRefresher Refresher    
        {
            set
            {
                value.Register(20001, (obj) => _Currency.SetText(obj.ToString()));
            }
        }

        public override object ViewId => GroupUI.Talent;

        public override ILayout Layout()
        {
            foreach (var tradeInfo in TradeMonitor.GetAll()) 
            {
                var listener = ListenerPool.Spawn("Talent_Listener") as TalentListener;

                listener.transform.SetParent(_TalentContent);
                listener.EventId = Notes.TradeTalent;
                listener.GroupId = Group.Item;
                listener.SetInfo(tradeInfo.info);
            }

            var back = ListenerPool.Spawn("Button_Listener");

            back.transform.SetParent(_BackContent);
            back.EventId = Notes.Back;
            back.GroupId = Group.UI;

            return base.Layout();
        }
    }
}
