using GameContent.Services.UIService.Abstract;
using GameContent.UI.UIScripts.GameMenuView.Abstract;
using UnityEngine;
using UniRx;

namespace GameContent.Services.UIService
{
    public class UIService : IUIService
    {
        private readonly GameObject _gameMenuView;
        
        private readonly ReactiveCommand<Unit> _saveCommand = new ReactiveCommand<Unit>();
        private readonly ReactiveCommand<Unit> _loadCommand = new ReactiveCommand<Unit>();
        private readonly ReactiveCommand<Unit> _exitCommand = new ReactiveCommand<Unit>();

        public IReactiveCommand<Unit> Save => _saveCommand;
        public IReactiveCommand<Unit> Load => _loadCommand;
        public IReactiveCommand<Unit> Exit => _exitCommand;
        
        public UIService(GameObject gameMenuView)
        {
            _gameMenuView = gameMenuView;

            CreateView();
        }

        private void CreateView()
        {
            var view = GameObject.Instantiate(_gameMenuView);
            var uiView = view.GetComponent<IGameMenuView>();

            uiView.Save.Subscribe(_ => _saveCommand.Execute(Unit.Default));
            uiView.Load.Subscribe(_ => _loadCommand.Execute(Unit.Default));
            uiView.Exit.Subscribe(_ => _exitCommand.Execute(Unit.Default));
        }
    }
}