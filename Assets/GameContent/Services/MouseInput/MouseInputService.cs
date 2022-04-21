using GameContent.Services.MouseInput.Abstract;
using UniRx;
using UnityEngine;

namespace GameContent.Services.MouseInput
{
    public class MouseInputService : IMouseInputService
    {
        private readonly MouseInputSettings _mouseInputSettings;

        private readonly ReactiveCommand<GameObject> _clickedObject;
        private readonly ReactiveCommand<Vector3> _clickedPosition;
        private readonly ReactiveCommand<Unit> _rotateLeft;
        private readonly ReactiveCommand<Unit> _rotateRight;

        private Vector3 _startMousePosition;
        private Vector3 _endMousePosition;

        public ReactiveCommand<GameObject> ClickedObject => _clickedObject;
        public ReactiveCommand<Vector3> ClickedPosition => _clickedPosition;
        public ReactiveCommand<Unit> RotateLeft => _rotateLeft;
        public ReactiveCommand<Unit> RotateRight => _rotateRight;

        public MouseInputService(MouseInputSettings _settings)
        {
            _mouseInputSettings = _settings;

            _clickedObject = new ReactiveCommand<GameObject>();
            _clickedPosition = new ReactiveCommand<Vector3>();
            _rotateLeft = new ReactiveCommand<Unit>();
            _rotateRight = new ReactiveCommand<Unit>();

            Observable.EveryUpdate()
                .Where(_ => Input.GetMouseButtonDown(0))
                .Subscribe(_ => OnMouseButtonDownClicked());

            Observable.EveryUpdate()
                .Where(_ => Input.GetMouseButtonUp(0))
                .Subscribe(_ => OnMouseButtonUp());
        }

        private void OnMouseButtonDownClicked()
        {
            _startMousePosition = Input.mousePosition;
        }
        
        private void OnMouseButtonUp()
        {
            _endMousePosition = Input.mousePosition;

            if (Vector3.Distance(_endMousePosition, _startMousePosition) < _mouseInputSettings.minimumMouseDelta)
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hit))
                {
                    _clickedObject.Execute(hit.transform.gameObject);
                    _clickedPosition.Execute(hit.point);
                }
            }
            
            if (_endMousePosition.x - _startMousePosition.x > 0)
            {
                _rotateRight.Execute(Unit.Default);
            }
            else
            {
                _rotateLeft.Execute(Unit.Default);
            }
        }
    }
}