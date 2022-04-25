using DG.Tweening;
using GameContent.Services.CameraControllerService.Abstract;
using GameContent.Services.MouseInput.Abstract;
using GameContent.Settings.CameraSettings;
using UniRx;
using UnityEngine;

namespace GameContent.Services.CameraControllerService
{
    public class CameraControllerService : ICameraControllerService
    {
        private readonly Camera _camera;
        private readonly GameObject _cameraPivot;

        private readonly CameraSettings _cameraSettings;
    
        public CameraControllerService(Camera camera, GameObject cameraPivot, CameraSettings cameraSettings)
        {
            _camera = camera;
            _cameraPivot = cameraPivot;
            _cameraSettings = cameraSettings;

            MessageBroker.Default.Receive<IMouseInputService>()
                .Subscribe(OnServiceReceived);
        }

        private void OnServiceReceived(IMouseInputService mouseInputService)
        {
            mouseInputService.RotateLeft
                .Subscribe(_ => RotateLeft());

            mouseInputService.RotateRight
                .Subscribe(_ => RotateRight());

            mouseInputService.ZoomCommand
                .Subscribe(OnZoom);
        }

        private void RotateLeft()
        {
            if (!_cameraSettings.canMove.Value) return;
            
            var newRotation = new Vector3(_cameraPivot.transform.eulerAngles.x,
                _cameraPivot.transform.eulerAngles.y - _cameraSettings.rotationAngle,
                _cameraPivot.transform.eulerAngles.z);

            Rotate(newRotation);
        }

        private void RotateRight()
        {
            if (!_cameraSettings.canMove.Value) return;
            
            var newRotation = new Vector3(_cameraPivot.transform.eulerAngles.x,
                _cameraPivot.transform.eulerAngles.y + _cameraSettings.rotationAngle,
                _cameraPivot.transform.eulerAngles.z);

            Rotate(newRotation);
        }

        private void Rotate(Vector3 newRotation)
        {
            DOTween.Sequence()
                .OnStart(() => _cameraSettings.canMove.Value = false)
                .Append(_cameraPivot.transform.DORotate(newRotation, _cameraSettings.rotationTime))
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