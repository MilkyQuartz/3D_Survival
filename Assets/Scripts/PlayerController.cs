using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    private Vector2 curMovementInput;

    private Rigidbody _rigdbody;

    private void Awake()
    {
        _rigdbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void FixedUpdate()
    {
        Move();
    }

    // ������ �̵��� �� �Լ�
    void Move()
    {
        // forward�� W�� S�� (�����ΰ��� �ڷΰ���), right�� A�� D�� (�¿�)
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;

        // ���Ⱚ�� �����ְ� ���� �����ִ°� 
        dir *= moveSpeed;

        // �� �κ��� �����ʿ�
        dir.y = _rigdbody.velocity.y;
        _rigdbody.velocity = dir;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        // pahse�� ���� ���� 
        if(context.phase == InputActionPhase.Performed) // Started�� Ű�� ����������(Ű�� ��������) �ѹ�, Performed�� Ű�� �����ڿ��� ���� ��� �޾ƿ�
        {
            // ReadValue�� ���� �о�ö�
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if(context.phase == InputActionPhase.Canceled) // Ű�� ��ҵ�����
        {
            // ������ �־���ϴϱ� ���Ͱ����� �ƹ��͵� ���� �ȵ�
            curMovementInput = Vector2.zero;
        }
    }
}
