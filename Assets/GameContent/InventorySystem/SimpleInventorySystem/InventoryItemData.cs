using UnityEngine;

namespace GameContent.InventorySystem.SimpleInventorySystem
{
    [CreateAssetMenu(menuName = "Evgenoid/InventoryItemData", fileName = "InventoryItem")]
    public class InventoryItemData : ScriptableObject
    {
        public string id;
        public string displayName;
        public Sprite icon;
        public GameObject prefab;
    }
}