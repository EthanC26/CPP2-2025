using UnityEngine;
using UnityEngine.UI;

public class CreditsMenu : BaseMenu
{

    public Button backBtn;

    public override void Init(MenuController contex)
    {
        base.Init(contex);
        state = MenuStates.Credits;

        backBtn.onClick.AddListener(() => SetNextMenu(MenuStates.MainMenu));
    }
}
