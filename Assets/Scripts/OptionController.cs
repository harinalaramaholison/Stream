using UnityEngine;

public class OptionController : MonoBehaviour
{
    [SerializeField] private GameObject buttonOn;
    [SerializeField] private GameObject buttonOff;
    private bool isOptionActive = false;
    void Start()
    {
        ToggleOption();
    }
    public void ToggleOption()
    {
        isOptionActive = !isOptionActive;
        if (buttonOn != null && buttonOff != null)
        {
            buttonOn.SetActive(isOptionActive);
            buttonOff.SetActive(!isOptionActive);
        }
    }
}
