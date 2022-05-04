namespace GameContent.InventorySystem.Abstract
{
    public interface IInventorySlot
    {
        bool IsFull { get; }
        bool IsEmpty { get; }
        
        InventoryItem Item { get; }
        
        int Amount { get; }
        int Capacity { get; }

        void SetItem(InventoryItem item);
        void Clear();
    }
}