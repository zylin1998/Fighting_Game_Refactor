using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Loyufei.MVP;

namespace FightingGame.QuestScene
{
    internal class InformationMenu : FadeOutView
    {
        [SerializeField]
        private HealthDisplay     _Health;
        [SerializeField]
        private TimeDisplay       _Time;
        [SerializeField]
        private CoinGatherDisplay _Coin;
        [SerializeField]
        private Transform         _PauseContent;

        public override object ViewId => GroupUI.Information;

        [Inject]
        public ListenerPool ListenerPool { get; }
        [Inject]
        public CurrencyMonitor Currency { get; }

        public override ILayout Layout()
        {
            var listener = ListenerPool.Spawn("Pause_Listener");
            
            listener.transform.SetParent(_PauseContent);
            listener.transform.localPosition = Vector3.zero;

            listener.EventId = Notes.Pause;
            listener.GroupId = Group.UI;

            _Coin.Set(Currency.Get((object)20001).Item.Icon);

            return base.Layout();
        }

        public void SetHealth(HealthModel.Result result) 
        {
            _Health.Set(result.Normalized);
        }

        public void SetTime(TimeSpan time) 
        {
            _Time.Set(time);
        }

        public void SetCoin(OnGather gather) 
        {
            _Coin.Set(gather.Gather);
        }

        public void Initialize(TimeSpan gameTime) 
        {
            _Health.Initialize();
            _Coin  .Initialize();
            _Time  .Set(gameTime);
        }
    }
}
