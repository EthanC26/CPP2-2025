using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : BaseMenu
{
    public Button quitBtn;
    public Button resumeBtn;
    public Button mainMenuBtn;

    public override void Init(MenuController context)
    {
        base.Init(context);
        state = MenuStates.Pause;

        quitBtn.onClick.AddListener(context.QuitGame);
        resumeBtn.onClick.AddListener(context.JumpBack);
        mainMenuBtn.onClick.AddListener(() => UnityEngine.SceneManagement.SceneManager.LoadScene("TitleScreen"));
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
