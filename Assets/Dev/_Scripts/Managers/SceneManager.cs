using Game.Utilities;

namespace Game.Managers
{
    public class SceneManager : StaticInstance<SceneManager>
    {
        public void ReloadScene()
        {
            var activeSceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
            UnityEngine.SceneManagement.SceneManager.LoadScene(activeSceneIndex);
        }
    }
}
