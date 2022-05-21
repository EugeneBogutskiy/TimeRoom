using UnityEngine;

namespace GameContent.Entities.ExperimentalScene
{
    public class TableInteractable : InteractableObject
    {
        public override void Interact()
        {
            transform.Rotate(Vector3.up, 15);
        }
    }
}