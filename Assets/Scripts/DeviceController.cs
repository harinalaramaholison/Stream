using UnityEngine;
using TMPro;
public class DeviceController : MonoBehaviour
{
    public TMP_Dropdown micDropdown;
    public TMP_Dropdown camDropdown;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DropMicrophones();
        DropCameras();
    }

    void DropMicrophones()
    {
        micDropdown.ClearOptions();
        var options = new System.Collections.Generic.List<string>(Microphone.devices);
        micDropdown.AddOptions(options);
    }
    void DropCameras()
    {
        camDropdown.ClearOptions();
        var camerasDevices = WebCamTexture.devices;
        var options = new System.Collections.Generic.List<string>();
        foreach (var cam in camerasDevices)
        {
            options.Add(cam.name);
        }
        camDropdown.AddOptions(options);
    }
}
