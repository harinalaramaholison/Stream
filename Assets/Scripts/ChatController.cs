using UnityEngine;

public class ChatController : MonoBehaviour
{
    [SerializeField] private GameObject chatPanel;
    private bool isPanelActive = false;
    void Start()
    {
        ToggleChatPanel();
    }
    public void ToggleChatPanel()
    {
        isPanelActive = !isPanelActive;
        if (chatPanel != null)
        {
            chatPanel.SetActive(isPanelActive);
        }
    }

}
