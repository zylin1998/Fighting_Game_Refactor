using UnityEngine;
using UnityEngine.Audio;
using Zenject;
using Loyufei.DomainEvents;

namespace FightingGame.TitleScene
{
    internal class TitleSceneInstaller : MonoInstaller
    {
        [SerializeField]
        private RequireViewAsset _ViewAsset;
        [SerializeField]
        private AudioMixer       _AudioMixer;

        public override void InstallBindings()
        {
            BindPresenter();
            BindInventory();
            BindAsset();
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
                .BindInterfacesAndSelfTo<TitleAssetPresenter>()
                .AsSingle()
                .NonLazy();

            Container
                .Bind<DynamicRegistration>()
                .AsSingle()
                .NonLazy();

            Container
                .Bind<ItemPresenter>()
                .AsSingle()
                .NonLazy();

            Container
                .Bind<ViewManagePresenter>()
                .AsSingle()
                .NonLazy();
        }

        private void BindInventory() 
        {
            Container
                .Bind<Trade>()
                .AsSingle();

            Container
                .Bind<Currency>()
                .AsSingle();

            Container
                .Bind<Talent>()
                .AsSingle();

            Container
                .Bind<TradeMonitor>()
                .AsSingle();

            Container
                .Bind<CurrencyMonitor>()
                .AsSingle();

            Container
                .Bind<TalentMonitor>()
                .AsSingle();

            Container
                .Bind<SaveSystemModel>()
                .AsSingle();
        }

        private void BindAsset()
        {
            Container
                .Bind<RequireViewAsset>()
                .FromInstance(_ViewAsset)
                .AsSingle();

            Container
                .Bind<GlobalDataAccess>()
                .AsSingle();

            Container
                .Bind<AudioMixer>()
                .FromInstance(_AudioMixer)
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