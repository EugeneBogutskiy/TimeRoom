using UnityEngine;
using UnityEngine.EventSystems;

namespace GameContent.InventorySystem.UIs
{
    public class UISlot : MonoBehaviour, IDropHandler
    {
        public void OnDrop(PointerEventData eventData)
        {
            var otherTransform = eventData.pointerDrag.transform;
            otherTransform.SetParent(transform);
            otherTransform.localPosition = Vector3.zero;;
        }
    }
}