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
                LevelId = _sceneId,
                Inventory = _inventoryService.InventorySystem.Inventory
            };

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
        }

        public void LoadFromSaveData()
        {
            var saveData = _saveLoadService.Load();

            foreach (var sceneItem in saveData.SceneItems)
            {
                foreach (var interactableObject in _sceneInteractableObjects)
                {
                    if (interactableObject.Id == sceneItem.Id)
                    {
                        interactableObject.SetState(new InteractableData()
                        {
                            Position = sceneItem.Position,
                            Rotation = sceneItem.Rotation
                        });
                    }
                }
            }

            _inventoryService.InventorySystem.SetInventory(saveData.Inventory);
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
            uiService.Load.Subscribe(_ => LoadFromSaveData());
        }

        private void OnInventoryServiceReceived(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
        {
            _sceneId = scene.name;
            
            _sceneInteractableObjects = new List<InteractableObject>();
            _sceneInteractableObjects = GameObject.FindObjectsOfType<InteractableObject>().ToList();

            //в самом начале игры загружаем сохраненные данные, применяем инвентарь
            //LoadFromSaveData();
        }
        
        private void OnSceneUnloaded(Scene scene)
        {
        }
    }
}