using UnityEngine;

public class EquipTool : Equip
{
    public float useStamina;
    public float attackRate;
    private bool attacking;
    public float attackDistance;

    [Header("Resource Gathering")]
    public bool doesGatherResources;

    [Header("Combat")]
    public bool doesDealDamage;
    public int damage;

    private Animator animator;
    private Camera camera;

    private void Awake()
    {
        camera = Camera.main;
        animator = GetComponent<Animator>();
    }

    public override void OnAttackInput()
    {
        if (!attacking)
        {
            if (CharacterManager.Instance.Player.condition.UseStamina(useStamina))
            {
                attacking = true;
                animator.SetTrigger("Attack");
                Invoke("OnCanAttack", attackRate);
            }
        }
    }

    void OnCanAttack()
    {
        attacking = false;
    }

    public void OnHit()
    {
        Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, attackDistance))
        {
            if (doesGatherResources && hit.collider.TryGetComponent(out Resource resource))
            {
                resource.Gather(hit.point, hit.normal);
            }

            if (doesDealDamage && hit.collider.TryGetComponent(out IDamageable damagable))
            {
                damagable.TakePhysicalIDamage(damage);
            }
        }
    }
    public void ApplyAbilities(EquipAbility[] abilities)
    {
        foreach (var ability in abilities)
        {
            switch (ability.type)
            {
                case EquipAbilityType.Speed:
                    CharacterManager.Instance.Player.controller.ModifyMoveSpeed(ability.value);
                    break;
                case EquipAbilityType.Damage:
                    damage += (int)ability.value;
                    break;
            }
        }
    }
}