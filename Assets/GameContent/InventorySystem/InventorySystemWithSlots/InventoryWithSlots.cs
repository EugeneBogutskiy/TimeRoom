using System.Collections.Generic;
using System.Linq;
using GameContent.InventorySystem.Abstract;
using UnityEngine;

namespace GameContent.InventorySystem
{
    public class InventoryWithSlots : IInventory
    {
        private List<IInventorySlot> _slots;

        public int Capacity { get; set; }
        public bool IsFull => _slots.All(x => x.IsFull);

        public InventoryWithSlots(int capacity)
        {
            Capacity = capacity;

            _slots = new List<IInventorySlot>(capacity);
            
            for (var i = 0; i < capacity; i++)
            {
                _slots.Add(new InventorySlot());
            }
        }
        
        public InventoryItem GetItem(InventoryType inventoryType)
        {
            return _slots.Where(x => x.Item.inventoryType == inventoryType)
                .Select(x => x.Item)
                .FirstOrDefault();
        }

        public InventoryItem[] GetAllItems(InventoryType inventoryType)
        {
            return _slots.Where(x => x.Item.inventoryType == inventoryType)
                .Select(x => x.Item)
                .ToArray();
        }

        public InventoryItem[] GetAllItems()
        {
            return _slots.Select(x => x.Item).ToArray();
        }

        public InventoryItem[] GetEquippedItems()
        {
            return _slots.Select(x => x.Item)
                .Where(x => x.isEquipped == true)
                .ToArray();
        }

        public int GetItemAmount(InventoryType inventoryType)
        {
            return _slots.Count(x => x.Item.inventoryType == inventoryType);
        }

        public bool TryAdd(object sender, InventoryItem item)
        {
            var notEmptySlotWithSameItem = _slots.FirstOrDefault(x => !x.IsEmpty && x.Item == item);

            if (notEmptySlotWithSameItem != null)
            {
                return TryAddToSlot(sender, notEmptySlotWithSameItem, item);
            }

            var emptySlot = _slots.FirstOrDefault(x => x.IsEmpty);

            if (emptySlot != null)
            {
                return TryAddToSlot(sender, emptySlot, item);
            }
            
            Debug.Log($"Can't add item {item.inventoryType}, amount: {item.amount}, there is not place for that");

            return false;
        }

        public void Remove(object sender, InventoryType inventoryType, int amount = 1)
        {
            var slotsWithItem = _slots.Where(x => !x.IsEmpty && x.Item.inventoryType == inventoryType).ToArray();

            if (slotsWithItem.Length == 0)
            {
                return;
            }

            foreach (var slot in slotsWithItem)
            {
                slot.Item.amount -= amount;
                if (slot.Item.amount <= 0)
                {
                    slot.Clear();
                    return;
                }
            }
        }

        public bool HasItem(InventoryType inventoryType, out InventoryItem inventoryItem)
        {
            inventoryItem = _slots.Where(x => x.Item.inventoryType == inventoryType)
                .Select(x => x.Item)
                .FirstOrDefault();

            return inventoryItem is null;
        }
        
        private bool TryAddToSlot(object sender, IInventorySlot slot, InventoryItem item)
        {
            var canAddToNotEmptySlot = slot.Amount + item.amount <= item.maxItemsInInventorySlot;

            var amountToAdd = canAddToNotEmptySlot ? item.amount : item.maxItemsInInventorySlot - slot.Amount;
            var amountLeft = item.amount - amountToAdd;

            if (slot.IsEmpty)
            {
                slot.SetItem(item);
            }
            else
            {
                slot.Item.amount += item.amount;
            }

            if (amountLeft <= 0)
            {
                return true;
            }

            item.amount = amountLeft;

            return TryAdd(sender, item);
        }
    }
}