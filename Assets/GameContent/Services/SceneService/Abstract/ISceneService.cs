namespace GameContent.Services.SceneService.Abstract
{
    public interface ISceneService
    {
        void SaveScene();
        void LoadScene(string sceneId);
    }
}