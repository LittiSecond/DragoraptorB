using UnityEngine;
using UnityEngine.UIElements;
using Dragoraptor.Interfaces;


namespace Dragoraptor.Input
{
    public class PointerUiChecker : IPointerUiChecker
    {
        private readonly VisualElement _root;

        public PointerUiChecker(UIDocument uiDocument)
        {
            _root = uiDocument.rootVisualElement;
        }


        #region IPointerUiChecker

        public bool IsPointerUnderUiElement(Vector2 screenPosition)
        {
            Vector2 uiPos = new Vector2()
            {
                x = screenPosition.x / Screen.width * _root.layout.width,
                y = (Screen.height - screenPosition.y) / Screen.height * _root.layout.height
            };

            VisualElement picked = _root.panel.Pick(uiPos);

            bool isPicked = picked != null;
            
            // if (!isPicked)
            // {
            //     Debug.Log($"PointerUiChecker->IsPointerUnderUiElement: screenPosition = {screenPosition}; " +
            //               $"uiPos = {uiPos}; picked = null; " +
            //               $"Screen.width = {Screen.width} Screen.height = {Screen.height}; isPicked = {isPicked};");
            // }
            // else
            // {
            //     Debug.Log($"PointerUiChecker->IsPointerUnderUiElement: screenPosition = {screenPosition}; " +
            //               $"uiPos = {uiPos}; picked = {picked.name}; " +
            //               $"_root.layout = {_root.layout}; isPicked = {isPicked};");
            // }
            
            return isPicked;
        }
        
        #endregion
        
    }
}