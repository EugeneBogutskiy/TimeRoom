using GameContent.InventorySystem.Abstract;

namespace GameContent.InventorySystem
{
    public class InventorySlot : IInventorySlot
    {
        public bool IsFull => Amount == Capacity;
        public bool IsEmpty => Item is null;
        public InventoryItem Item { get; private set; }
        public int Amount => IsEmpty ? 0 : Item.amount;
        public int Capacity { get; private set; }
        
        public void SetItem(InventoryItem item)
        {
            if (! IsEmpty)
            {
                return;
            }
            
            Item = item;
            Capacity = item.maxItemsInInventorySlot;
        }

        public void Clear()
        {
            if (IsEmpty)
            {
                return;;
            }
            
            Item.amount = 0;
            Item = null;
        }
    }
}