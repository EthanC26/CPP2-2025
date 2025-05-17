using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverMenu : BaseMenu
{
    public Button MainMenuBtn;
    public Button CreditsBtn;
    public Button QuitBtn;

    public override void Init(MenuController contex)
    {
        base.Init(contex);
        state = MenuStates.GameOver;

       if(MainMenuBtn) MainMenuBtn.onClick.AddListener(() => SceneManager.LoadScene("Title"));
       if(CreditsBtn) CreditsBtn.onClick.AddListener(() => SetNextMenu(MenuStates.Credits));
        QuitBtn.onClick.AddListener(contex.QuitGame);
    }
}
