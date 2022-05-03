namespace GameContent.InventorySystem.Abstract
{
    public interface IInventory
    {
        int Capacity { get; set; }
        bool IsFull { get; }

        InventoryItem GetItem(InventoryType inventoryType);
    }
}