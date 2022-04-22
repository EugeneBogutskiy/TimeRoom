using DG.Tweening;
using UniRx;
using UnityEngine;

namespace GameContent.Settings.CameraSettings
{
    [CreateAssetMenu(menuName = "Evgenoid/CameraSettings", fileName = "CameraSettings")]
    public class CameraSettings : ScriptableObject
    {
        public BoolReactiveProperty canMove;
        public BoolReactiveProperty canZoom;
        public float minZoom;
        public float maxZoom;
        public float rotationTime;
        public float zoomTime;
        public int rotationAngle;

        [Header("Rotation type")]
        public Ease easeType;

        private void OnEnable()
        {
            canMove.Value = true;
            canZoom.Value = true;
        }
    }
}