using UnityEngine;
using UnityEngine.UIElements;


namespace Dragoraptor.MonoBehs
{
    public class SceneObjectsContainer : MonoBehaviour
    {
        [SerializeField] private UIDocument _uiDocument;
        
        
        public UIDocument GetUIDocument => _uiDocument;
    }
}