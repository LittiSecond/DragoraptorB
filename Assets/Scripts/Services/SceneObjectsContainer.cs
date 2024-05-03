using UnityEngine;
using UnityEngine.UIElements;


namespace Dragoraptor.Services
{
    public class SceneObjectsContainer : MonoBehaviour
    {
        [SerializeField] private UIDocument _uiDocument;
        
        
        public UIDocument GetUIDocument => _uiDocument;
    }
}