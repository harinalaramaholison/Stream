using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Video;

public class ScreenShare_memotest : MonoBehaviour
{
   private Coroutine m_coroutine = null;
   private RenderTexture m_renderTexture;
   private int m_streamId = -1;
   private int m_sourceId = -1;
   private int m_converterId = -1;
   private int m_EncoderId = -1;

   //ShareScreen
   private int m_bufferSourceId = -1;
   private string m_token = "";

    public string m_ConfigPath = "Christian.txt";
    public string m_license = "2RGFL5YHGVWPLPQR6D4RNPCXSBUBE529F42FD825";

    private bool Capture()
    {
        m_coroutine = StartCoroutine(CaptureScreen());
        return m_coroutine != null;
    }

   static void PrinterMsg(string msg)
   {
      if (msg != null)
         Debug.Log(msg);
   }

    private void Start()
    {
         WYCast.RegisterPrinter(PrinterMsg);

         //Load token
         m_token = File.ReadAllText(m_ConfigPath);

         if( m_token.Equals("") )
         {
            Debug.Log("Couldn't get token");
            return;
         }

        m_streamId = WYCast.CreateStream();

        if(m_streamId >= 0 )
        {
            m_bufferSourceId = WYCast.CreateSourceBuffer("rgba", Screen.width, Screen.height, WYCast.GetTimeNow(200));

            if (m_bufferSourceId >= 0)
            {
                m_sourceId = WYCast.AddVideoSourceBuffer(m_streamId, m_bufferSourceId);

               if( m_sourceId >= 0)
               {
                   m_converterId = WYCast.AddVideoConverter(m_streamId, m_sourceId, "yuv420p", 1280, 720 );

                   m_EncoderId = WYCast.AddVideoEncoder(m_streamId,  m_converterId, "libx264" );
                   
                   WYCast.AddDestination(m_streamId , -1, m_EncoderId, "SampleMedia/output.mp4");
               }

               if (!Capture())
                  return;
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
            GC.Collect();
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
                    WYCast.WriteBuffer(m_bufferSourceId, ptr);
                    imageBytes.Dispose();
                }
            }
        }
    }


   public void Destroy()
   {
      if (m_streamId >= 0)
      {
         WYCast.DestroyStream(m_streamId);
         WYCast.DeleteSourceBuffer(m_bufferSourceId);
         WYCast.InitLogVariable();
         WYCast.DisconnectAll(m_streamId);

         m_streamId = -1;
         m_bufferSourceId = -1;
         m_sourceId = -1;
         m_converterId = -1;
      }
   }

    private void OnDisable()
    {
        if( m_coroutine != null )
           StopCoroutine(m_coroutine);

        Destroy();
    }
}
