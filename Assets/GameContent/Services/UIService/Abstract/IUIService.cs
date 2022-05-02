using UniRx;

namespace GameContent.Services.UIService.Abstract
{
    public interface IUIService
    {
        IReactiveCommand<Unit> Save { get; }
        IReactiveCommand<Unit> Load { get; }
        IReactiveCommand<Unit> Exit { get; }
    }
}