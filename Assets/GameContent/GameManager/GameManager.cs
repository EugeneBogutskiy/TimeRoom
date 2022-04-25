using GameContent.Services.CameraControllerService;
using GameContent.Services.CameraControllerService.Abstract;
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
        private Camera _camera;
        [SerializeField]
        private GameObject _cameraPivot;
        [SerializeField]
        private GameObject _zoomObjectView;
    
        [SerializeField]
        private MouseInputSettings _mouseInputSettings;
        [SerializeField]
        private CameraSettings _cameraSettings;
        [SerializeField]
        private WallServiceSettings _wallServiceSettings;
    
        private void Awake()
        {
            IMouseInputService mouseInputService = new MouseInputService(_mouseInputSettings);

            ICameraControllerService cameraControllerService =
                new CameraControllerService(_camera, _cameraPivot, _cameraSettings, _zoomObjectView);
            
            IWallService wallService = new WallService(_wallServiceSettings,
                _mouseInputSettings, _cameraSettings, _camera, _cameraPivot);

            MessageBroker.Default.Publish(mouseInputService);
            MessageBroker.Default.Publish(cameraControllerService);
            MessageBroker.Default.Publish(wallService);
        }
    }
}