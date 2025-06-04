using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ContinueMenu : BaseMenu
{
    public TMP_Text continueText;
    public Button yesBtn;
    public Button noBtn;

    public override void Init(MenuController context)
    {
        base.Init(context);
        state = MenuStates.Continue;
        continueText.text = "Do you want to continue?";
        yesBtn.onClick.AddListener(context.JumpBack);
        noBtn.onClick.AddListener(context.QuitGame);
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
