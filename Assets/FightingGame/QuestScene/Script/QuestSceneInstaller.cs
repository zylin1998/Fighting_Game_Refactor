using UnityEngine;
using UnityEngine.Audio;
using Zenject;
using Loyufei.DomainEvents;

namespace FightingGame.QuestScene
{
    internal class QuestSceneInstaller : MonoInstaller
    {
        [SerializeField]
        private PropertyAsset         _ExtraStat;
        [SerializeField]
        private CharacterRequireAsset _Require;
        [SerializeField]
        private RequireViewAsset      _RequireView;
        [SerializeField]
        private RequireItemAsset      _RequireItem;
        [SerializeField]
        private AudioMixer            _AudioMixer;
        
        public override void InstallBindings()
        {
            BindPresenter();
            BindSystem();
            BindQuest();
            BindAsset();
            BindEnvironment();
            BindCharacter();
            BindInventory();            
            BindEvent();
            BindView();
        }

        private void BindPresenter() 
        {
            Container
                .BindInterfacesAndSelfTo<ConfigPresenter>()
                .AsSingle()
                .NonLazy();

            Container
                .BindInterfacesAndSelfTo<QuestAssetPresenter>()
                .AsSingle()
                .NonLazy();

            Container
                .Bind<ItemPresenter>()
                .AsSingle()
                .NonLazy();

            Container
                .Bind<ViewEventManager>()
                .AsSingle()
                .NonLazy();

            Container
                .Bind<GameLoopPresenter>()
                .AsSingle()
                .NonLazy();

            Container
                .Bind<CharacterModelPresenter>()
                .AsSingle()
                .NonLazy();

            Container
                .Bind<DynamicRegistration>()
                .AsSingle()
                .NonLazy();
        }

        private void BindSystem() 
        {
            Container
                .Bind<AudioMixer>()
                .FromInstance(_AudioMixer)
                .AsSingle();

            Container
                .Bind<SaveSystemModel>()
                .AsSingle();
        }

        private void BindQuest() 
        {
            Container
                .Bind<GameReportModel>()
                .AsSingle();

            Container
                .Bind<GameLoop>()
                .AsSingle();
        }

        private void BindAsset() 
        {
            Container
                .Bind<PropertyAsset>()
                .WithId("Extra")
                .FromInstance(_ExtraStat)
                .AsCached();

            Container
                .Bind<CharacterRequireAsset>()
                .FromInstance(_Require)
                .AsSingle();

            Container
                .Bind<RequireViewAsset>()
                .FromInstance(_RequireView)
                .AsSingle();

            Container
                .Bind<RequireItemAsset>()
                .FromInstance(_RequireItem)
                .AsSingle();

            Container
                .Bind<GlobalDataAccess>()
                .AsSingle();
        }

        private void BindEnvironment() 
        {
            Container
                .Bind<EnvironmentPool>()
                .AsSingle();
        }

        private void BindCharacter()
        {
            Container
                .Bind<CharacterCollection>()
                .AsSingle();

            Container
                .Bind<CharacterPool>()
                .WithId("Player")
                .To<PlayerCharacterPool>()
                .AsCached();

            Container
                .Bind<CharacterPool>()
                .WithId("Enemy")
                .To<EnemyCharacterPool>()
                .AsCached();

            Container
                .Bind<CharacterConstructor>()
                .WithId("Player")
                .To<PlayerCharacterConstructor>()
                .AsCached();

            Container
                .Bind<CharacterConstructor>()
                .WithId("Enemy")
                .To<EnemyCharacterConstructor>()
                .AsCached();

            Container
                .Bind<CharacterModels>()
                .AsSingle();
        }

        private void BindInventory() 
        {
            Container
                .Bind<ItemFacadePool>()
                .AsSingle();

            Container
                .Bind<Currency>()
                .AsSingle();

            Container
                .Bind<Talent>()
                .AsSingle();

            Container
                .Bind<Trade>()
                .AsSingle();

            Container
                .Bind<TalentMonitor>()
                .AsSingle();

            Container
                .Bind<CurrencyMonitor>()
                .AsSingle();

            Container
                .Bind<TradeMonitor>()
                .AsSingle();
        }

        private void BindEvent() 
        {
            SignalBusInstaller.Install(Container);

            Container
                .Bind<IDomainEventBus>()
                .To<DomainEventBus>()
                .AsSingle();

            Container
                .Bind<EventNotes>()
                .AsSingle();
        }

        private void BindView() 
        {
            Container
                .Bind<ViewManager>()
                .AsSingle();

            Container
                .Bind<ListenerPool>()
                .AsSingle();
        }
    }
}