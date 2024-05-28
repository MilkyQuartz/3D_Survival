using UnityEngine;

public enum TrickItemType
{
    JumpPad
}

[CreateAssetMenu(fileName = "TrickItem", menuName = "Trick New Item")]
public class TrickItemData : ScriptableObject
{
    [Header("Info")]
    public string displayName;
    public string description;
    public TrickItemType trickItemType;

    [Header("JumpPad")]
    public GameObject jumpPadPrefab;
    public float jumpForce;
}
