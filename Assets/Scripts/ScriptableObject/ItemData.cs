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

public enum ConsumableType //체력, 배고픔 회복 아이템
{
    Hunger,
    Health
}

public enum SpecialConsumableType // 스페셜 아이템
{
    Scale,
    Speed
}
public enum EquipAbilityType // 장비별 능력
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