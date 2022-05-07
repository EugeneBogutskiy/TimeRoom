using System.Collections.Generic;
using UniRx;

namespace GameContent.InventorySystem.SimpleInventorySystem.Abstract
{
    public interface IInventorySystem
    {
        void Add(InventoryItemData referenceData);
        void Remove(InventoryItemData referenceData);
        InventoryItem GetInventoryItem(InventoryItemData referenceData);
        
        List<InventoryItem> Inventory { get; }
        
        IReactiveCommand<Unit> OnUpdateInventory { get; }
    }
}