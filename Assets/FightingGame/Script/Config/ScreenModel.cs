using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace FightingGame
{
    public class ScreenModel : PropertyModel
    {
        public ScreenModel(GlobalDataAccess dataAccess, [Inject(Id = "System")] ConfigProperty file) 
        {
            _Standard = new[]
            {
                new Property<int>("ScreenMode", 0),
                new Property<int>("Resolution", 0),
                new Property<int>("FPS"       , 0),
                new Property<int>("Vsync"     , 0),
            };

            Properties = _Standard.Select(p => file.GetInteger(p.Id, p.Value)).ToArray();

            foreach (var p in Properties) { dataAccess.Install(p); }

            dataAccess.Install(this);
        }

        private Property<int>[] _Standard;

        public Property<int>[] Properties { get; }

        public Resolution[] Resolutions => GetResolutions().ToArray();

        public string Current
        {
            get
            {
                var r = Resolutions[Properties[1].Value];

                return string.Format("{0} x {1}", r.width, r.height);
            }
        }

        public void Initialize() 
        {
            SetScreenMode(Properties[0].Value, true);
            SetResolution(Properties[1].Value, true);
            SetFPS       (Properties[2].Value, true);
            SetVsync     (Properties[3].Value, true);
        }

        public void SetScreenMode(int screenMode, bool force = false) 
        {
            var result = screenMode % 4;

            if (result == Properties[0].Value && !force) { return; }

            Properties[0].Set(result);
            
            Screen.fullScreenMode = (FullScreenMode)result;
        }

        public void SetResolution(int resolution, bool force = false)
        {
            var result = resolution % Resolutions.Length;

            if (result == Properties[1].Value && !force) { return; }

            Properties[1].Set(result);

            var r = Resolutions[Properties[1].Value];

            Screen.SetResolution(r.width, r.height, (FullScreenMode)Properties[0].Value);
        }

        public void SetFPS(int fps, bool force = false)
        {
            var result = fps % 3;

            if (result == Properties[2].Value && !force) { return; }

            Properties[2].Set(result);

            Application.targetFrameRate = result == 0 ? -1 : 30 * 2 / result;
        }

        public void SetVsync(int value, bool force = false) 
        {
            var result = value % 2;

            if (result == Properties[3].Value && !force) { return; }

            Properties[3].Set(result);

            QualitySettings.vSyncCount = (result == 0 ? 0 : 1);
        }

        private IEnumerable<Resolution> GetResolutions() 
        {
            foreach (var resolution in Screen.resolutions.Reverse()) 
            {
                if ((float)resolution.width / resolution.height < 1.6f) { continue; } 

                if (resolution.width < 1280 || resolution.width < 720) { continue; }

                yield return resolution;
            }
        }
    }
}
