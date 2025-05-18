using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VictoryMenu : BaseMenu
{
    public Button MainMenuBtn;
    public Button QuitBtn;
    public TMP_Text VictoryText;

    public override void Init(MenuController contex)
    {
        base.Init(contex);
        state = MenuStates.Victory;

        if (MainMenuBtn) MainMenuBtn.onClick.AddListener(() => SceneManager.LoadScene("Title"));
        QuitBtn.onClick.AddListener(contex.QuitGame);
    }
}
