using System.Collections.Generic;
using System.Linq;
using GameContent.Entities;
using GameContent.Services.InventoryService.Abstract;
using GameContent.Services.SaveLoadService.Abstract;
using GameContent.Services.SaveLoadService.BinarySaveLoadSystem;
using GameContent.Services.SceneService.Abstract;
using GameContent.Services.UIService.Abstract;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameContent.Services.SceneService
{
    public class SceneService : ISceneService
    {
        private ISaveLoadService _saveLoadService;
        private IInventoryService _inventoryService;
        private string _sceneId;

        private List<InteractableObject> _sceneInteractableObjects;

        public SceneService()
        {
            Initialize();
        }

        private void Initialize()
        {
            MessageBroker.Default.Receive<ISaveLoadService>().Subscribe(OnServiceReceived);
            MessageBroker.Default.Receive<IUIService>().Subscribe(OnUIServiceReceived);
            MessageBroker.Default.Receive<IInventoryService>().Subscribe(OnInventoryServiceReceived);
            
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }
        
        public void SaveScene()
        {
            var saveData = new SaveData()
            {
                SceneItems = new List<SceneItem>(),
                LevelId = _sceneId
            };

            _sceneInteractableObjects = GameObject.FindObjectsOfType<InteractableObject>().ToList();
            
            foreach (var interactableObject in _sceneInteractableObjects)
            {
                var interactableData = interactableObject.GetState();
                
                saveData.SceneItems.Add(new SceneItem()
                {
                    Id = interactableData.Id,
                    Position = interactableData.Position,
                    Rotation = interactableData.Rotation
                });
            }
            
            _saveLoadService.Save(saveData);

            SaveInventory();
        }

        public void LoadScene(string sceneId)
        {
            //стартуем экран загрузки
            //берем данные из сохранения если они есть
            //применяем данные к сцене и предметам
            //берем данные игрока
            //применяем данные и инвентарь к игроку
            //запускаем сцену
            //убираем экран загрузки
            
            SceneManager.LoadSceneAsync(sceneId);
        }

        private void OnServiceReceived(ISaveLoadService saveLoadService)
        {
            _saveLoadService = saveLoadService;
        }

        private void OnUIServiceReceived(IUIService uiService)
        {
            uiService.Save.Subscribe(_ => SaveScene());
        }

        private void OnInventoryServiceReceived(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
        {
            _sceneId = scene.name;
        }
        
        private void OnSceneUnloaded(Scene scene)
        {
        }
        
        private void SaveInventory()
        {
            //создаем данные для сохранения инвентаря игрока и его состояния
            //отправляем данные на сохранение
        }
    }
}