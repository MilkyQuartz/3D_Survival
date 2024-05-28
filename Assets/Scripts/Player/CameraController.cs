using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public Transform playerTransform;
    public GameObject firstPersonCameraContainer;
    public GameObject thirdPersonCameraContainer;

    [Header("Camera Settings")]
    public Vector3 firstPersonOffset; // 1인칭 시점일 때 카메라의 상대적 위치
    public Vector3 thirdPersonOffset = new Vector3(0f, 2f, -2f); // 3인칭 시점일 때 카메라의 상대적 위치
    public Vector3 thirdPersonRotation = new Vector3(20f, 0f, 0f); // 3인칭 시점일 때 카메라의 회전

    void Start()
    {
        // 플레이어 캐릭터와 카메라 연결
        if (playerTransform == null)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }

        // 처음에는 1인칭 시점을 활성화
        ActivateFirstPersonView();
    }

    void LateUpdate()
    {
        // 카메라 위치 및 회전 업데이트
        if (thirdPersonCameraContainer.activeSelf) // 3인칭 시점일 때
        {
            // 카메라 컨테이너 위치 및 회전 설정
            thirdPersonCameraContainer.transform.position = playerTransform.position + thirdPersonOffset;
            thirdPersonCameraContainer.transform.rotation = Quaternion.Euler(thirdPersonRotation);

            // 3인칭 시점
            transform.LookAt(playerTransform);
        }
        else // 1인칭 시점일 때
        {
            // 1인칭 시점
            transform.position = playerTransform.position + firstPersonOffset;
            transform.rotation = playerTransform.rotation;
        }
    }

    // 입력에 따라 시점 변경
    public void OnCameraInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            // 시점 토글
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

    // 1인칭 시점 활성화
    void ActivateFirstPersonView()
    {
        firstPersonCameraContainer.SetActive(true);
        thirdPersonCameraContainer.SetActive(false);
    }

    // 3인칭 시점 활성화
    void ActivateThirdPersonView()
    {
        firstPersonCameraContainer.SetActive(false);
        thirdPersonCameraContainer.SetActive(true);
    }
}
