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

    // 실제로 이동을 할 함수
    void Move()
    {
        // forward는 W값 S값 (앞으로가고 뒤로가고), right는 A값 D값 (좌우)
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;

        // 방향값을 정해주고 힘을 곱해주는거 
        dir *= moveSpeed;

        // 이 부분은 공부필요
        dir.y = _rigdbody.velocity.y;
        _rigdbody.velocity = dir;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        // pahse는 현재 상태 
        if(context.phase == InputActionPhase.Performed) // Started는 키가 시작했을때(키가 눌렸을때) 한번, Performed는 키가 눌린뒤에도 값을 계속 받아옴
        {
            // ReadValue는 값을 읽어올때
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if(context.phase == InputActionPhase.Canceled) // 키가 취소됐을때
        {
            // 가만히 있어야하니깐 벡터값에는 아무것도 들어가면 안됨
            curMovementInput = Vector2.zero;
        }
    }
}
