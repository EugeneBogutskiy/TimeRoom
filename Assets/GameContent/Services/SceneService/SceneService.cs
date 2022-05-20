using GameContent.Services.SaveLoadService.Abstract;
using GameContent.Services.SceneService.Abstract;
using UniRx;
using UnityEngine.SceneManagement;

namespace GameContent.Services.SceneService
{
    public class SceneService : ISceneService
    {
        private ISaveLoadService _saveLoadService;
        
        public SceneService()
        {
            Initialize();
        }

        private void Initialize()
        {
            MessageBroker.Default.Receive<ISaveLoadService>().Subscribe(OnServiceReceived);
            
            SceneManager.sceneLoaded += OnsceneLoaded;
            SceneManager.sceneUnloaded += OnsceneUnloaded;
        }

        private void OnServiceReceived(ISaveLoadService saveLoadService)
        {
            _saveLoadService = saveLoadService;
        }

        private void OnsceneLoaded(Scene scene, LoadSceneMode loadMode)
        {
            
        }
        
        private void OnsceneUnloaded(Scene scene)
        {
        }
    }
}