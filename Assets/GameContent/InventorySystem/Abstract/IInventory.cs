namespace GameContent.InventorySystem.Abstract
{
    public interface IInventory
    {
        int Capacity { get; set; }
        bool IsFull { get; }

        InventoryItem GetItem(InventoryType inventoryType);
        InventoryItem[] GetAllItems(InventoryType inventoryType);
        InventoryItem[] GetAllItems();
        InventoryItem[] GetEquippedItems();
        int GetItemAmount(InventoryType inventoryType);

        bool TryAdd(object sender, InventoryItem item);
        void Remove(object sender, InventoryType inventoryType, int amount = 1);
        bool HasItem(InventoryType inventoryType, out InventoryItem inventoryItem);
    }
}