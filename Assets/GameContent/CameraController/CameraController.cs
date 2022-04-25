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

            Rotate(newRotation);
        }

        private void RotateRight()
        {
            if (!_cameraSettings.canMove.Value) return;
            
            var newRotation = new Vector3(transform.eulerAngles.x,
                transform.eulerAngles.y + _cameraSettings.rotationAngle,
                transform.eulerAngles.z);

            Rotate(newRotation);
        }

        private void Rotate(Vector3 newRotation)
        {
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
            newCameraSize = Mathf.Clamp(newCameraSize, _cameraSettings.maxZoom, _cameraSettings.minZoom);

            DOVirtual.Float(_camera.orthographicSize, newCameraSize,
                _cameraSettings.zoomTime, f => _camera.orthographicSize = f);
        }
    }
}