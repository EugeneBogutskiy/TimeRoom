using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using GameContent.Services.MouseInput.Abstract;
using GameContent.Services.WallService.Abstract;
using GameContent.Settings.CameraSettings;
using GameContent.Settings.MouseInputSettings;
using GameContent.Settings.WallServiceSettings;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameContent.Services.WallService
{
    public class WallService : IWallService
    {
        private readonly WallServiceSettings _settings;
        private readonly MouseInputSettings _mouseInputSettings;
        private readonly CameraSettings _cameraSettings;

        private GameObject _camera;
        private GameObject _cameraPivot;
        private GameObject _ghostCamera;
        private float _startWallPositionY;
        
        private List<GameObject> _walls;

        public WallService(WallServiceSettings settings, MouseInputSettings mouseInputSettings,
            CameraSettings cameraSettings, GameObject camera, GameObject cameraPivot)
        {
            _settings = settings;
            _mouseInputSettings = mouseInputSettings;
            _cameraSettings = cameraSettings;
            _camera = camera;
            _cameraPivot = cameraPivot;

            _walls = new List<GameObject>();

            _ghostCamera = new GameObject("GhostCamera")
            {
                transform =
                {
                    position = _camera.transform.position,
                    rotation = _camera.transform.rotation
                }
            };

            MessageBroker.Default.Receive<IMouseInputService>().Subscribe(OnServiceReceived);

            Observable.FromEvent(
                x => SceneManager.sceneLoaded += OnSceneLoaded,
                x => SceneManager.sceneLoaded -= OnSceneLoaded)
                .Subscribe();
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            _walls = GameObject.FindGameObjectsWithTag("Wall").ToList();
            _startWallPositionY = _walls[0].transform.position.y;
        }

        private void OnServiceReceived(IMouseInputService service)
        {
            service.RotateLeft.Subscribe(_ => OnRotateLeft());
            service.RotateRight.Subscribe(_ => OnRotateRight());
        }

        private void OnRotateLeft()
        {
            if(!_mouseInputSettings.canMoveScene.Value) return;
            if (!_cameraSettings.canMove.Value) return;
            
            _ghostCamera.transform.RotateAround(_cameraPivot.transform.position, Vector3.up, - _cameraSettings.rotationAngle);

            foreach (var wall in _walls)
            {
                var dot = Vector3.Dot(_ghostCamera.transform.position.normalized,
                    new Vector3(wall.transform.position.x, 0, wall.transform.position.z).normalized);
                
                if (dot >= 0)
                {
                    wall.transform.DOMoveY(_settings.wallOffsetY, _settings.moveTime).SetEase(_settings.easeType);
                }

                if (dot < 0)
                {
                    wall.transform.DOMoveY(_startWallPositionY, _settings.moveTime).SetEase(_settings.easeType);
                }
            }
        }

        private void OnRotateRight()
        {
            if(!_mouseInputSettings.canMoveScene.Value) return;
            if (!_cameraSettings.canMove.Value) return;
            
            _ghostCamera.transform.RotateAround(_cameraPivot.transform.position, Vector3.up, _cameraSettings.rotationAngle);

            foreach (var wall in _walls)
            {
                var dot = Vector3.Dot(_ghostCamera.transform.position.normalized,
                    new Vector3(wall.transform.position.x, 0, wall.transform.position.z).normalized);
                
                if (dot >= 0)
                {
                    wall.transform.DOMoveY(_settings.wallOffsetY, _settings.moveTime).SetEase(_settings.easeType);
                }

                if (dot < 0)
                {
                    wall.transform.DOMoveY(_startWallPositionY, _settings.moveTime).SetEase(_settings.easeType);
                }
            }
        }
    }
}