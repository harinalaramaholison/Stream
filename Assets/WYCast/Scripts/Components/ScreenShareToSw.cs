using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.Rendering;

public class ScreenShareToSw : MonoBehaviour
{
    public delegate void FunctionPointer(string id);
    GCHandle m_gchNew;
    GCHandle m_gchDel;

    /// <summary>
    /// Class which contains texture informations needed
    /// </summary>
    /// <param name="m_index"> Selected texture index </param>
    /// <param name="m_id"> Selected texture ID </param>
    /// <param name="m_inboundId"> Inbound ID </param>
    /// <param name="m_flag"> Texture's flag </param>
    /// <param name="m_texture"> Selected texture as a Texture2D </param>
    private class TEXTURE_INFO
    {
        public int m_index = -1;
        public int m_id = -1;
        public int m_inboundId = -1;
        public bool m_flag = false;
        public Texture2D m_texture;

        public void ReInitialize()
        {
            m_index = -1;
            m_id = -1;
            m_inboundId = -1;
            m_flag = false;
        }
    }

    /// <summary>
    /// Default audio formats
    /// </summary>
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

    [SerializeField]
    private ConfigDTO configDto;

    [SerializeField]
    List<GameObject> screenPlanes = new List<GameObject>();

    private string m_token = "{\"token\":\"eyJhbGciOiJFQ0RILUVTK0EyNTZLVyIsImVuYyI6IkEyNTZHQ00iLCJlcGsiOnsiY3J2IjoiUC0yNTYiLCJrdHkiOiJFQyIsIngiOiJXRzlyYll2dzNNd2F3Qy1XLXN2TDNfUllUU0diZy1mcVhoRDhxdVVBSzdvIiwieSI6IkhkaWk4dlFhTWJTN0tUbENIQ21zZ08yUHhod0k1OVFjSzB3cUIyZXptMWsifSwia2lkIjoiZGdJMG9nWUdrdndOTDg0dExQQVhQWXdkSlhMYkF4allhNGVzSERXY0VPWSJ9.amDalM2T3tI2258oe33mdQSLiWL7nEc5ElaaMu-Am6V2O_yM2zdG2g.oddXgkB7qAbflUU5.8640Qo8QVWpKQ4rVnM374oaigYjck7UIw1xbwMH78HPx0q1QOObcBAnmiVDkzJuPW581OBgZGw_53WysXzSWSY7Fn6uKywaB6GGCOl4OqDJUtT-6oC9OWxdXWwUEbwWMqyvZDUIKTZq3t2CJNTbMvKP9o5pba_k_B7k0_wuIbRGBfLyNGFDj_oowfMgd3GXsZpE0RYSWwTb8Ftu83I5qon1GAoEAT2ag7w-JKyx9H3vtW6eFQfyXWC_McINU31MnNakqYRAOiDytYbrc1RvROEMsrOyZAJmYQI2ooBJGVTwBRdEC0H3fZbNWQojZyFQoiQaMPMir5Rf3_HfHdBEBDcRC3tF6Fq4hQ_T5IBgWYryJLjStEIwonq2Y2IaqGcsNLi4C0SpGxCNt3gWO-TrmsYXJbVHxKnfbI2VGlTylfMhaPgpCgHLL.5IufvnoqAgG0kJSB1ZkirQ\"}";

    private int m_streamId = -1;
    private int m_sourceId = -1;
    private int m_sessionTeam = -1;
    private int m_sessionId = -1;
    private int m_converterId = -1;
    private int m_outboundId = -1;

    //InBoundStream
    private int m_idInboundStream = -1;
    private bool m_newInBound = false;
    private bool m_delInbound = false;
    private string m_delInboundstream = null;
    private Dictionary<string, int> m_inboundlistId = new Dictionary<string, int>();
    private Dictionary<int, Coroutine> m_CoroutinesInbound = new Dictionary<int, Coroutine>();
    private Dictionary<int, int> m_converterSink = new Dictionary<int, int>();
    private Dictionary<int, TEXTURE_INFO> m_lstTexture = new Dictionary<int, TEXTURE_INFO>();

    //ShareScreen
    private long m_startTime = -1;
    private int m_bufferSourceId = -1;

    private Coroutine m_coroutine = null;
    RenderTexture m_renderTexture;
    [SerializeField]
    private int max_user = 10;

    private Coroutine m_videoRenderer = null;
    private int DELAY = 200; //Playback delay in milliseconds. Can help in syncing audio and video. 

    void Start()
    {
        // Create callbacks notification to notify users that there is a new stream or there is a stream deleted.
        CreateCallbacksNotification();

        CreateTexture(configDto.m_width, configDto.m_height, DEFAULT_TEXTURE_FORMAT.value, false);

        //Prepare and Load token
        m_token = (configDto.m_ConfigPath != null && configDto.m_ConfigPath != "") ? File.ReadAllText(configDto.m_ConfigPath) : "";

        //Token verification
        if (m_token.Equals(""))
        {
            Debug.Log("Couldn't get token");
            Logger.Log("Couldn't get token");
            return;
        }

        // Create Wycast session manager
        if (!WYCast.CreateSessionMgr(configDto.m_license, configDto.m_logLevel))
        {
            Debug.Log("Session couldn't create");
            Logger.Log("Session couldn't create");
            return;
        }

        // Create Wycast Stream container
        m_streamId = WYCast.CreateStream();

        if (m_streamId >= 0)
        {
            //Create Session to connect to Wycast
            m_sessionTeam = WYCast.GetExistingSessionTeam();

            // Connect to the room
            m_sessionId = WYCast.ConnectRoom(m_streamId, m_token);
            if (m_sessionId >= 0 && m_sessionTeam == -1)
                m_sessionTeam = m_streamId;

            // Create Source buffer
            m_bufferSourceId = WYCast.CreateSourceBuffer("rgba", Screen.width, Screen.height, WYCast.GetTimeNow(200));

            if (m_bufferSourceId >= 0)
            {
                // Add video to Source buffer
                m_sourceId = WYCast.AddVideoSourceBuffer(m_streamId, m_bufferSourceId);

                if (m_sourceId >= 0)
                {
                    // Convert Video to the right size
                    m_converterId = WYCast.AddVideoConverter(m_streamId, m_sourceId, "rgba", 1280, 720);

                    // Create OutBound Stream
                    if (m_converterId >= 0)
                    {
                        m_outboundId = WYCast.CreateOutboundStream(m_streamId, m_converterId, m_sessionTeam, m_sessionId, configDto.m_StreamName);
                    }                        

                    if (m_outboundId < 0)
                    {
                        Destroy();
                    }                        
                    else
                    {
                        // Capture screen content
                        if (!Capture())
                        {
                            return;
                        }
                            
                    }
                }
            }
        }
    }
    void Update()
    {
        //create texture
        if (m_newInBound)
        {
            m_newInBound = false; // immediatly
            StartCoroutine(CreateTextureInboundStream(m_idInboundStream));
        }

        if (m_delInbound)
        {
            m_delInbound = false;

            if (m_inboundlistId.ContainsKey(m_delInboundstream))
            {
                int id = -1;
                int idConverter = -1;

                id = m_inboundlistId[m_delInboundstream];
                //Remove Converter
                idConverter = m_converterSink[id];
                WYCast.Remove(m_streamId, idConverter);
                m_converterSink.Remove(id);

                //Remove InboundStream
                WYCast.Remove(m_streamId, id);

                //Stop coroutine first
                Coroutine cor = m_CoroutinesInbound[id];
                if (cor != null)
                {
                    StopCoroutine(cor);
                    m_CoroutinesInbound.Remove(id);
                }
            }

            DeleteTextureInboundStream();
        }
    }
    void OnDisable()
    {
        if (m_coroutine != null)
            StopCoroutine(m_coroutine);

        Destroy();
    }

    /// <summary>
    /// Prepare textures for any Planes for streams
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="format"></param>
    /// <param name="mipChain"></param>
    private void CreateTexture(int width, int height, TextureFormat format, bool mipChain)
    {
        for (int i = 0; i < max_user; i++)
        {
            m_lstTexture.Add(i, new TEXTURE_INFO { m_index = i, m_id = -1, m_inboundId = -1, m_flag = false, m_texture = new Texture2D(width, height, format, mipChain) });
        }
    }

    /// <summary>
    /// Get texture informations
    /// </summary>
    /// <returns>TEXTURE_INFO</returns>
    private TEXTURE_INFO GetFreeTexture()
    {
        TEXTURE_INFO tRet = null;

        foreach (TEXTURE_INFO tInfo in m_lstTexture.Values)
        {
            if (!tInfo.m_flag)
            {
                tRet = tInfo;
                break;
            }
        }

        return tRet;
    }

    /// <summary>
    /// Get texture informations by Inbound
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private TEXTURE_INFO GetTextureByInBound(int id)
    {
        TEXTURE_INFO tRet = null;

        foreach (TEXTURE_INFO tInfo in m_lstTexture.Values)
        {
            if (tInfo.m_inboundId == id)
            {
                tRet = tInfo;
                break;
            }
        }

        return tRet;
    }

    /// <summary>
    /// Get texture by Inbound
    /// </summary>
    /// <param name="inBound"></param>
    /// <returns>texture ID</returns>
    private int GetTextureIdByInBound(int inBound)
    {
        int itextId = -1;

        foreach (TEXTURE_INFO tInfo in m_lstTexture.Values)
        {
            if (tInfo.m_inboundId == inBound)
            {
                itextId = tInfo.m_id;
                break;
            }
        }

        return itextId;
    }

    /// <summary>
    /// Start Capture screen thread
    /// </summary>
    /// <returns>true if the thread works</returns>
    private bool Capture()
    {
        m_coroutine = StartCoroutine(CaptureScreen());
        return m_coroutine != null;
    }

    /// <summary>
    /// Print log messages
    /// </summary>
    /// <param name="msg"></param>
    static void PrinterMsg(string msg)
    {
        if (msg != null)
        { 
            Debug.Log(msg);
            Logger.Log(msg);
        }
    }

    /// <summary>
    /// Callback called when there is a new Inbound
    /// </summary>
    /// <param name="peerId"></param>
    void OnNewInboundFunc(string peerId)
    {
        int id = -1;

        if (m_sessionId >= 0 && m_streamId >= 0)
        {
            id = WYCast.AddInboundStream(m_streamId, m_sessionId, peerId);

            if (id > 0)
            {
                m_idInboundStream = id;
                m_newInBound = true;                
                m_inboundlistId[peerId] = id;
            }
        }
    }

    /// <summary>
    /// Callback called when an Inbound is deleted
    /// </summary>
    /// <param name="peerId"></param>
    void OnDeleteInBoundStream(string peerId)
    {
        m_delInboundstream = peerId;
        m_delInbound = true;
    }

    /// <summary>
    /// Delete texture Inbound
    /// </summary>
    void DeleteTextureInboundStream()
    {
        int id = -1;
        int textId = -1;
        TEXTURE_INFO tInfo = null;

        if (m_inboundlistId.ContainsKey(m_delInboundstream))
        {
            id = m_inboundlistId[m_delInboundstream];

            //Remove Texture
            textId = GetTextureIdByInBound(id);
            if (textId >= 0)
            {
                WYCast.DeleteTextureBuffer(textId);
            }

            //ReInitialize Texture of scenes
            tInfo = GetTextureByInBound(id);
            if (tInfo != null)
            {
                tInfo.ReInitialize();
            }

            m_inboundlistId.Remove(m_delInboundstream);
            m_delInboundstream = null;
        }
    }

    /// <summary>
    /// Create an Inbound stream texture
    /// </summary>
    /// <param name="a_id"></param>
    /// <returns></returns>
    IEnumerator CreateTextureInboundStream(int a_id)
    {
        yield return null;
        //sink here
        int iTextureId = -1;
        Coroutine currentCoroutine = null;

        VideoConvert vConvert = new VideoConvert();
        vConvert.Format = DEFAULT_TEXTURE_FORMAT.name;
        vConvert.Width = configDto.m_width;
        vConvert.Height = configDto.m_height;

        TEXTURE_INFO tInfo = null;

        tInfo = GetFreeTexture();

        if (tInfo != null && tInfo.m_texture != null)
        {
            IntPtr pNative = tInfo.m_texture.GetNativeTexturePtr();
            iTextureId = SetTexture(pNative, vConvert, a_id);
            tInfo.m_flag = true;
            tInfo.m_inboundId = a_id;
            tInfo.m_id = iTextureId;

            yield return null;

            if (iTextureId >= 0)
            {
                if (m_converterSink[a_id] >= 0)
                {
                    //string PlaneName = string.Empty;
                    //PlaneName = "PlanePlayback" + tInfo.m_index.ToString();

                    //GameObject pl = GameObject.Find(PlaneName);
                    Renderer renderer = screenPlanes[tInfo.m_index].GetComponent<Renderer>();
                    Material material = renderer.material;

                    material.mainTexture = tInfo.m_texture;
                    material.mainTextureScale = new Vector2(1, -1);//m_flipX ? -1 :1, m_flipY ? -1 : 1);

                    if (configDto.m_emit)
                    {
                        material.EnableKeyword("_EMISSION");
                        material.SetColor("_EmissionColor", Color.white);
                        material.SetTexture("_EmissionMap", tInfo.m_texture);
                    }

                    if (IsVideoReady(iTextureId))
                    {
                        currentCoroutine = StartCoroutine(VideoRenderer(iTextureId)); //Render video frame loop.
                        m_CoroutinesInbound[a_id] = currentCoroutine;
                    }
                }
                else
                {
                    RemoveTexture(iTextureId);
                }
                    
            }
        }

        yield return new WaitForSeconds(.5f);
        StopCoroutine("CreateTextureInboundStream");
    }

    /// <summary>
    /// Render video frame
    /// </summary>
    /// <param name="iTexture"></param>
    public void RenderVideo(int iTexture)
    {
        GL.IssuePluginEvent(WYCast.RenderNextVideoFrame(), iTexture);
    }

    /// <summary>
    /// Render next frame from texture buffer.
    /// </summary>
    /// <param name="iTexture"></param>
    /// <returns>Error if m_player is not initialized or is invalid.</returns>
    IEnumerator VideoRenderer(int iTexture)
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            RenderVideo(iTexture); //Render next frame from texture buffer. Error if m_player is not initialized or is invalid.
        }
    }

    /// <summary>
    /// Set Inbound stream texture
    /// </summary>
    /// <param name="t_texturePtr"></param>
    /// <param name="t_convert"></param>
    /// <param name="iInboundId"></param>
    /// <returns>Texture ID</returns>
    private int SetTexture(System.IntPtr t_texturePtr, VideoConvert t_convert, int iInboundId)
    {
        int textureId = -1;
        int iConverterSink = -1;

        if (m_streamId < 0 || configDto.m_video == false)
            return -1;

        if (m_startTime < 0)
            m_startTime = WYCast.GetTimeNow(DELAY);

        textureId = WYCast.CreateTextureBuffer(t_texturePtr, t_convert.Format, t_convert.Width, t_convert.Height, m_startTime);

        if (textureId < 0)
            goto TextureError;

        iConverterSink = WYCast.AddVideoConverterSink(m_streamId, iInboundId, textureId);

        if (iConverterSink < 0)
        {
            RemoveTexture(textureId);
            goto TextureError;
        }

        m_converterSink[iInboundId] = iConverterSink;

        return textureId;

    TextureError:
        return -1;
    }

    /// <summary>
    /// Delete Inbound stream texture
    /// </summary>
    /// <param name="textureId"></param>
    private void RemoveTexture(int textureId)
    {
        if (textureId >= 0)
            WYCast.DeleteTextureBuffer(textureId);
    }

    /// <summary>
    /// Check Video
    /// </summary>
    /// <param name="textureId"></param>
    /// <returns>true if video is ready</returns>
    private bool IsVideoReady(int textureId)
    {
        return configDto.m_video && textureId >= 0;
    }

    /// <summary>
    /// Create callbacks notification
    /// </summary>
    private void CreateCallbacksNotification()
    {
        FunctionPointer NewIB = new FunctionPointer(this.OnNewInboundFunc);
        FunctionPointer DelIB = new FunctionPointer(this.OnDeleteInBoundStream);

        m_gchNew = GCHandle.Alloc(NewIB);
        m_gchDel = GCHandle.Alloc(DelIB);

        IntPtr newPointer = Marshal.GetFunctionPointerForDelegate(NewIB);
        IntPtr delPointer = Marshal.GetFunctionPointerForDelegate(DelIB);

        WYCast.RegisterNewInboundStreamFunc(newPointer);
        WYCast.RegisterDelInboundStreamFunc(delPointer);
        WYCast.RegisterPrinter(PrinterMsg);
    }

    /// <summary>
    /// Capture screen for each frames
    /// </summary>
    /// <returns></returns>
    IEnumerator CaptureScreen()
    {
        yield return new WaitForEndOfFrame();

        while (true)
        {
            m_renderTexture = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.ARGB32);
            ScreenCapture.CaptureScreenshotIntoRenderTexture(m_renderTexture);
            if (m_renderTexture != null)
            {
                AsyncGPUReadback.Request(m_renderTexture, 0, TextureFormat.ARGB32, ReadbackCompleted);
            }
            yield return new WaitForEndOfFrame();

            AsyncOperation asyncUnload = Resources.UnloadUnusedAssets();

            if (asyncUnload != null)
            {
                yield return asyncUnload;
                GC.Collect();
            }

        }
    }

    /// <summary>
    /// CallBack when a screen is captured
    /// </summary>
    /// <param name="t_request"></param>
    void ReadbackCompleted(AsyncGPUReadbackRequest t_request)
    {
        DestroyImmediate(m_renderTexture);
        using (var imageBytes = t_request.GetData<byte>())
        {
            unsafe
            {
                System.IntPtr ptr = (System.IntPtr)((NativeArray<byte>)imageBytes).GetUnsafePtr();
                if (m_bufferSourceId >= 0)
                {
                    while (!WYCast.WriteBuffer(m_bufferSourceId, ptr))
                    {
                    }
                    imageBytes.Dispose();
                }
            }
        }
    }

    /// <summary>
    /// Remove all textures
    /// </summary>
    private void RemoveAllTextures()
    {
        foreach (TEXTURE_INFO t in m_lstTexture.Values)
        {
            if (t.m_id >= 0 && t.m_flag)
                WYCast.DeleteTextureBuffer(t.m_id);

            t.m_id = -1;
            t.m_flag = false;
            t.m_texture = null;
        }
    }

    /// <summary>
    /// Delete all streams
    /// </summary>
    public void Destroy()
    {
        if (m_streamId >= 0)
        {
            m_gchDel.Free();
            m_gchNew.Free();
            RemoveAllTextures();
            WYCast.DestroyStream(m_streamId);
            WYCast.DeleteSourceBuffer(m_bufferSourceId);
            WYCast.InitLogVariable();
            WYCast.DisconnectAll(m_streamId);

            m_bufferSourceId = -1;
            m_streamId = -1;
            m_sourceId = -1;
            m_converterId = -1;
            m_startTime = -1;
            m_sessionId = -1;
            m_outboundId = -1;
        }
    }
        
}

