using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Loyufei.MVP;

namespace FightingGame.QuestScene
{
    internal class QuestAssetPresenter : ModelPresenter<GlobalDataAccess>
    {
        [Inject]
        public Canvas                Canvas         { get; }
        [Inject]
        public DiContainer           Container      { get; }
        [Inject]
        public RequireItemAsset      ItemAsset      { get; }
        [Inject]
        public RequireViewAsset      ViewAsset      { get; }
        [Inject]
        public CharacterRequireAsset CharacterAsset { get; }
        [Inject]
        public ViewManager           ViewManager    { get; }
        [Inject]
        public QuestModel            QuestModel     { get; }
        [Inject]
        public AudioModel            AudioModel     { get; }

        public override object GroupId => Group.System;

        protected override void Init()
        {
            Model.LoadObjects(GetKeys().Distinct()).Completed += (op) =>
            {
                GetViews (op.Result).ToList();
                GetAudios(op.Result);

                SettleEvents(Group.System, new Note(Notes.Read, default));

                SettleEvents(Group.Gaming, new Note(Notes.Loaded, QuestModel.Current));
            };
        }

        private IEnumerable<MonoViewBase> GetViews(IList<UnityEngine.Object> list) 
        {
            foreach (var obj in list)
            {
                if (!(obj is GameObject go)) { continue; }
                
                var mono = go.GetComponent<MonoViewBase>();
                
                if (mono) { yield return CreateView(mono); }
            }
        }

        private void GetAudios(IList<UnityEngine.Object> list) 
        {
            var (select, click) = (default(AudioClip), default(AudioClip));

            foreach (var obj in list)
            {
                if (!(obj is AudioClip clip)) { continue; }

                if (clip.name == ViewAsset.Select) { select = clip; continue; }
                if (clip.name == ViewAsset.Click ) { click  = clip; continue; }
                
                if (clip.name == QuestModel.Current.BGM ) 
                {
                    AudioModel.PlayLoop("BGM", clip);    
                }
            }

            AudioModel.SetSelectableClip(select, click);
        }

        private MonoViewBase CreateView(MonoViewBase mono) 
        {
            var type   = mono.GetType();
            var parent = Canvas.transform;
            var args   = new object[] { };
            var view   = Container
                .InstantiatePrefabForComponent(type, mono, parent, args) as MonoViewBase;

            view.transform.localPosition = Vector3.zero;
            ((RectTransform)view.transform).sizeDelta = new Vector2(Screen.width, Screen.height);

            ViewManager.Register(view);

            view.Layout();

            return view;
        }

        private IEnumerable<string> GetKeys() 
        {
            var quest      = QuestModel.Current;
            var characters = quest.Characters;
            
            yield return quest.Environment;
            yield return quest.BGM;

            foreach (var require in characters.Select(c => CharacterAsset[c]))
            {
                foreach (var key in require.AllWithFacade) 
                {
                    yield return key;
                }
            }

            foreach (var key in ItemAsset.Items) 
            {
                yield return key;
            }

            foreach (var key in ViewAsset.All) 
            {
                yield return key;
            }
        }
    }
}
