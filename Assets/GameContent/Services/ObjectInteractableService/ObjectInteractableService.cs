using GameContent.Entities.Abstract;
using GameContent.Services.MouseInput.Abstract;
using GameContent.Services.ObjectInteractableService.Abstract;
using UniRx;
using UnityEngine;

namespace GameContent.Services.ObjectInteractableService
{
    public class ObjectInteractableService : IObjectInteractableService
    {
        public ObjectInteractableService()
        {
            MessageBroker.Default.Receive<IMouseInputService>().Subscribe(OnServiceReceived);
        }

        private void OnServiceReceived(IMouseInputService mouseInputService)
        {
            mouseInputService.ClickedObject.Subscribe(OnObjectClicked);
        }

        private void OnObjectClicked(GameObject gameObject)
        {
            var interactable = gameObject.GetComponent<IInteractable>();
            interactable?.Interact();
        }
    }
}