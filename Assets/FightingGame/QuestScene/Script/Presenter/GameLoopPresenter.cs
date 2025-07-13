using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Loyufei.MVP;
using StateMachineX;

namespace FightingGame.QuestScene
{
    internal class GameLoopPresenter : ModelPresenter<GameLoop>
    {
        private float _CountDown = 0;

        [Inject]
        public EventNotes EventNotes { get; }
        
        public override object GroupId => Group.Gaming;

        public IStateMachine Machine  { get; private set; } 

        public IDisposable   Register { get; private set; }

        protected override void Init()
        {
            EventNotes.Add(GroupId , Notes.Loaded    , OnAssetLoad);
            EventNotes.Add(GroupId , Notes.Initialize, Initialize);
            EventNotes.Add(GroupId , Notes.Start     , Enable);
            EventNotes.Add(Group.UI, Notes.Restart   , Disable);
            EventNotes.Add(Group.UI, Notes.NextQuest , Disable);
            EventNotes.Add(Group.UI, Notes.Title     , Disable);
        }

        private void OnAssetLoad(object data) 
        {
            Model.LoadQuest((QuestInfo)data);

            var count = StateMachine.FunctionalState()
                .ExitWhen(() => _CountDown <= 0)
                .DoOnEnter(StartCountDown)
                .DoFixedTick(() => _CountDown -= Time.fixedDeltaTime)
                .WithId(1);

            var enable = StateMachine.FunctionalState()
                .DoOnEnter(Model.StartLoop)
                .WithId(2);

            var looping = StateMachine.FunctionalState()
                .ExitWhen(Model.GameOver)
                .DoFixedTick(Model.Looping)
                .WithId(3);

            var disable = StateMachine.FunctionalState()
                .DoOnEnter(GameOver)
                .WithId(4);

            Machine = StateMachine.SingleEntrance()
                .WithStates(count, enable, looping, disable)
                .Sequence()
                .OrderBy(1, 2, 3, 4);

            SettleEvents(Group.UI, new Note(Notes.Start, default));
        }

        private void Initialize(object obj) 
        {
            SettleEvents(Group.UI, new Note(Notes.Initialize, Model.Initialize()));
        }

        private void Enable(object data) 
        {
            Register?.Dispose();

            Register = Machine.FixedUpdate();
        }

        private void Disable(object data) 
        {
            Register.Dispose();

            Model.Recycle();
        }

        private void GameOver() 
        {
            Register.Dispose();

            Model.Looping();

            var result = Model.GameResult;

            if (result.Result)
            {
                SettleEvents(Group.Item, new Note(Notes.AddCurrency, new Add(20001, result.Gather)));
            }

            SettleEvents(Group.UI  , new Note(Notes.GameOver, result));
        }

        private void StartCountDown() 
        {
            _CountDown = 3;

            SettleEvents(Group.UI, new Note(Notes.CountDown, 3f));
        }
    }
}
