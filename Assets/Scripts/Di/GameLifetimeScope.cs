using VContainer;
using VContainer.Unity;
using Dragoraptor.Core;
using Dragoraptor.Interfaces;
using Dragoraptor.Services;
using Dragoraptor.Ui;
using TimersService;
using EventBus;
using UnityEngine;
using UnityEngine.UIElements;


namespace Dragoraptor.Di
{
    public class GameLifetimeScope : LifetimeScope
    {

        [SerializeField] private SceneObjectsContainer _sceneObjectsContainer;
        

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<GameStarter>();
            
            builder.Register<TimersServiceBehaviour>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<EventBusBehaviour>(Lifetime.Singleton).As<IEventBus>();

            builder.Register<GameStateManager>(Lifetime.Singleton).AsSelf();

            builder.Register<UiFactory>(Lifetime.Singleton)
                .WithParameter<UIDocument>(_sceneObjectsContainer.GetUIDocument)
                .AsSelf();
            builder.Register<UiManager>(Lifetime.Singleton).AsSelf();
            builder.Register<MainScreenWidget>(Lifetime.Singleton).AsSelf();
            builder.Register<HuntScreenWidget>(Lifetime.Singleton).AsSelf();
            builder.Register<HuntMenuWidget>(Lifetime.Singleton).AsSelf();

            builder.Register<VictoryPossibilityStump>(Lifetime.Singleton).As<IVictoryPossibilityHolder>();



        }
    }
}