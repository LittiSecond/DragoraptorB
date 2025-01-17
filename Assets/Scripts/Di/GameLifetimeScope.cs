﻿using UnityEngine;
using UnityEngine.UIElements;

using VContainer;
using VContainer.Unity;

using TimersService;
using EventBus;
using ObjPool;

using Dragoraptor.Character;
using Dragoraptor.Core;
using Dragoraptor.Core.NpcManagment;
using Dragoraptor.Input;
using Dragoraptor.Interfaces;
using Dragoraptor.Interfaces.Character;
using Dragoraptor.Interfaces.Ui;
using Dragoraptor.Interfaces.Npc;
using Dragoraptor.MonoBehs;
using Dragoraptor.Ui;


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
            builder.Register<UpdateService>(Lifetime.Singleton).AsImplementedInterfaces();

            builder.Register<GameStateManager>(Lifetime.Singleton).AsSelf();
            builder.Register<GameProgress>(Lifetime.Singleton).AsImplementedInterfaces();

            // -------  Ui System --------
            builder.Register<UiFactory>(Lifetime.Singleton)
                .WithParameter<UIDocument>(_sceneObjectsContainer.GetUIDocument)
                .As<IUiFactory>();
            builder.Register<UiManager>(Lifetime.Singleton).AsSelf();
            builder.Register<MainScreenWidget>(Lifetime.Singleton).AsSelf();
            builder.Register<HuntScreenWidget>(Lifetime.Singleton).AsSelf();
            builder.Register<HuntMenuWidget>(Lifetime.Singleton).AsSelf();
            builder.Register<NoEnergyMessageView>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<EnergyView>(Lifetime.Singleton).As<IHuntUiInitializable>();
            builder.Register<HealthView>(Lifetime.Singleton).As<IHuntUiInitializable>();
            builder.Register<SatietyView>(Lifetime.Singleton).As<IHuntUiInitializable>();
            builder.Register<LevelTimerView>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<ScoreView>(Lifetime.Singleton).As<IHuntUiInitializable>();
            builder.Register<HuntResultWidget>(Lifetime.Singleton).AsSelf();
            builder.Register<EndHuntMessageView>(Lifetime.Singleton).As<IHuntUiInitializable>();
            builder.Register<LevelsMapWidget>(Lifetime.Singleton).AsSelf();
            builder.Register<UiLevelsMapController>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<SettingsPanelWidget>(Lifetime.Singleton).AsSelf();
            builder.Register<StatisticPanelWidget>(Lifetime.Singleton).AsSelf();
            builder.Register<PointerUiChecker>(Lifetime.Singleton).As<IPointerUiChecker>()
                .WithParameter<UIDocument>(_sceneObjectsContainer.GetUIDocument);
            // ---------

            builder.Register<SceneController>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<SceneGeometry>(Lifetime.Singleton).AsSelf().As<ISceneGeometry>();
            builder.Register<AreaChecker>(Lifetime.Singleton).AsImplementedInterfaces();

            //builder.Register<VictoryPossibilityStump>(Lifetime.Singleton).As<IVictoryPossibilityHolder>();
            //builder.Register<LevelLoaderStump>(Lifetime.Singleton).As<ICurrentLevelDescriptorHolder>();

            //  -----  Character system  -----
            builder.Register<CharacterManager>(Lifetime.Singleton).AsImplementedInterfaces();
            
                // ----- ----- IBodyUser -----
            builder.Register<WalkController>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<JumpController>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<AnimationController>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<JumpPainter>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<CharHorizontalDirection>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<FlightController>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<PlayerHealth>(Lifetime.Singleton).As<IPlayerHealth, IBodyUser, IHealthObservable, IPlayerDamaged>();
            builder.Register<AttackController>(Lifetime.Singleton).AsImplementedInterfaces();
                // ----- -----


            builder.Register<CharStateHolder>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            builder.Register<JumpCalculator>(Lifetime.Singleton).As<IJumpCalculator>();
            builder.Register<EnergyController>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<CharacterMediator>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<ScoreController>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<PickUpController>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<SatietyController>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<PlayerDamageVisualizer>(Lifetime.Singleton).AsImplementedInterfaces();
            // -------------------------


            builder.Register<NpcManager>(Lifetime.Singleton).As<ITickable, INpcManager>();
            builder.Register<NpcSpawner>(Lifetime.Singleton).AsImplementedInterfaces();

            builder.Register<PrefabLoader>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.RegisterInstance(_dataContainer).As<IDataHolder>();

            builder.Register<TouchInput>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<TouchHandler>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<LevelTimerController>(Lifetime.Singleton).AsImplementedInterfaces();

            builder.Register<PoolFactory>(Lifetime.Singleton).As<IPoolFactory>();
            builder.Register<ObjectPoolManager>(Lifetime.Singleton).AsImplementedInterfaces();

            builder.Register<LevelProgressController>(Lifetime.Singleton).AsImplementedInterfaces();

        }
    }
}