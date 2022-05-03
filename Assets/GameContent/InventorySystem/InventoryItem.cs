using UnityEngine;

namespace GameContent.InventorySystem
{
    [CreateAssetMenu(menuName = "Evgenoid/InventoryItem", fileName = "InventoryItem")]
    public class InventoryItem : ScriptableObject
    {
        public bool isEquipped;
        public InventoryType inventoryType;
        public int maxItemsInInventorySlot;
        public int amount;
    }

    public enum InventoryType
    {
        One,
        Two,
        Three
    }
}