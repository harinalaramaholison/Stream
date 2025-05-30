using System.Collections;
using UnityEngine;

public class Playback_360 : MonoBehaviour
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

    public string m_URL = "video=HP HD Camera";
    public bool m_audio = true;
    public bool m_video = true;
    public int m_width = DEFAULT_TEXTURE_FORMAT.width;
    public int m_height = DEFAULT_TEXTURE_FORMAT.height;
    public bool m_flipX = false;
    public bool m_flipY = false;
    public bool m_emit = false;
    public string m_options = "";
    public uint m_timeout = 10000;
    public string m_format = "dshow";


    private StreamIn m_stream = null;
    private AudioSource m_audioSource = null;
    private Coroutine m_videoRenderer = null;

    void Start()
    {
        m_stream = new StreamIn();
        m_stream.SetDelay(DELAY);

        if (m_stream.Init(m_URL, m_audio, m_video, m_timeout, m_format, m_options))
        {
            if (m_audio)
            {
                AudioConvert aConvert = new AudioConvert();
                aConvert.Format = DEFAULT_AUDIO_FORMAT.name;
                aConvert.Channels = DEFAULT_AUDIO_FORMAT.channels;
                aConvert.Samplerate = DEFAULT_AUDIO_FORMAT.sampleRate;
                aConvert.Samples = DEFAULT_AUDIO_FORMAT.samples;

                if (m_stream.SetAudioClip(aConvert))
                    m_audioSource = gameObject.AddComponent<AudioSource>();
            }

            if (m_video)
            {
                Texture2D texture = new Texture2D(m_width, m_height, DEFAULT_TEXTURE_FORMAT.value, false);

                VideoConvert vConvert = new VideoConvert();
                vConvert.Format = DEFAULT_TEXTURE_FORMAT.name;
                vConvert.Width = m_width;
                vConvert.Height = m_height;

                if (m_stream.SetTexture(texture.GetNativeTexturePtr(), vConvert))
                {
                    Renderer renderer = gameObject.GetComponent<Renderer>();
                    Material material = renderer.materials[0];

                    material.mainTexture = texture;
                    material.mainTextureScale = new Vector2(m_flipX ? -1 : 1, m_flipY ? -1 : 1);

                    if (m_emit)
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

    IEnumerator AudioStart()
    {
        yield return new WaitForEndOfFrame();

        m_audioSource.Play(); //Need to start the OnAudioFilterRead() loop.
    }

    private void OnAudioFilterRead(float[] data, int channels)
    {
        m_stream.ReadAudio(ref data); //Read next frame from audio clip buffer. Error if m_player is not initialized or is invalid.
    }

    IEnumerator VideoRenderer()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();

            m_stream.RenderVideo(); //Render next frame from texture buffer. Error if m_player is not initialized or is invalid.
        }
    }

    private void OnDisable()
    {
        if(m_audioSource != null)
            m_audioSource.Stop(); //Stop audio loop first before destroying m_player.

        if (m_videoRenderer != null)
            StopCoroutine(m_videoRenderer); //Stop video loop first before destroying m_player.

        m_stream.Destroy();
    }

    public float rotationSpeed = 100.0f;  // Vitesse de rotation
    private float rotationX = 0.0f;
    private float rotationY = 0.0f;

    void Update()
    {
        // Si l'utilisateur maintient le bouton gauche de la souris enfoncé
        if (Input.GetMouseButton(0))
        {
            // Récupère les mouvements de la souris
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            // Calcul de la rotation en fonction des mouvements de la souris
            rotationX += mouseX * rotationSpeed * Time.deltaTime;
            rotationY -= mouseY * rotationSpeed * Time.deltaTime;

            // Applique la rotation à la sphère
            transform.rotation = Quaternion.Euler(rotationY, rotationX, 0);
        }
    }
}
