using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Loyufei.MVP;

namespace FightingGame.TitleScene
{
    internal class TitleAssetPresenter : ModelPresenter<GlobalDataAccess>
    {
        [Inject]
        public Canvas           Canvas      { get; }
        [Inject]
        public DiContainer      Container   { get; }
        [Inject]
        public RequireViewAsset ViewAsset   { get; }
        [Inject]
        public ViewManager      ViewManager { get; }
        [Inject]
        public AudioModel       Audio       { get; }

        protected override void Init()
        {
            Model.LoadObjects(GetKeys()).Completed += (op) => 
            {
                GetViews (op.Result).ToList();
                GetAudios(op.Result);

                SettleEvents(Group.System, new Note(Notes.Read, default));
                SettleEvents(Group.Item  , new Note(Notes.Read, default));
                SettleEvents(Group.UI, new Note(Notes.Initialize, default));
            };
        }

        private IEnumerable<string> GetKeys() 
        {
            foreach (var key in ViewAsset.All) 
            {
                yield return key;
            }
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
                if (clip.name == ViewAsset.Click) { click = clip; continue; }

                if (clip.name == "Menu_Audio")
                {
                    Audio.PlayLoop("BGM", clip);
                }
            }

            Audio.SetSelectableClip(select, click);
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
    }
}
