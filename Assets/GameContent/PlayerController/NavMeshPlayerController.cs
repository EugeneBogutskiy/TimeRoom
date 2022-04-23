using GameContent.PlayerController.Abstract;
using GameContent.Services.MouseInput.Abstract;
using UniRx;
using UnityEngine;
using UnityEngine.AI;

namespace GameContent.PlayerController
{
    public class NavMeshPlayerController : MonoBehaviour, IPlayerController
    {
        private NavMeshAgent _agent;
        
        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();

            MessageBroker.Default.Receive<IMouseInputService>()
                .Subscribe(OnServiceReceived)
                .AddTo(this);
        }

        private void OnServiceReceived(IMouseInputService mouseInputService)
        {
            mouseInputService.ClickedPosition.Subscribe(OnClickedPositionReceived).AddTo(this);
        }

        private void OnClickedPositionReceived(Vector3 destination)
        {
            _agent.destination = destination;
        }
    }
}