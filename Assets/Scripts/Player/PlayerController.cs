using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float currentMoveSpeed;
    private Vector2 curMovementInput;
    public float jumptForce;
    public LayerMask groundLayerMask;

    [Header("Look")]
    public Transform cameraContainer;
    public float minXLook;
    public float maxXLook;
    private float camCurXRot;
    public float lookSensitivity;

    private Vector2 mouseDelta;

    [HideInInspector]
    public bool canLook = true;

    public Action inventory;

    protected Rigidbody rigidbody;
    public GameObject objectA;
    public GameObject objectB;
    private bool isObjectAActive = true;
    private TrickItemObject currentTrickItem;
    private bool isUIVisible = false;
    private Image uiImage; 

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        currentMoveSpeed = moveSpeed;
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void ModifyMoveSpeed(float value)
    {
        currentMoveSpeed = moveSpeed + value;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        if (canLook)
        {
            CameraLook();
        }
    }

    public void OnLookInput(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
        }
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGrounded())
        {
            rigidbody.AddForce(Vector2.up * jumptForce, ForceMode.Impulse);
        }
    }

    public void OnDivingJumpInput(InputAction.CallbackContext context) // 다이빙대에서 Q 누르면 튀어나감 
    {
        if (context.phase == InputActionPhase.Started)
        {
            if (currentTrickItem != null && currentTrickItem.trickItemData.trickItemType == TrickItemType.DivingJumpPad)
            {
                StartCoroutine(DiveToTarget());
            }
        }
    }

    private IEnumerator DiveToTarget()
    {
        Vector3 startPos = transform.position; // 플레이어 위치
        Vector3 endPos = transform.position + transform.forward * 20f + Vector3.down * 10f; // 목표 지점 (Z 방향으로 +20, Y 방향으로 -7)
        float height = 5f; // 포물선의 높이
        float duration = 1.5f; // 이동하는 데 걸리는 시간
        float elapsedTime = 0f; // 경과 시간

        while (elapsedTime < duration)
        {
            // 포물선 운동 계산
            Vector3 targetPos = Parabola(startPos, endPos, height, elapsedTime / duration);
            // 이동
            transform.position = targetPos;

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
    // 포물선 운동을 계산하는 함수
    protected static Vector3 Parabola(Vector3 start, Vector3 end, float height, float t)
    {
        Func<float, float> f = x => -4 * height * x * x + 4 * height * x;
        Vector3 mid = Vector3.Lerp(start, end, t);
        return new Vector3(mid.x, f(t) + Mathf.Lerp(start.y, end.y, t), mid.z);
    }

    public void SetCurrentTrickItem(TrickItemObject item)
    {
        currentTrickItem = item;
    }

    public void SetUIImages(Image image)
    {
        this.uiImage = image;
    }

    public void OnInteractiveItem(InputAction.CallbackContext context)
    {
        if (uiImage != null)
        {
            isUIVisible = !isUIVisible;
            uiImage.gameObject.SetActive(isUIVisible);
        }
    }


    private void Move()
    {
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= currentMoveSpeed; 
        dir.y = rigidbody.velocity.y;

        rigidbody.velocity = dir;
    }

    void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    bool IsGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) +(transform.up * 0.01f), Vector3.down)
        };

        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.1f, groundLayerMask))
            {
                return true;
            }
        }

        return false;
    }

    public void OnInventory(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            inventory?.Invoke();
            ToggleCursor();
        }
    }

    public void OnChangedCamera(InputAction.CallbackContext context)
    {
        if (context.action.triggered && context.control.device is Keyboard && context.control.name == "c")
        {
            // 키가 눌렸을 때 상태 변경
            isObjectAActive = !isObjectAActive;

            // 상태에 따라 오브젝트 활성화/비활성화
            objectA.SetActive(isObjectAActive);
            objectB.SetActive(!isObjectAActive);
        }
    }

    void ToggleCursor()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked;
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
    }
}