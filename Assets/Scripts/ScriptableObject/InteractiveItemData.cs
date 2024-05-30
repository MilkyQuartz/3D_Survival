using UnityEngine;

public enum InteractiveItemType
{
    Button
}

[CreateAssetMenu(fileName = "InteractiveItem", menuName = "Interactive New Item")]
public class InteractiveItemData : ScriptableObject
{
    [Header("Info")]
    public InteractiveItemType trickItemType;
    public string description;

[Header("Button")]
    public GameObject buttonPrefab;
}
