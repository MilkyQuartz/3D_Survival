using UnityEngine;

public class UIPreference : MonoBehaviour
{
    public GameObject settingWindow;

    private PlayerController controller;
    // Start is called before the first frame update
    void Start()
    {
        controller = CharacterManager.Instance.Player.controller;

        controller.setting += Toggle;
        settingWindow.SetActive(false);
    }

    public void Toggle()
    {
        if (settingWindow.activeInHierarchy)
        {
            settingWindow.SetActive(false);
        }
        else
        {
            settingWindow.SetActive(true);
        }
    }

    public bool IsOpen()
    {
        return settingWindow.activeInHierarchy;
    }
}
