using UnityEngine;

namespace GameContent.InventorySystem
{
    [CreateAssetMenu(menuName = "Evgenoid/InventoryItem", fileName = "InventoryItem")]
    public class InventoryItem : ScriptableObject
    {
        [SerializeField]
        private string _id;
        [SerializeField]
        private Sprite _icon;

        public string Id => _id;
        public Sprite Icon => _icon;
        
        public InventoryType inventoryType;
        public bool isEquipped;
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