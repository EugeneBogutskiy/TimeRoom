using UniRx;

namespace GameContent.Services.CameraControllerService.Abstract
{
    public interface ICameraControllerService
    {
        IReadOnlyReactiveProperty<bool> IsOnZoomStage { get; }
    }
}