using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Equipment : MonoBehaviour
{
    public Equip curEquip;
    public Transform equipParent;

    private PlayerController controller;
    private PlayerCondition condition;

    void Start()
    {
        controller = CharacterManager.Instance.Player.controller;
        condition = CharacterManager.Instance.Player.condition;
    }

    public void OnAttackInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && curEquip != null && controller.canLook)
        {
            curEquip.OnAttackInput();
        }
    }

    public void EquipNew(ItemData data)
    {
        UnEquip();
        curEquip = Instantiate(data.equipPrefab, equipParent).GetComponent<Equip>();
        curEquip.data = data; // ��� ������ ����
        if (curEquip is EquipTool equipTool)
        {
            equipTool.ApplyAbilities(data.ability);
        }
    }

    public void UnEquip()
    {
        if (curEquip != null)
        {
            // ���� ���� �� �̵� �ӵ��� ������� �ǵ���
            if (curEquip is EquipTool equipTool)
            {
                foreach (var ability in equipTool.data.ability) // curEquip.data ����
                {
                    if (ability.type == EquipAbilityType.Speed)
                    {
                        controller.ModifyMoveSpeed(-ability.value);
                    }
                }
            }

            // ��� �����͸� null�� �����Ͽ� ��� ������ ��Ÿ��
            curEquip.data = null;
            controller.currentMoveSpeed = controller.moveSpeed;

            Destroy(curEquip.gameObject);
            curEquip = null;
        }
    }
}