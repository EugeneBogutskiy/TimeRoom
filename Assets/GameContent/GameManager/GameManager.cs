using GameContent.Services.MouseInput;
using GameContent.Services.MouseInput.Abstract;
using UniRx;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _cameraPivot;
    
    [SerializeField]
    private readonly MouseInputSettings _mouseInputSettings;
    
    private void Awake()
    {
        IMouseInputService mouseInputService = new MouseInputService(_mouseInputSettings);

        MessageBroker.Default.Publish(mouseInputService);
    }
}