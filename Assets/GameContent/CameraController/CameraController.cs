using DG.Tweening;
using GameContent.Services.MouseInput.Abstract;
using GameContent.Settings.CameraSettings;
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
            if (!_cameraSettings.canMove.Value) return;
            
            var newRotation = new Vector3(transform.eulerAngles.x,
                transform.eulerAngles.y - _cameraSettings.rotationAngle,
                transform.eulerAngles.z);

            DOTween.Sequence()
                .OnStart(() => _cameraSettings.canMove.Value = false)
                .Append(transform.DORotate(newRotation, _cameraSettings.rotationTime))
                .OnComplete(() => _cameraSettings.canMove.Value = true)
                .SetEase(_cameraSettings.easeType);
        }

        private void RotateRight()
        {
            if (!_cameraSettings.canMove.Value) return;
            
            var newRotation = new Vector3(transform.eulerAngles.x,
                transform.eulerAngles.y + _cameraSettings.rotationAngle,
                transform.eulerAngles.z);
            
            DOTween.Sequence()
                .OnStart(() => _cameraSettings.canMove.Value = false)
                .Append(transform.DORotate(newRotation, _cameraSettings.rotationTime))
                .OnComplete(() => _cameraSettings.canMove.Value = true)
                .SetEase(_cameraSettings.easeType);
        }

        private void OnZoom(float value)
        {
            if (!_cameraSettings.canZoom.Value) return;
            
            var newCameraSize = _camera.orthographicSize + value;
            
            if (newCameraSize > _cameraSettings.minZoom)
            {
                newCameraSize = _cameraSettings.minZoom;
            }
            if (newCameraSize < _cameraSettings.maxZoom)
            {
                newCameraSize = _cameraSettings.maxZoom;
            }

            DOVirtual.Float(_camera.orthographicSize, newCameraSize,
                _cameraSettings.zoomTime, f => _camera.orthographicSize = f);
        }
    }
}