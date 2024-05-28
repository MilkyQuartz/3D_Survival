using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrickItemObject : MonoBehaviour, IInteractable
{
    public TrickItemData trickItemData;
    private bool isMovePadMoving = false;
    private GameObject playerObject;

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

            Rigidbody playerRigidbody = playerObject.GetComponent<Rigidbody>();
            if (playerRigidbody != null)
            {
                if (trickItemData.trickItemType == TrickItemType.JumpPad)
                {
                    Vector3 jumpDirection = transform.up;
                    float jumpForce = trickItemData.jumpForce;

                    Debug.Log("점프대랑 충돌");
                    playerRigidbody.AddForce(jumpDirection * jumpForce, ForceMode.Impulse);
                }
                else if (trickItemData.trickItemType == TrickItemType.MovePad && !isMovePadMoving)
                {
                    Debug.Log("전망대랑 충돌");
                    StartCoroutine(MovePadRoutine());

                    // 플레이어를 발판의 자식으로 설정하여 발판 위에 올라가게 함
                    playerObject.transform.parent = transform;
                }
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == playerObject)
        {
            // 부모-자식 관계를 해제
            playerObject.transform.parent = null;
            playerObject = null;
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
