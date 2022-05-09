using GameContent.Services.SaveLoadService.BinarySaveLoadSystem;

namespace GameContent.Services.SaveLoadService.Abstract
{
    public interface ISaveLoadSystem
    {
        void Save(SaveData data);
        SaveData Load();
    }
}