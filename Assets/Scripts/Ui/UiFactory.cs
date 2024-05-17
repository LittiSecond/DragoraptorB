﻿using UnityEngine;
using UnityEngine.UIElements;
using Dragoraptor.Interfaces.Ui;


namespace Dragoraptor.Ui
{
    public class UiFactory : IUiFactory
    {
        private const string MAIN_SCREEN_PATH = "MainScreen"; 
        private const string HUNT_SCREEN_PATH = "HuntScreen"; 
        private const string HUNT_MENU_PATH = "HuntMenu"; 
        private const string HUNT_RESULT_WINDOW_PATH = "HuntResultScreen";
        private const string HUNT_RESULT_STYLE_NAME = "hunt-result-root";

        private UIDocument _uiDocument;

        private VisualElement _mainScreenRoot;
        private VisualElement _huntScreenRoot;
        private VisualElement _huntMenuRoot;
        private VisualElement _huntResultWindow;


        public UiFactory(UIDocument uiDocument)
        {
            _uiDocument = uiDocument;
        }
        

        public VisualElement GetMainScreen()
        {
            if (_mainScreenRoot == null)
            {
                VisualTreeAsset prefab = Resources.Load<VisualTreeAsset>(MAIN_SCREEN_PATH);
                _mainScreenRoot = prefab.Instantiate();
                _uiDocument.rootVisualElement.Add(_mainScreenRoot);
            }
            
            return _mainScreenRoot;
        }

        public VisualElement GetHuntScreen()
        {
            if (_huntScreenRoot == null)
            {
                VisualTreeAsset prefab = Resources.Load<VisualTreeAsset>(HUNT_SCREEN_PATH);
                _huntScreenRoot = prefab.Instantiate();
                _uiDocument.rootVisualElement.Add(_huntScreenRoot);
            }

            return _huntScreenRoot;
        }
        
        public VisualElement GetHuntMenu()
        {
            if (_huntMenuRoot == null)
            {
                VisualTreeAsset prefab = Resources.Load<VisualTreeAsset>(HUNT_MENU_PATH);
                _huntMenuRoot = prefab.Instantiate();
                _uiDocument.rootVisualElement.Add(_huntMenuRoot);
            }

            return _huntMenuRoot;
        }

        public VisualElement GetHuntResultWindow()
        {
            if (_huntResultWindow == null)
            {
                VisualTreeAsset prefab = Resources.Load<VisualTreeAsset>(HUNT_RESULT_WINDOW_PATH);
                _huntResultWindow = prefab.Instantiate();
                _huntResultWindow.AddToClassList(HUNT_RESULT_STYLE_NAME);
                _uiDocument.rootVisualElement.Add(_huntResultWindow);
            }
            
            return _huntResultWindow;
        }
        
    }
}