using System.Collections.Generic;
using UnityEngine;
using VContainer.Unity;

using Dragoraptor.Character;
using Dragoraptor.Core;
using Dragoraptor.Interfaces;
using Dragoraptor.Services;


namespace Dragoraptor
{
    public class GameStarter : IStartable
    {

        private GameStateManager _gameStateManager;
        //private CharStateHolder _stateHolder;
        //private IReadOnlyList<ICharStateListener> _listeners;

         public GameStarter(GameStateManager gsm//, 
             //IReadOnlyList<ICharStateListener> list,
             //CharStateHolder csh
             )
         {
             _gameStateManager = gsm;
             //_stateHolder = csh;
             //_listeners = list;
         }
         
         
         public void Start()
         {
             Debug.Log("GameStarter->Start: ");
             CameraFitter.FitCamera();
             //_stateHolder.SetStateListeners(_listeners);
             _gameStateManager.StartProgram();
         }
         
     }
}


