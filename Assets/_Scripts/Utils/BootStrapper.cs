using UnityEngine;
using UnityEngine.SceneManagement;
public class BootStrapper : Singleton<BootStrapper>
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]

    public static void BootstrapGame()
    {
        CheckScene("Bootstrap");
    }

    private void Start()
    {

    }

    protected override void Awake()
    {
        base.Awake();
    }

    public static void CheckScene(string sceneName)
    {
        for (int i = 0; 1 < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);

            if (scene.name == sceneName)
                return;
        }
        
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }
}
