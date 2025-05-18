using TMPro;
using UnityEngine;

public class InGame : BaseMenu
{
    public TMP_Text livesText;
    public TMP_Text eLivesText;
    public BetterEnemy enemy;
    public override void Init(MenuController contex)
    {
        base.Init(contex);
        state = MenuStates.InGame;

        livesText.text = $"remaining lives: {GameManager.Instance.Lives}";
        eLivesText.text = $"remaining Enemy lives: {enemy.curHealth}";
        GameManager.Instance.OnLifeValueChanged += LifeValueChanged; // Subscribe to the event
       enemy.OnHealthChanged += EnemyHealthChanged; // Subscribe to the event
    }
    private void LifeValueChanged(int Value) => livesText.text = $"remaining lives: {Value}"; // Update the text with the new value
    private void EnemyHealthChanged(int Value) => eLivesText.text = $"remaining Enemy lives: {Value}"; // Update the text with the new value

    private void OnDestroy()
    {
        GameManager.Instance.OnLifeValueChanged -= LifeValueChanged;
        enemy.OnHealthChanged -= EnemyHealthChanged; // Unsubscribe from the event
    }
}
