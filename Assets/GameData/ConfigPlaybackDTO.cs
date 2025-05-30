using UnityEngine;

//PlayBack
[CreateAssetMenu(fileName = "ConfigPlaybackDto", menuName = "Wycast/ConfigPlayback", order = 1)]
public class ConfigPlaybackDTO : ScriptableObject
{    
    public string m_cam_url = "video=HP HD Camera";
    public bool m_p_audio = true;
    public bool m_p_video = true;
    public int m_p_width = 1280;
    public int m_p_height = 720;
    public bool m_p_flipX = false;
    public bool m_p_flipY = false;
    public bool m_p_emit = false;
    public string m_p_options = "";
    public uint m_p_timeout = 10000;
    public string m_format = "dshow";
}