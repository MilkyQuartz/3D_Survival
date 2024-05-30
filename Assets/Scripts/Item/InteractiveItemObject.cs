using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveItemObject : MonoBehaviour, IInteractable
{
    public InteractiveItemData interactiveItemData;
    private GameObject playerObject;

    private Rigidbody playerRigidbody;


    private void Start()
    {
        playerObject = CharacterManager.Instance.Player.gameObject;
        playerRigidbody = playerObject.GetComponent<Rigidbody>();
    }

    public string GetInteractPrompt()
    {
        string str = $"{interactiveItemData.description}";
        return str;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerObject = collision.gameObject;
            if (playerRigidbody != null)
            {
                switch (interactiveItemData.trickItemType)
                {
                    case InteractiveItemType.Button:

                        Debug.Log("버튼 누름");
                        break;
                }
            }
        }
    }

    public void OnInteract()
    {

    }
}
