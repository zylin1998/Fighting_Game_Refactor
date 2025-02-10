using System;
using System.Linq;
using System.Collections.Generic;
using Loyufei;
using UnityEngine;

namespace FightingGame
{
    [CreateAssetMenu(fileName = "SystemPropertyFile", menuName = "FightingGame/System/File", order = 1)]
    public class ConfigFileInstaller : FileInstallAsset<ConfigProperty, Channel<ConfigProperty>>
    {
        protected override void BindingAssets()
        {
            Container
                .Bind<ScreenModel>()
                .AsSingle();
            
            Container
                .Bind<AudioModel>()
                .AsSingle();
            
            Container
                .Bind<LanguageModel>()
                .AsSingle();

            Container
                .Bind<DataRefresher>()
                .AsSingle();
        }
    }
}
