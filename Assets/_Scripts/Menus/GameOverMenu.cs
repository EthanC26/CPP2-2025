using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverMenu : BaseMenu
{
    public Button MainMenuBtn;
    public Button QuitBtn;
    public TMP_Text GameoverText;

    public override void Init(MenuController contex)
    {
        base.Init(contex);
        state = MenuStates.GameOver;

       if(MainMenuBtn) MainMenuBtn.onClick.AddListener(() => SceneManager.LoadScene("Title"));
        QuitBtn.onClick.AddListener(contex.QuitGame);
    }
}
