using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : BaseMenu
{
    public Button StartBtn;
    public Button CreditsBtn;
    public Button QuitBtn;

    public override void Init(MenuController contex)
    {
        base.Init(contex);
        state = MenuStates.MainMenu;

        if (StartBtn) StartBtn.onClick.AddListener(() => SceneManager.LoadScene("Game"));
        if (CreditsBtn) CreditsBtn.onClick.AddListener(() => SetNextMenu(MenuStates.Credits));
        QuitBtn.onClick.AddListener(contex.QuitGame);
    }
}
