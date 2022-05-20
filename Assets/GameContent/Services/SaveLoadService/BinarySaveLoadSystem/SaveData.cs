using System;
using System.Collections.Generic;
using GameContent.DataSources;
using GameContent.Entities.Abstract;

namespace GameContent.Services.SaveLoadService.BinarySaveLoadSystem
{
    [Serializable]
    public class SaveData
    {
        public int LevelId;
        public SceneData SceneData;
        public List<IItemState> itemStates;
    }
}