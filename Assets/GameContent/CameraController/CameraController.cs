using GameContent.Services.MouseInput.Abstract;
using GameContent.Settings.CameraSettings;
using GameContent.Settings.MouseInputSettings;
using UniRx;
using UnityEngine;

namespace GameContent.CameraController
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField]
        private Camera _camera;

        [SerializeField]
        private CameraSettings _cameraSettings;
    
        private void Awake()
        {
            MessageBroker.Default.Receive<IMouseInputService>()
                .Subscribe(OnServiceReceived).AddTo(this);
        }

        private void OnServiceReceived(IMouseInputService mouseInputService)
        {
            mouseInputService.RotateLeft
                .Subscribe(_ => RotateLeft())
                .AddTo(this);
            
            mouseInputService.RotateRight
                .Subscribe(_ => RotateRight())
                .AddTo(this);
            
            mouseInputService.ZoomCommand
                .Subscribe(OnZoom)
                .AddTo(this);
        }

        private void RotateLeft()
        {
            Debug.Log("rotate left");
        }

        private void RotateRight()
        {
            Debug.Log("rotate right");
        }

        private void OnZoom(float value)
        {
            _camera.orthographicSize += value;
            
            if (_camera.orthographicSize > _cameraSettings.minZoom)
            {
                _camera.orthographicSize = _cameraSettings.minZoom;
            }
            if (_camera.orthographicSize < _cameraSettings.maxZoom)
            {
                _camera.orthographicSize = _cameraSettings.maxZoom;
            }
        }
    }
}