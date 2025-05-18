using TMPro;
using UnityEngine;

public class InGame : BaseMenu
{
    public TMP_Text livesText;

    public override void Init(MenuController contex)
    {
        base.Init(contex);
        state = MenuStates.InGame;

        livesText.text = $"{GameManager.Instance.Lives}";

        GameManager.Instance.OnLifeValueChanged += LifeValueChanged; // Subscribe to the event
    }
    private void LifeValueChanged(int Value) => livesText.text = $"{Value}"; // Update the text with the new value

    private void OnDestroy() => GameManager.Instance.OnLifeValueChanged -= LifeValueChanged;

}
