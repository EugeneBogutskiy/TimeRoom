using GameContent.Services.SaveLoadService.Abstract;

namespace GameContent.Services.SaveLoadService.SaveLoadService
{
    public class SaveLoadService : ISaveLoadSystem
    {
        private readonly ISaveLoadSystem _saveLoadSystem;
        
        public SaveLoadService(ISaveLoadSystem saveLoadSystem)
        {
            _saveLoadSystem = saveLoadSystem;
        }

        public void Save()
        {
            _saveLoadSystem.Save();
        }

        public void Load()
        {
            _saveLoadSystem.Load();
        }
    }
}