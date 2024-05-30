using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InteractiveItemObject : MonoBehaviour, IInteractable
{
    public InteractiveItemData interactiveItemData;
    public Image uiImage;

    public string GetInteractPrompt()
    {
        string str = $"{interactiveItemData.description}";
        return str;
    }

    public void OnInteract()
    {
        CharacterManager.Instance.Player.controller.SetUIImages(uiImage);
    }
}
