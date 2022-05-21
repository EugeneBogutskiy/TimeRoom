using GameContent.InventorySystem.SimpleInventorySystem.Abstract;
using GameContent.Services.CameraControllerService;
using GameContent.Services.CameraControllerService.Abstract;
using GameContent.Services.InventoryService;
using GameContent.Services.InventoryService.Abstract;
using GameContent.Services.MouseInput;
using GameContent.Services.MouseInput.Abstract;
using GameContent.Services.ObjectInteractableService;
using GameContent.Services.ObjectInteractableService.Abstract;
using GameContent.Services.SaveLoadService.Abstract;
using GameContent.Services.SaveLoadService.BinarySaveLoadSystem;
using GameContent.Services.SaveLoadService.SaveLoadService;
using GameContent.Services.SceneService;
using GameContent.Services.SceneService.Abstract;
using GameContent.Services.UIService;
using GameContent.Services.UIService.Abstract;
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
        private GameObject _gameMenuView;
        [SerializeField]
        private GameObject _inventoryView;
    
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

            IUIService uiService = new UIService(_gameMenuView);

            ISaveLoadSystem saveLoadSystem = new BinarySaveLoadSystem();
            ISaveLoadService saveLoadService = new SaveLoadService(saveLoadSystem);

            IInventorySystem inventorySystem = new InventorySystem.SimpleInventorySystem.InventorySystem(_inventoryView);
            IInventoryService inventoryService = new InventoryService(inventorySystem);

            ISceneService sceneService = new SceneService();

            IObjectInteractableService interactableService = new ObjectInteractableService();

            MessageBroker.Default.Publish(mouseInputService);
            MessageBroker.Default.Publish(cameraControllerService);
            MessageBroker.Default.Publish(wallService);
            MessageBroker.Default.Publish(uiService);
            MessageBroker.Default.Publish(saveLoadService);
            MessageBroker.Default.Publish(inventoryService);
            MessageBroker.Default.Publish(sceneService);
            MessageBroker.Default.Publish(interactableService);
        }
    }
}