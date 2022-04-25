using GameContent.PlayerController.Abstract;
using GameContent.Services.CameraControllerService.Abstract;
using GameContent.Services.MouseInput.Abstract;
using UniRx;
using UnityEngine;
using UnityEngine.AI;

namespace GameContent.PlayerController
{
    public class NavMeshPlayerController : MonoBehaviour, IPlayerController
    {
        private NavMeshAgent _agent;

        private bool _canMove;
        
        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();

            MessageBroker.Default.Receive<IMouseInputService>()
                .Subscribe(OnMouseInputServiceReceived)
                .AddTo(this);

            MessageBroker.Default.Receive<ICameraControllerService>()
                .Subscribe(OnCameraControllerServiceReceived)
                .AddTo(this);
        }

        private void OnMouseInputServiceReceived(IMouseInputService mouseInputService)
        {
            mouseInputService.ClickedPosition.Subscribe(OnClickedPositionReceived).AddTo(this);
        }

        private void OnCameraControllerServiceReceived(ICameraControllerService cameraControllerService)
        {
            cameraControllerService.IsOnZoomStage.Subscribe(OnCameraObjectZoomInfoReceived).AddTo(this);
        }

        private void OnClickedPositionReceived(Vector3 destination)
        {
            if(!_canMove) return;
            
            _agent.destination = destination;
        }

        private void OnCameraObjectZoomInfoReceived(bool value)
        {
            _canMove = !_canMove;
            Debug.Log("Player received zoom object info");
            gameObject.SetActive(!value);
        }
    }
}