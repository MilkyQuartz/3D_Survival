using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public Transform playerTransform;
    public GameObject firstPersonCameraContainer;
    public GameObject thirdPersonCameraContainer;

    [Header("Camera Settings")]
    public Vector3 firstPersonOffset; // 1��Ī ������ �� ī�޶��� ����� ��ġ
    public Vector3 thirdPersonOffset = new Vector3(0f, 2f, -2f); // 3��Ī ������ �� ī�޶��� ����� ��ġ
    public Vector3 thirdPersonRotation = new Vector3(20f, 0f, 0f); // 3��Ī ������ �� ī�޶��� ȸ��

    void Start()
    {
        // �÷��̾� ĳ���Ϳ� ī�޶� ����
        if (playerTransform == null)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }

        // ó������ 1��Ī ������ Ȱ��ȭ
        ActivateFirstPersonView();
    }

    void LateUpdate()
    {
        // ī�޶� ��ġ �� ȸ�� ������Ʈ
        if (thirdPersonCameraContainer.activeSelf) // 3��Ī ������ ��
        {
            // ī�޶� �����̳� ��ġ �� ȸ�� ����
            thirdPersonCameraContainer.transform.position = playerTransform.position + thirdPersonOffset;
            thirdPersonCameraContainer.transform.rotation = Quaternion.Euler(thirdPersonRotation);

            // 3��Ī ����
            transform.LookAt(playerTransform);
        }
        else // 1��Ī ������ ��
        {
            // 1��Ī ����
            transform.position = playerTransform.position + firstPersonOffset;
            transform.rotation = playerTransform.rotation;
        }
    }

    // �Է¿� ���� ���� ����
    public void OnCameraInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            // ���� ���
            if (thirdPersonCameraContainer.activeSelf)
            {
                ActivateFirstPersonView();
            }
            else
            {
                ActivateThirdPersonView();
            }
        }
    }

    // 1��Ī ���� Ȱ��ȭ
    void ActivateFirstPersonView()
    {
        firstPersonCameraContainer.SetActive(true);
        thirdPersonCameraContainer.SetActive(false);
    }

    // 3��Ī ���� Ȱ��ȭ
    void ActivateThirdPersonView()
    {
        firstPersonCameraContainer.SetActive(false);
        thirdPersonCameraContainer.SetActive(true);
    }
}
