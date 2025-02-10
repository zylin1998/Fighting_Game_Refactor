using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Loyufei;
using System;

namespace FightingGame
{
    [CreateAssetMenu(fileName = "QuestFile", menuName = "FightingGame/Quest/File", order = 1)]
    public class QuestFile : FileInstallAsset<IQuestLog, QuestFile.Channel>
    {
        [SerializeField]
        private QuestSelector  _Selector;
        [SerializeField]
        private QuestInfoAsset _InfoAsset;

        protected override void BindingAssets()
        {
            Container
                .Bind<QuestSelector>()
                .FromInstance(_Selector)
                .AsSingle();

            Container
                .Bind<QuestInfoAsset>()
                .FromInstance(_InfoAsset)
                .AsSingle();

            Container
                .Bind<QuestModel>()
                .AsSingle();
        }

        [Serializable]
        public class Channel : Channel<IQuestLog>
        {
            public override bool GetOrCreate(out IQuestLog instance)
            {
                var added = false;

                instance = _Saveable.GetOrAdd(Identity, () =>
                {
                    added = true;

                    return new QuestLog();
                });

                return added;
            }
        }
    }
}
