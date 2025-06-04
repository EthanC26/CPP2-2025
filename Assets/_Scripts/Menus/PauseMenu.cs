using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : BaseMenu
{
    public Button quitBtn;
    public Button resumeBtn;
    public Button mainMenuBtn;
    public Button saveBtn;
    public Button loadBtn;

    public override void Init(MenuController context)
    {
        base.Init(context);
        state = MenuStates.Pause;

        quitBtn.onClick.AddListener(context.QuitGame);
        resumeBtn.onClick.AddListener(context.JumpBack);
        //saveBtn.onClick.AddListener(() =>
        //loadBtn.onClick.AddListener(() => Debug.Log("Load Game")); // Placeholder for save/load functionality
        mainMenuBtn.onClick.AddListener(() => UnityEngine.SceneManagement.SceneManager.LoadScene("Title"));
    }

    public override void EnterState()
    {
        base.EnterState();
        Time.timeScale = 0.0f;
    }

    public override void ExitState()
    {
        base.ExitState();
        Time.timeScale = 1.0f;
    }

    private void OnDestroy() => Time.timeScale = 1.0f;
}
