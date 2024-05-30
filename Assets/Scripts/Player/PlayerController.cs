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

    public void OnDivingJumpInput(InputAction.CallbackContext context) // ���̺��뿡�� Q ������ Ƣ��� 
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
        Vector3 startPos = transform.position; // �÷��̾� ��ġ
        Vector3 endPos = transform.position + transform.forward * 20f + Vector3.down * 10f; // ��ǥ ���� (Z �������� +20, Y �������� -7)
        float height = 5f; // �������� ����
        float duration = 1.5f; // �̵��ϴ� �� �ɸ��� �ð�
        float elapsedTime = 0f; // ��� �ð�

        while (elapsedTime < duration)
        {
            // ������ � ���
            Vector3 targetPos = Parabola(startPos, endPos, height, elapsedTime / duration);
            // �̵�
            transform.position = targetPos;

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
    // ������ ��� ����ϴ� �Լ�
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
            // Ű�� ������ �� ���� ����
            isObjectAActive = !isObjectAActive;

            // ���¿� ���� ������Ʈ Ȱ��ȭ/��Ȱ��ȭ
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