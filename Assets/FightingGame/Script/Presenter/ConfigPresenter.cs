using System;
using System.Linq;
using System.Collections.Generic;
using UniRx;
using Zenject;
using Loyufei.MVP;
using Loyufei;
using UnityEngine;

namespace FightingGame
{
    public class ConfigPresenter : EventNotesPresenter, IInitializable
    {
        [Inject]
        public ScreenModel   Screen   { get; }
        [Inject]
        public AudioModel    Audio    { get; }
        [Inject]
        public LanguageModel Language { get; }
        [Inject]
        public DataRefresher Refresh  { get; }

        [Inject]
        public ViewManager     Manager   { get; }
        [Inject]
        public SaveSystemModel SaveModel { get; }

        public override object GroupId => Group.System;

        public void Initialize() 
        {
            Screen.Initialize();
            Audio .Initialize();

            Language.Subscribe(SetAllText);
        }

        protected override void Init()
        {
            Add(Notes.ScreenModel, SetScreenMode);
            Add(Notes.Resolution , SetResolution);
            Add(Notes.FPS        , SetFPS);
            Add(Notes.Vsync      , SetVsync);
            Add(Notes.Language   , SetLanguage);
            Add(Notes.Master     , SetMaster);
            Add(Notes.BGM        , SetBGM);
            Add(Notes.SE         , SetSE);
            Add(Notes.SFX        , SetSFX);
            Add(Notes.Read       , Read);
        }

        private void SetScreenMode(object data) 
        {
            Screen.SetScreenMode((int)data);

            SaveModel.Save("System");

            var uuid  = "10001";
            var other = Screen.Properties[0].Value;

            Refresh.Refresh("100011", Language[uuid, other]);
        }

        private void SetResolution(object data)
        {
            Screen.SetResolution((int)data);

            SaveModel.Save("System");

            Refresh.Refresh("100021", Screen.Current.ToString());
        }

        private void SetFPS(object data)
        {
            Screen.SetFPS((int)data);

            SaveModel.Save("System");

            var uuid = "10003";
            var other = Screen.Properties[2].Value;

            Refresh.Refresh("100031", Language[uuid, other]);
        }

        private void SetVsync(object data)
        {
            Screen.SetVsync((int)data);

            SaveModel.Save("System");

            var uuid = "10004";
            var other = Screen.Properties[3].Value;

            Refresh.Refresh("100041", Language[uuid, other]);
        }

        private void SetLanguage(object data)
        {
            Language.Set((int)data);

            SaveModel.Save("System");

            Action action = () =>
            {
                Language.Read();
            };

            SettleEvents(Group.UI, new Note(Notes.CutSceneONOFF, action));
        }

        private void SetMaster(object data)
        {
            if (data is float volumn) { Audio.Set("Master", volumn); }

            if (data is bool  mute  ) { Audio.Set("Master", mute);   }

            SaveModel.Save("System");
        }

        private void SetBGM(object data)
        {
            if (data is float volumn) { Audio.Set("BGM", volumn); }

            if (data is bool  mute  ) { Audio.Set("BGM", mute);   }

            SaveModel.Save("System");
        }

        private void SetSE(object data)
        {
            if (data is float volumn) { Audio.Set("SE", volumn); }

            if (data is bool  mute  ) { Audio.Set("SE", mute);   }

            SaveModel.Save("System");
        }

        private void SetSFX(object data)
        {
            if (data is float volumn) { Audio.Set("SFX", volumn); }

            if (data is bool  mute  ) { Audio.Set("SFX", mute);   }

            SaveModel.Save("System");
        }

        private void Read(object data) 
        {
            Language.Read();
        }

        private void SetAllText(IEnumerable<LanguageModel.Context> texts) 
        {
            foreach (var text in texts)
            {
                Refresh.Refresh(text.UUID, text.Title);
            }

            Refresh.Refresh("100011", Screen.Properties[0].Value);
            Refresh.Refresh("100021", Screen.Properties[1].Value);
            Refresh.Refresh("100031", Screen.Properties[2].Value);
            Refresh.Refresh("100041", Screen.Properties[3].Value);
            Refresh.Refresh("100051", Language.Property.Value);
            Refresh.Refresh("100011", Language["10001", Screen.Properties[0].Value]);
            Refresh.Refresh("100021", Screen.Current);
            Refresh.Refresh("100031", Language["10003", Screen.Properties[2].Value]);
            Refresh.Refresh("100041", Language["10004", Screen.Properties[3].Value]);
            Refresh.Refresh("100051", Language["10005", Language.Property.Value]);
            Refresh.Refresh("100061", Audio.Volumns["Master"].Value);
            Refresh.Refresh("100071", Audio.Volumns["BGM"]   .Value);
            Refresh.Refresh("100081", Audio.Volumns["SE"]    .Value);
            Refresh.Refresh("100091", Audio.Volumns["SFX"]   .Value);
            Refresh.Refresh("100062", Audio.Mutes  ["Master"].Value);
            Refresh.Refresh("100072", Audio.Mutes  ["BGM"]   .Value);
            Refresh.Refresh("100082", Audio.Mutes  ["SE"]    .Value);
            Refresh.Refresh("100092", Audio.Mutes  ["SFX"]   .Value);
        }
    }
}
