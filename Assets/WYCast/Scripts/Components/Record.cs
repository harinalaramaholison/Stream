using UnityEngine;

public class Record : MonoBehaviour
{
    public string m_URL = "";
    public string m_destination = "";
    public bool m_audio = true;
    public bool m_video = true;
    public bool m_audioTranscode = false;
    public bool m_videoTranscode = false;
    public string m_audioCodec = "aac";
    public string m_videoCodec = "libx264";
    public bool m_audioConvert = false;
    public bool m_videoConvert = false;
    public string m_audioFormat = "fltp";
    public int m_channels = 2;
    public int m_samplerate = 44100;
    public string m_videoFormat = "yuv420p";
    public int m_width = 1920;
    public int m_height = 1080;
    
    private StreamOut m_stream;
    void Start()
    {
        m_stream = new StreamOut();

        if (m_stream.Init(m_URL, m_audio, m_video))
        {
            AudioConvert aConvert = null;

            if (m_audioConvert)
            {
                m_audioTranscode = true; //Create encoder if converting.

                aConvert = new AudioConvert();
                aConvert.Format = m_audioFormat;
                aConvert.Channels = m_channels;
                aConvert.Samplerate = m_samplerate;
            }

            VideoConvert vConvert = null;

            if (m_videoConvert)
            {
                m_videoTranscode = true; //Create encoder if converting.

                vConvert = new VideoConvert();
                vConvert.Format = m_videoFormat;
                vConvert.Width = m_width;
                vConvert.Height = m_height;
            }

            AudioTranscode aTranscode = null;
            if (m_audioTranscode)
            {
                aTranscode = new AudioTranscode();
                aTranscode.Codec = m_audioCodec;
            }

            VideoTranscode vTranscode = null;
            if (m_videoTranscode)
            {
                vTranscode = new VideoTranscode();
                vTranscode.Codec = m_videoCodec;
            }

            m_stream.SetDestination(m_destination, aTranscode, vTranscode, aConvert, vConvert);
        }
    }

    private void OnDisable()
    {
        m_stream.Destroy();
    }
}
