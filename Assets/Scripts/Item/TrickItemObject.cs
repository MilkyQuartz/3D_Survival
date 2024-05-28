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
                Vector3 jumpDirection = transform.up; // �������� �� �������� �÷��̾ ƨ��� ���� ����
                float jumpForce = trickItemData.jumpForce; // �����뿡�� ������ ���� ũ��

                Debug.Log("������� �浹");
                playerRigidbody.AddForce(jumpDirection * jumpForce, ForceMode.Impulse);
            }
        }
    }
    public void OnInteract()
    {

    }


}