using System.Collections.Generic;
using UnityEngine;
using VContainer.Unity;

using Dragoraptor.Character;
using Dragoraptor.Core;
using Dragoraptor.Interfaces.Character;


namespace Dragoraptor
{
    public class GameStarter : IStartable
    {

        private GameStateManager _gameStateManager;
        private CharStateHolder _stateHolder;
        private IReadOnlyList<ICharStateListener> _listeners;
        private SceneGeometry _sceneGeometry;

        public GameStarter(GameStateManager gsm, 
            IReadOnlyList<ICharStateListener> list,
            CharStateHolder csh,
            SceneGeometry sg
            )
        {
            _gameStateManager = gsm;
            _stateHolder = csh;
            _listeners = list;
            _sceneGeometry = sg;
        }
         
         
        public void Start()
        {
            Debug.Log("GameStarter->Start: ");
            CameraFitter.FitCamera();
            _sceneGeometry.Initialize();
            _stateHolder.SetStateListeners(_listeners);
            _gameStateManager.StartProgram();
        }
         
     }
}


