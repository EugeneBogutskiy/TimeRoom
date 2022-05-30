using System.Collections.Generic;
using GameContent.InventorySystem.SimpleInventorySystem.Abstract;
using UniRx;
using UnityEngine;

namespace GameContent.InventorySystem.SimpleInventorySystem
{
    public class InventorySystem : IInventorySystem
    {
        private readonly ReactiveCommand<Unit> _onUpdateInventory;
        private readonly Dictionary<InventoryItemData, InventoryItem> _inventoryItems;
        
        public List<InventoryItem> Inventory { get; private set; }

        public IReactiveCommand<Unit> OnUpdateInventory => _onUpdateInventory;

        public InventorySystem(GameObject inventoryView)
        {
            _onUpdateInventory = new ReactiveCommand<Unit>();
            
            _inventoryItems = new Dictionary<InventoryItemData, InventoryItem>();
            Inventory = new List<InventoryItem>();

            GameObject.Instantiate(inventoryView);
        }

        public void Add(InventoryItemData referenceData)
        {
            if (_inventoryItems.TryGetValue(referenceData, out var value))
            {
                value.AddToStack();
            }
            else
            {
                InventoryItem item = new InventoryItem(referenceData);
                Inventory.Add(item);
                _inventoryItems.Add(referenceData, item);
            }

            _onUpdateInventory.Execute(Unit.Default);
        }

        public void Remove(InventoryItemData referenceData)
        {
            if (_inventoryItems.TryGetValue(referenceData, out var value))
            {
                value.RemoveFromStack();

                if (value.StackSize == 0)
                {
                    Inventory.Remove(value);
                    _inventoryItems.Remove(referenceData);
                }
            }

            _onUpdateInventory.Execute(Unit.Default);
        }

        public InventoryItem GetInventoryItem(InventoryItemData referenceData)
        {
            if (_inventoryItems.TryGetValue(referenceData, out var value))
            {
                return value;
            }

            return null;
        }

        public void SetInventory(List<InventoryItem> inventoryItems)
        {
            Inventory = inventoryItems;

            _onUpdateInventory.Execute(Unit.Default);
        }
    }
}