using GameContent.Services.MouseInput;
using GameContent.Services.MouseInput.Abstract;
using GameContent.Settings.MouseInputSettings;
using UniRx;
using UnityEngine;

namespace GameContent.GameManager
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject _cameraPivot;
    
        [SerializeField]
        private MouseInputSettings _mouseInputSettings;
    
        private void Awake()
        {
            IMouseInputService mouseInputService = new MouseInputService(_mouseInputSettings);

            MessageBroker.Default.Publish(mouseInputService);
        }
    }
}