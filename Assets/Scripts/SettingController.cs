using UnityEngine;

public class SettingController : MonoBehaviour
{
    [SerializeField] private GameObject settingPanel;
    private bool isPanelActive = false;
    void Start()
    {
        if (settingPanel != null)
        {
            settingPanel.SetActive(isPanelActive);
        }

    }

    public void ToggleSettingPanel()
    {
        isPanelActive = !isPanelActive;
        settingPanel.SetActive(isPanelActive);
    }
    public void CloseSettingPanel()
    {
        isPanelActive = false;
        settingPanel.SetActive(isPanelActive);
    }
}
