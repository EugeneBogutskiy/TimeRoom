using System;
using System.Collections.Generic;
using GameContent.Entities;
using GameContent.InventorySystem.SimpleInventorySystem;

namespace GameContent.Services.SaveLoadService.BinarySaveLoadSystem
{
    [Serializable]
    public class SaveData
    {
        public string LevelId;
        public List<SceneItem> SceneItems;
        public List<InventoryItem> Inventory;
    }
}