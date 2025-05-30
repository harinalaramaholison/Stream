using System;
using System.Collections;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Rendering;

public class ScreenShare : MonoBehaviour
{
    private int m_bufferSourceId = -1;
    private int m_streamId = -1;
    private int m_sourceId = -1;
    private int m_converterId = -1;
    private int m_encoderId = -1;
    private Coroutine m_coroutine = null;
    RenderTexture m_renderTexture;
    NativeArray<byte> m_textureData;
    public string m_ConversionFormat = "yuv420p";
    public string m_Codec = "libx264";
    public string m_Destination = string.Empty;

    private bool Capture()
    {
        m_coroutine = StartCoroutine(CaptureScreen());
        return m_coroutine != null;
    }

    private void Start()
    {
        //m_textureData = new NativeArray<byte>(Screen.width * Screen.height * 4, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
        //m_renderTexture = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.ARGB32);

        m_streamId = WYCast.CreateStream();

        if(m_streamId >= 0 )
        {
            m_bufferSourceId = WYCast.CreateSourceBuffer("rgba", Screen.width, Screen.height, WYCast.GetTimeNow(200));

            if (m_bufferSourceId >= 0)
            {
                m_sourceId = WYCast.AddVideoSourceBuffer(m_streamId, m_bufferSourceId);

                if(m_sourceId >= 0 )
                {
                    m_converterId = WYCast.AddVideoConverter(m_streamId, m_sourceId, m_ConversionFormat, Screen.width, Screen.height );

                    if(m_converterId >= 0 )
                    {
                        m_encoderId = WYCast.AddVideoEncoder(m_streamId, m_converterId, m_Codec);

                        if(m_encoderId >= 0 )
                        {
                            WYCast.AddDestination(m_streamId, -1, m_encoderId, m_Destination);

                            //Start Capture
                            if (!Capture())
                                return;
                        }
                    }
                }
            }
        }
    }

    IEnumerator CaptureScreen()
    {
        yield return new WaitForEndOfFrame();

        while (true)
        {
            m_renderTexture = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.ARGB32);
            ScreenCapture.CaptureScreenshotIntoRenderTexture(m_renderTexture);
            if( m_renderTexture != null )
            {
                AsyncGPUReadback.Request(m_renderTexture, 0, TextureFormat.ARGB32, ReadbackCompleted);
            }
            yield return new WaitForEndOfFrame();
        }
    }

    void ReadbackCompleted(AsyncGPUReadbackRequest t_request)
    {
        DestroyImmediate( m_renderTexture );
        using (var imageBytes = t_request.GetData<byte>())
        {
            unsafe
            {
                System.IntPtr ptr = (System.IntPtr)((NativeArray<byte>)imageBytes).GetUnsafePtr();
                if (m_bufferSourceId >= 0)
                {
                    while( !WYCast.WriteBuffer(m_bufferSourceId, ptr) )
                    {

                    }
                }
            }
        }
    }

    private void OnDisable()
    {
        StopCoroutine(m_coroutine);

        if (m_streamId >= 0)
        {
            WYCast.DestroyStream(m_streamId);
            WYCast.DeleteSourceBuffer(m_bufferSourceId);

            m_bufferSourceId = -1;
            m_streamId = -1;
            m_sourceId = -1;
            m_converterId = -1;
            m_encoderId = -1;
        }
    }
}
