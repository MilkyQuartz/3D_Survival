using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrickItemObject : MonoBehaviour, IInteractable
{
    public TrickItemData trickItemData;
    private bool isMovePadMoving = false;
    private bool isClimbing = false;
    private GameObject playerObject;

    private Rigidbody playerRigidbody;

    private void Start()
    {
        playerObject = CharacterManager.Instance.Player.gameObject;
        playerRigidbody = playerObject.GetComponent<Rigidbody>();
    }

    public string GetInteractPrompt()
    {
        string str = $"{trickItemData.displayName}\n{trickItemData.description}";
        return str;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerObject = collision.gameObject;
            if (playerRigidbody != null)
            {
                switch (trickItemData.trickItemType)
                {
                    case TrickItemType.JumpPad:
                        Vector3 jumpDirection = transform.up;
                        float jumpForce = trickItemData.jumpForce;

                        Debug.Log("점프대랑 충돌");
                        playerRigidbody.AddForce(jumpDirection * jumpForce, ForceMode.Impulse);
                        break;
                    case TrickItemType.MovePad:
                        Debug.Log("전망대랑 충돌");
                        StartCoroutine(MovePadRoutine());
                        playerObject.transform.parent = transform;
                        break;
                    case TrickItemType.Ladder:
                        Debug.Log("사다리 사용");
                        isClimbing = true;
                        playerRigidbody.useGravity = false;
                        break;
                    case TrickItemType.DivingJumpPad:
                        Debug.Log("다이빙대랑 충돌");
                        CharacterManager.Instance.Player.GetComponent<PlayerController>().SetCurrentTrickItem(this);
                        break;
                }
            }
        }
    }


    private void OnCollisionExit(Collision collision)
    {
        switch (trickItemData.trickItemType)
        {
            case TrickItemType.JumpPad:
                if (collision.gameObject == playerObject)
                {
                    // 부모-자식 관계를 해제
                    playerObject.transform.parent = null;
                    playerObject = null;
                }
                break;
            case TrickItemType.Ladder:
                isClimbing = false;
                playerRigidbody.useGravity = true;
                break;
        }

    }

    private void Update()
    {
        if (isClimbing && playerObject != null)
        {
            float vertical = Input.GetAxis("Vertical");
            Vector3 climbDirection = new Vector3(0, vertical * trickItemData.radderSpeed, 0);
            playerObject.transform.Translate(climbDirection * Time.deltaTime);
        }
    }


    private IEnumerator MovePadRoutine()
    {
        isMovePadMoving = true;
        float distanceMoved = 0f;

        while (distanceMoved < trickItemData.moveDistance)
        {
            float step = trickItemData.moveSpeed * Time.deltaTime;
            transform.Translate(Vector3.up * step);
            distanceMoved += step;
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        while (distanceMoved > 0f)
        {
            float step = trickItemData.moveSpeed * Time.deltaTime;
            transform.Translate(Vector3.down * step);
            distanceMoved -= step;
            yield return null;
        }

        isMovePadMoving = false;
    }

    public void OnInteract()
    {

    }
}
