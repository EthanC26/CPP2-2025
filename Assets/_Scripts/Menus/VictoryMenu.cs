using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VictoryMenu : BaseMenu
{
    public Button CreditsBtn;
    public Button MainMenuBtn;
    public Button QuitBtn;

    public override void Init(MenuController contex)
    {
        base.Init(contex);
        state = MenuStates.Victory;

        if (MainMenuBtn) MainMenuBtn.onClick.AddListener(() => SceneManager.LoadScene("Title"));
        if (CreditsBtn) CreditsBtn.onClick.AddListener(() => SetNextMenu(MenuStates.Credits));
        QuitBtn.onClick.AddListener(contex.QuitGame);
    }
}
