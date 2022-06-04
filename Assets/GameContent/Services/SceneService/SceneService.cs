using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using GameContent.Entities;
using GameContent.InventorySystem.SimpleInventorySystem;
using GameContent.Services.InventoryService.Abstract;
using GameContent.Services.SaveLoadService.Abstract;
using GameContent.Services.SaveLoadService.BinarySaveLoadSystem;
using GameContent.Services.SceneService.Abstract;
using GameContent.Services.UIService.Abstract;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;
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
                Inventory = new Dictionary<string, int>()
            };

            // Fill SaveData with scene items
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
            
            // Fill SaveData with Inventory Items
            foreach (var inventoryItem in _inventoryService.InventorySystem.Inventory)
            {
                var itemId = inventoryItem.Data.id;
                
                saveData.Inventory.Add(itemId, inventoryItem.StackSize);
            }
            
            _saveLoadService.Save(saveData);
        }

        public async void LoadFromSaveData()
        {
            // Запускаем перезагрузку игры, загружаем данные
            
            var saveData = _saveLoadService.Load();

            // Set items in scene
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
            
            // Set inventory
            var inventoryItems = new List<InventoryItem>();

            foreach (var inventoryItem in saveData.Inventory)
            {
                var itemData = await LoadItemDataAsync(inventoryItem);
                var item = new InventoryItem(itemData);
                item.RemoveFromStack(int.MaxValue);
                item.AddToStack(inventoryItem.Value);
                inventoryItems.Add(item);

                if (_inventoryService.InventorySystem.InventoryItems.ContainsKey(itemData)) continue;

                _inventoryService.InventorySystem.AddToInventoryItems(itemData, item);
            }

            _inventoryService.InventorySystem.SetInventory(inventoryItems);
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

        private async UniTask<InventoryItemData> LoadItemDataAsync(KeyValuePair<string, int> inventoryItem)
        {
            var asset = await Addressables.LoadAssetAsync<InventoryItemData>(inventoryItem.Key);
            return asset;
        }
    }
}