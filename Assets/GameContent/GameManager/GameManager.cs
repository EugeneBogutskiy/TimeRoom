using GameContent.Services.MouseInput;
using GameContent.Services.MouseInput.Abstract;
using GameContent.Services.WallService;
using GameContent.Services.WallService.Abstract;
using GameContent.Settings.CameraSettings;
using GameContent.Settings.MouseInputSettings;
using GameContent.Settings.WallServiceSettings;
using UniRx;
using UnityEngine;

namespace GameContent.GameManager
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject _cameraPivot;
        [SerializeField]
        private GameObject _camera;
    
        [SerializeField]
        private MouseInputSettings _mouseInputSettings;
        [SerializeField]
        private CameraSettings _cameraSettings;
        [SerializeField]
        private WallServiceSettings _wallServiceSettings;
    
        private void Awake()
        {
            IMouseInputService mouseInputService = new MouseInputService(_mouseInputSettings);
            IWallService wallService = new WallService(_wallServiceSettings,
                _mouseInputSettings, _cameraSettings, _camera, _cameraPivot);

            MessageBroker.Default.Publish(mouseInputService);
            MessageBroker.Default.Publish(wallService);
        }
    }
}