using UnityEngine;
using UnityEngine.UIElements;

using VContainer;
using VContainer.Unity;

using TimersService;
using EventBus;

using Dragoraptor.Character;
using Dragoraptor.Core;
using Dragoraptor.Interfaces;
using Dragoraptor.MonoBehs;
using Dragoraptor.Services;
using Dragoraptor.Ui;
using Interfaces;


namespace Dragoraptor.Di
{
    public class GameLifetimeScope : LifetimeScope
    {

        [SerializeField] private SceneObjectsContainer _sceneObjectsContainer;
        [SerializeField] private DataContainerBehaviour _dataContainer;

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


            builder.Register<SceneController>(Lifetime.Singleton).AsImplementedInterfaces();
            

            builder.Register<VictoryPossibilityStump>(Lifetime.Singleton).As<IVictoryPossibilityHolder>();
            builder.Register<LevelLoaderStump>(Lifetime.Singleton).As<ICurrentLevelDescriptorHolder>();

            //  -----  Character system  -----
            builder.Register<CharacterManager>(Lifetime.Singleton).AsImplementedInterfaces();
            
            
            // -------------------------

            builder.Register<PrefabLoader>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.RegisterInstance(_dataContainer).As<IDataHolder>();

        }
    }
}