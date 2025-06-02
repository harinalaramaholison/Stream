using System.Collections;
using UnityEngine;

public class Playback : MonoBehaviour
{
    private class DEFAULT_AUDIO_FORMAT
    {
        public static readonly string name = "flt"; //float. Format that Unity is using for audio.
        public static readonly int channels = (int)AudioSettings.GetConfiguration().speakerMode;
        public static readonly int sampleRate = AudioSettings.GetConfiguration().sampleRate;
        public static readonly int samples = AudioSettings.GetConfiguration().dspBufferSize;
    }

    private static class DEFAULT_TEXTURE_FORMAT
    {
        public const string name = "rgba";
        public const TextureFormat value = TextureFormat.RGBA32;
        public const int width = 1280;
        public const int height = 720;
    }

    private int DELAY = 200; //Playback delay in milliseconds. Can help in syncing audio and video. 

    public ConfigPlaybackDTO configPlaybakcDto;

    //public string m_URL = "video=HP HD Camera";
    //public bool m_audio = true;
    //public bool m_video = true;
    //public int m_width = DEFAULT_TEXTURE_FORMAT.width;
    //public int m_height = DEFAULT_TEXTURE_FORMAT.height;
    //public bool m_flipX = false;
    //public bool m_flipY = false;
    //public bool m_emit = false;
    //public string m_options = "";
    //public uint m_timeout = 10000;
    //public string m_format = "dshow";


    private StreamIn m_stream = null;
    private AudioSource m_audioSource = null;
    private Coroutine m_videoRenderer = null;

   #region UNITY MSG
   private void Awake()
   {
      WYCast.Load();
      WYCast.SetLogLevelforTrace(6);
      WYCast.EnableLogging(true);
      WYCast.InitLogVariable();
      WYCast.WriteLogToFile(true);

   }
   void Start()
    {
        m_stream = new StreamIn();
        m_stream.SetDelay(DELAY);

        if (m_stream.Init(configPlaybakcDto.m_cam_url, configPlaybakcDto.m_p_audio, configPlaybakcDto.m_p_video, configPlaybakcDto.m_p_timeout, configPlaybakcDto.m_format, configPlaybakcDto.m_p_options))
        {
            if (configPlaybakcDto.m_p_audio)
            {
                AudioConvert aConvert = new AudioConvert();
                aConvert.Format = DEFAULT_AUDIO_FORMAT.name;
                aConvert.Channels = DEFAULT_AUDIO_FORMAT.channels;
                aConvert.Samplerate = DEFAULT_AUDIO_FORMAT.sampleRate;
                aConvert.Samples = DEFAULT_AUDIO_FORMAT.samples;

                if (m_stream.SetAudioClip(aConvert))
                    m_audioSource = gameObject.AddComponent<AudioSource>();
            }

            if (configPlaybakcDto.m_p_video)
            {
                Texture2D texture = new Texture2D(configPlaybakcDto.m_p_width, configPlaybakcDto.m_p_height, DEFAULT_TEXTURE_FORMAT.value, false);

                VideoConvert vConvert = new VideoConvert();
                vConvert.Format = DEFAULT_TEXTURE_FORMAT.name;
                vConvert.Width = configPlaybakcDto.m_p_width;
                vConvert.Height = configPlaybakcDto.m_p_height;

                if (m_stream.SetTexture(texture.GetNativeTexturePtr(), vConvert))
                {
                    Renderer renderer = gameObject.GetComponent<Renderer>();
                    Material material = renderer.materials[0];

                    material.mainTexture = texture;
                    material.mainTextureScale = new Vector2(configPlaybakcDto.m_p_flipX ? -1 : 1, configPlaybakcDto.m_p_flipY ? -1 : 1);

                    if (configPlaybakcDto.m_p_emit)
                    {
                        material.EnableKeyword("_EMISSION");
                        material.SetColor("_EmissionColor", Color.white);
                        material.SetTexture("_EmissionMap", texture);
                    }
                }
            }
        }

        if (m_stream.IsAudioReady())
            StartCoroutine(AudioStart()); //Start together with video at WaitForEndOfFrame().

        if (m_stream.IsVideoReady())
            m_videoRenderer = StartCoroutine(VideoRenderer()); //Render video frame loop.
    }

    private void OnAudioFilterRead(float[] data, int channels)
    {
        m_stream.ReadAudio(ref data); //Read next frame from audio clip buffer. Error if m_player is not initialized or is invalid.
    }

    private void OnDisable()
    {
        if(m_audioSource != null)
            m_audioSource.Stop(); //Stop audio loop first before destroying m_player.

        if (m_videoRenderer != null)
            StopCoroutine(m_videoRenderer); //Stop video loop first before destroying m_player.

        m_stream.Destroy();
    }

   private void OnDestroy()
   {
      WYCast.Unload();
   }
   #endregion

   IEnumerator AudioStart()
    {
        yield return new WaitForEndOfFrame();

        m_audioSource.Play(); //Need to start the OnAudioFilterRead() loop.
    }
    IEnumerator VideoRenderer()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();

            m_stream.RenderVideo(); //Render next frame from texture buffer. Error if m_player is not initialized or is invalid.
        }
    }
}
