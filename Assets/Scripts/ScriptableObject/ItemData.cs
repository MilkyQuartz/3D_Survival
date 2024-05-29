using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Resource,
    Equipable,
    Consumable,
    SpecialConsumable
}

public enum ConsumableType //ü��, ����� ȸ�� ������
{
    Hunger,
    Health
}

public enum SpecialConsumableType // ����� ������
{
    Scale,
    Speed
}
public enum EquipAbilityType // ��� �ɷ�
{
    Damage,
    Speed
}

[System.Serializable]
public class ItemDataConsumable
{
    public ConsumableType type;
    public float value;
}

[System.Serializable]
public class SpecialItemDataConsumable
{
    public SpecialConsumableType type;
    public float value;
}

[System.Serializable]
public class EquipAbility
{
    public EquipAbilityType type;
    public float value;
}

[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string displayName;
    public string description;
    public ItemType type;
    public Sprite icon;
    public GameObject dropPrefab;

    [Header("Stacking")]
    public bool canStack;
    public int maxStackAmount;

    [Header("Consumable")]
    public ItemDataConsumable[] consumables;

    [Header("Special")]
    public SpecialItemDataConsumable[] specialConsumable;

    [Header("Equip")]
    public GameObject equipPrefab;
    public EquipAbility[] ability;
}