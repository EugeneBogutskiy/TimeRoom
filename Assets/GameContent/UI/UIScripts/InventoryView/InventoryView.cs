using GameContent.InventorySystem.SimpleInventorySystem;
using GameContent.InventorySystem.SimpleInventorySystem.Abstract;
using GameContent.UI.UIScripts.InventoryItemSlotView.Abstract;
using GameContent.UI.UIScripts.InventoryView.Abstract;
using UniRx;
using UnityEngine;

namespace GameContent.UI.UIScripts.InventoryView
{
    public class InventoryView : MonoBehaviour, IInventoryView
    {
        private IInventorySystem _inventorySystem;
        
        [SerializeField]
        private Transform _root;
        [SerializeField]
        private GameObject _slotPrefab;

        private void Awake()
        {
            MessageBroker.Default.Receive<IInventorySystem>()
                .Subscribe(OnInventorySystemReceived)
                .AddTo(this);
        }

        private void OnInventorySystemReceived(IInventorySystem inventorySystem)
        {
            _inventorySystem = inventorySystem;
            inventorySystem.OnUpdateInventory.Subscribe(_ => UpdateInventory()).AddTo(this);
        }

        private void UpdateInventory()
        {
            foreach (Transform slot in _root)
            {
                Destroy(slot.gameObject);
            }

            DrawInventory();
        }

        private void DrawInventory()
        {
            foreach (var item in _inventorySystem.Inventory)
            {
                AddInventorySlot(item);
            }
        }

        private void AddInventorySlot(InventoryItem item)
        {
            var itemObj = Instantiate(_slotPrefab);
            itemObj.transform.SetParent(_root, false);

            var slotItem = itemObj.GetComponent<IInventoryItemSlotView>();
            slotItem.Init(item);
        }
    }
}