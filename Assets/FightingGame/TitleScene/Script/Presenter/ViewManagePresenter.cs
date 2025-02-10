using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;
using Zenject;
using Loyufei.MVP;

namespace FightingGame.TitleScene
{
    internal class ViewManagePresenter : EventNotesPresenter
    {
        public override object GroupId => Group.UI;

        [Inject]
        public RectTransform Mask        { get; }
        [Inject]
        public ViewManager   ViewManager { get; }
        [Inject]
        public QuestModel    QuestModel  { get; }

        private object _CurrentView;

        private Action _Confirm = () => { };

        protected override void Init()
        {
            Add(Notes.Initialize   , Initialize);            
            Add(Notes.Quest        , QuestMenu);
            Add(Notes.Talent       , TalentMenu);
            Add(Notes.Config       , ConfigMenu);
            Add(Notes.End          , EndGame);
            Add(Notes.Back         , Back);
            Add(Notes.CutSceneONOFF, CutSceneONOFF);
            Add(Notes.Confirm      , Confirm);
            Add(Notes.Cancel       , Cancel);
            Add(Notes.QuestSelect  , QuestSelect);
        }

        private void Initialize(object data) 
        {
            ViewManager
                .Close(GroupUI.CutScene)
                .Subscribe((o) => { }, () =>
                {
                    ViewManager.Show(GroupUI.Back);
                    ViewManager.Show(GroupUI.Main);

                    _CurrentView = GroupUI.Main;
                });

            GameObject.Destroy(Mask.gameObject);
        }

        private void QuestMenu(object data) 
        {
            ViewManager
                .Close(_CurrentView)
                .Subscribe((o) => { }, () =>
                {
                    ViewManager.Show(GroupUI.Quest);
                    
                    _CurrentView = GroupUI.Quest;
                });
        }

        private void TalentMenu(object data) 
        {
            ViewManager
                .Close(_CurrentView)
                .Subscribe((o) => { }, () =>
                {
                    ViewManager.Show(GroupUI.Talent);

                    _CurrentView = (GroupUI.Talent);
                });
        }

        private void ConfigMenu(object data)
        {
            ViewManager
                .Close(_CurrentView)
                .Subscribe((o) => { }, () =>
                {
                    ViewManager.Show(GroupUI.Config);

                    _CurrentView = GroupUI.Config;
                });
        }

        private void EndGame(object data)
        {
            _Confirm = () =>
            {
                ViewManager
                    .Show(GroupUI.CutScene)
                    .Subscribe((o) => { }, () =>
                    {
                        Application.Quit();
                    });
            };

            ViewManager
                .Show(GroupUI.Confirm);
        }

        private void Back(object data) 
        {
            ViewManager
                .Close(_CurrentView)
                .Subscribe((o) => { }, () =>
                {
                    ViewManager.Show(GroupUI.Main);

                    _CurrentView = GroupUI.Main;
                });
        }

        private void CutSceneONOFF(object data) 
        {
            var action = (Action)data;

            ViewManager
                .Show(GroupUI.CutScene)
                .Subscribe((o) => { }, () =>
                {
                    action.Invoke();

                    ViewManager.Close(GroupUI.CutScene);
                });
        }

        private void Confirm(object data) 
        {
            _Confirm?.Invoke();

            ViewManager.Close(GroupUI.Confirm);
        }

        private void Cancel(object data)
        {
            _Confirm = () => { };

            ViewManager.Close(GroupUI.Confirm);
        }

        private void QuestSelect(object data) 
        {
            var id = (int)data;

            if (QuestModel.GoTo(id)) 
            {
                ViewManager
                    .Show(GroupUI.CutScene)
                    .Subscribe((o) => { }, () =>
                    {
                        SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
                    });
            }
        }
    }
}
