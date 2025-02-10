using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;
using Zenject;
using Loyufei.MVP;
using Loyufei;

namespace FightingGame.QuestScene
{
    internal class ViewEventManager : EventNotesPresenter
    {
        public override object GroupId => Group.UI;

        [Inject]
        public RectTransform       Mask        { get; }
        [Inject]
        public CharacterCollection Collection  { get; }
        [Inject]
        public GlobalDataAccess    DataAccess  { get; }
        [Inject]
        public ViewManager         ViewManager { get; }
        [Inject]
        public QuestModel          QuestModel  { get; }
        [Inject]
        public SaveSystemModel     SaveModel   { get; }

        protected override void Init()
        {
            Add(Notes.Start        , Start);
            Add(Notes.Initialize   , DisplayBinding);
            Add(Notes.CountDown    , CountDown);
            Add(Notes.GameOver     , GameOver);
            Add(Notes.Pause        , Pause);
            Add(Notes.CutSceneON   , CutSceneOn);
            Add(Notes.CutSceneOFF  , CutSceneOff);
            Add(Notes.CutSceneONOFF, CutSceneOnOff);
            Add(Notes.Title        , BackTitle);
            Add(Notes.Restart      , Restart);
            Add(Notes.Back         , Back);
            Add(Notes.NextQuest    , NextQuest);
        }

        private void Start(object data)
        {
            ViewManager.Show(GroupUI.CutScene)
                .Subscribe((o) => { }, () =>
                {
                    SettleEvents(Group.Gaming, new Note(Notes.Initialize, default));

                    ViewManager.Close(GroupUI.CutScene)
                        .Subscribe((obj) => { }, () =>
                        {
                            SettleEvents(Group.Gaming, new Note(Notes.Start, default));
                        });
                });

            GameObject.Destroy(Mask.gameObject);
        }

        private void DisplayBinding(object data)
        {
            var view       = ViewManager.Views[GroupUI.Information] as InformationMenu;
            var initialize = (GameInitialize)data; 

            view.Initialize(initialize.GameTime);

            Collection
                .Get(initialize.PlayerTag, initialize.GUID)
                .GetModel<HealthModel>()
                .Subscribe(view.SetHealth);

            var report = DataAccess.GetModel<GameReportModel>();
            var subject = new Subject<int>();
            
            report.Subscribe<TimeSpan>(view.SetTime);
            report.Subscribe<OnGather>(view.SetCoin);
        }

        private void CountDown(object data)
        {
            var time = (float)data;

            ViewManager.Show(GroupUI.Timer);

            (ViewManager.Views[GroupUI.Timer] as TimerMenu).CountDown(time)
                .Subscribe((f) => { }, () =>
                {
                    ViewManager.Close(GroupUI.Timer);
                    ViewManager.Show(GroupUI.Information);
                });
        }

        private void GameOver(object data)
        {
            var result = (GameResult)data;

            if (result.Result) 
            {
                QuestModel.Done(); 
                
                SaveModel.Save("GameFile");
            }

            var view = ViewManager.Views[GroupUI.Result] as ResultMenu;

            view.Set(result);

            ViewManager.Close(GroupUI.Information);
            ViewManager.Show(GroupUI.Result);
        }

        private void Pause(object data) 
        {
            ViewManager.Close(GroupUI.Information);
            ViewManager.Show(GroupUI.Pause);
        }

        private void CutSceneOn(object data) 
        {
            ViewManager
                .Show(GroupUI.CutScene)
                .Subscribe((l) => { }, ((Action)data).Invoke) ;
        }

        private void CutSceneOff(object data)
        {
            ViewManager
                .Close(GroupUI.CutScene)
                .Subscribe((l) => { }, ((Action)data).Invoke);
        }

        private void CutSceneOnOff(object data) 
        {
            ViewManager
                .Show(GroupUI.CutScene)
                .Subscribe((l) => { }, () =>
                {
                    ((Action)data).Invoke();

                    ViewManager.Close(GroupUI.CutScene);
                });
        }

        private void BackTitle(object data)
        {
            ViewManager.Show(GroupUI.CutScene)
                .Subscribe((o) => { }, () =>
                {
                    SceneManager.LoadSceneAsync(0, LoadSceneMode.Single).completed += (op) =>
                    {
                        Time.timeScale = 1f;
                    };
                });
        }

        private void Restart(object data) 
        {
            ViewManager.Show(GroupUI.CutScene)
                .Subscribe((o) => { }, () =>
                {
                    SceneManager.LoadSceneAsync(1, LoadSceneMode.Single).completed += (op) =>
                    {
                        Time.timeScale = 1f;
                    };
                });
        } 

        private void NextQuest(object data)
        {
            if (!QuestModel.GoNext()) { return; }

            ViewManager.Show(GroupUI.CutScene)
                .Subscribe((o) => { }, () =>
                {
                    SceneManager.LoadSceneAsync(1, LoadSceneMode.Single).completed += (op) =>
                    {
                        Time.timeScale = 1f;
                    };
                });
        }

        private void Back(object data)
        {
            ViewManager.Close(GroupUI.Pause);
            ViewManager.Show(GroupUI.Information);
        }
    }
}
