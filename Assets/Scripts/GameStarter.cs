using Dragoraptor.Core;
using Dragoraptor.Services;
using UnityEngine;
using VContainer.Unity;


namespace Dragoraptor
{
    public class GameStarter : IStartable
    {

        private GameStateManager _gameStateManager;

         public GameStarter(GameStateManager gsm)
         {
             _gameStateManager = gsm;
         }
         
         
         public void Start()
         {
             Debug.Log("GameStarter->Start: ");
             CameraFitter.FitCamera();
             _gameStateManager.StartProgram();
         }
         
     }
}


