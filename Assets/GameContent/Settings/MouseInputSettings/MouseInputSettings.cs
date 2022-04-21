using UniRx;
using UnityEngine;

[CreateAssetMenu(menuName = "Evgenoid/MouseInputSettings", fileName = "MouseInputSettings")]
public class MouseInputSettings : ScriptableObject
{
    [Header("Common settings")]
    public BoolReactiveProperty canMoveScene;
    public BoolReactiveProperty canMoveCharacter;

    [Space(50)]
    [Tooltip("minimum mouse delta to rotate camera")]
    public float minimumMouseDelta;
}