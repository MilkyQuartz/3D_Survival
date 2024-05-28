using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrickItemObject : MonoBehaviour, IInteractable
{
    public TrickItemData trickItemData;

    public string GetInteractPrompt()
    {
        string str = $"{trickItemData.displayName}\n{trickItemData.description}";
        return str;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && trickItemData.trickItemType == TrickItemType.JumpPad)
        {
            Rigidbody playerRigidbody = other.GetComponent<Rigidbody>();
            if (playerRigidbody != null)
            {
                Vector3 jumpDirection = transform.up; // 점프대의 위 방향으로 플레이어를 튕기기 위한 벡터
                float jumpForce = trickItemData.jumpForce; // 점프대에서 가해질 힘의 크기

                Debug.Log("점프대랑 충돌");
                playerRigidbody.AddForce(jumpDirection * jumpForce, ForceMode.Impulse);
            }
        }
    }
    public void OnInteract()
    {

    }


}