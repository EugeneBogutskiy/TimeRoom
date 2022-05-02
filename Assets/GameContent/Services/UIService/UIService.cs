using GameContent.Services.UIService.Abstract;
using UnityEngine;

namespace GameContent.Services.UIService
{
    public class UIService : IUIService
    {
        private GameObject _gameMenuView;
        
        public UIService(GameObject gameMenuView)
        {
            _gameMenuView = gameMenuView;

            CreateView();
        }

        private void CreateView()
        {
            var view = GameObject.Instantiate(_gameMenuView);
        }
    }
}