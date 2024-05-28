using UnityEngine;

public class UICondition : MonoBehaviour
{
    public Condition health;
    public Condition hunger;
    public Condition stamina;
    public Condition scale;

    private void Start()
    {
        CharacterManager.Instance.Player.condition.uiCondition = this;
    }
}