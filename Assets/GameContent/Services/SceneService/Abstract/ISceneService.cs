namespace GameContent.Services.SceneService.Abstract
{
    public interface ISceneService
    {
        void SaveScene();
        void LoadFromSaveData();
        void LoadScene(string sceneId);
    }
}