using System;
using System.Collections;
using Unity.Collections;
using UnityEngine;

public class SWTester_OutBound : MonoBehaviour
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

   public string m_URL = "";
   public bool m_audio = true;
   public bool m_video = true;
   public int m_width = DEFAULT_TEXTURE_FORMAT.width;
   public int m_height = DEFAULT_TEXTURE_FORMAT.height;
   public bool m_flipX = false;
   public bool m_flipY = false;
   public bool m_emit = false;
   public string m_options = "";
   public uint m_timeout = 10000;

   public string m_peerName = "user2_F4A6a";
   public string m_ConversionFormat = "yuv420p";
   public string m_Codec = "libx264";
   public string m_Destination = string.Empty;

   private string m_config = "{\"user\":\"user2_F4A6a\",\"password\":\"authnotneededondemossesions\",\"session\":\"sdk_session_demo\",\"signaling\":\"https://swdemo.evostream.com:5555\"}";
   private int m_streamId = -1;
   private int m_sourceId = -1;
   private int m_sessionTeam = -1;
   private int m_sessionId = -1;
   private int m_decoderId = -1;
   private int m_converterId = -1;
   private int m_outboundId = -1;

   private int m_inboundId = -1;

   private int m_textureId = -1;
   private long m_startTime = -1;
   //private int m_peerId = -1;
   private int m_videoConverterId = -1;

   private Coroutine m_videoRenderer = null;
   private int DELAY = 200; //Playback delay in milliseconds. Can help in syncing audio and video. 


   static void PrinterMsg(string msg)
   {
      if (msg != null)
         Debug.Log(msg);
   }

   void Start()
   {
      WYCast.RegisterPrinter(PrinterMsg);

      if ( !WYCast.CreateSessionMgr("") )
      {
         Debug.Log("Session couldn't create");
         return;
      }

      m_streamId = WYCast.CreateStream();

      if (m_streamId >= 0)
      {
         //Create Session to connect to SW
         m_sessionTeam = WYCast.GetExistingSessionTeam();
         m_sessionId = WYCast.ConnectRoom(m_streamId, m_config);
         if (m_sessionId >= 0 && m_sessionTeam == -1)
            m_sessionTeam = m_streamId;

         //Run video first ShareScreen or Inout
         m_sourceId = WYCast.AddSource( m_streamId, m_URL, false );

         if( m_sourceId >= 0 ) 
         {
            //Decode if needed
            m_decoderId = WYCast.AddVideoDecoder(m_streamId, m_sourceId);
         }

         if( m_decoderId >= 0)
         {
            m_converterId = WYCast.AddVideoConverter( m_streamId, m_decoderId, DEFAULT_TEXTURE_FORMAT.name, DEFAULT_TEXTURE_FORMAT.width, DEFAULT_TEXTURE_FORMAT.height );  
         }

         if (m_sessionId >= 0 && m_converterId >= 0 )
         {
            m_outboundId = WYCast.CreateOutboundStream(m_streamId, m_converterId, m_sessionTeam, m_sessionId, "unity_out_stream");

            if( m_outboundId < 0)
               Destroy();
         }
         //InBoundStream Bot
         if( m_sessionId >= 0 && m_streamId >= 0 )
         {
            //WYCast.AddPeer( m_streamId, m_sessionId, m_peerName, "", "", -1 );

            m_inboundId = WYCast.AddInboundStream(m_streamId, m_sessionId, m_peerName );

            if( m_inboundId > 0)
            {
               //sink here
               Texture2D texture = new Texture2D(m_width, m_height, DEFAULT_TEXTURE_FORMAT.value, false);

               VideoConvert vConvert = new VideoConvert();
               vConvert.Format = DEFAULT_TEXTURE_FORMAT.name;
               vConvert.Width = m_width;
               vConvert.Height = m_height;

               if (SetTexture(texture.GetNativeTexturePtr(), vConvert))
               {
                  if (m_videoConverterId > 0)
                  {
                     GameObject pl = GameObject.Find("PlaneObj");
                     Renderer renderer = pl.GetComponent<Renderer>();
                     Material material = renderer.material;

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

               if (IsVideoReady())
                  m_videoRenderer = StartCoroutine(VideoRenderer()); //Render video frame loop.
            }
         }
      }
   }

   public void RenderVideo()
   {
      GL.IssuePluginEvent(WYCast.RenderNextVideoFrame(), m_textureId);
   }

   IEnumerator VideoRenderer()
   {
      while (true)
      {
         yield return new WaitForEndOfFrame();
         RenderVideo(); //Render next frame from texture buffer. Error if m_player is not initialized or is invalid.
      }
   }

   private bool SetTexture(System.IntPtr t_texturePtr, VideoConvert t_convert)
   {
      if (m_streamId < 0 || m_video == false)
         return false;

      RemoveTexture();

      if (m_startTime < 0)
         m_startTime = WYCast.GetTimeNow(DELAY);

      m_textureId = WYCast.CreateTextureBuffer(t_texturePtr, t_convert.Format, t_convert.Width, t_convert.Height, m_startTime);

      if (m_textureId < 0)
         goto TextureError;

        if (m_textureId < 0)
            goto TextureError;

        m_videoConverterId = WYCast.AddVideoConverterSink(m_streamId, m_inboundId, m_textureId);

        if (m_videoConverterId < 0)
            goto TextureError;

      return true;

   TextureError:
      RemoveTexture();
      return false;
   }

   private void RemoveTexture()
   {
      if (m_textureId >= 0)
         WYCast.DeleteTextureBuffer(m_textureId);
      m_textureId = -1;
   }

   private bool IsVideoReady()
   {
      return m_video && m_videoConverterId >= 0 && m_textureId >= 0;
   }
   private void OnDisable()
   {
      if (m_videoRenderer != null)
         StopCoroutine(m_videoRenderer); //Stop video loop first before destroying m_player.

      Destroy();
   }

   public void Destroy()
   {
      if (m_streamId >= 0)
      {
         WYCast.InitLogVariable();
         WYCast.DestroyStream(m_streamId);
         WYCast.DeleteTextureBuffer(m_textureId);

         m_inboundId = -1;
         m_videoConverterId = -1;
         m_textureId = -1;
         m_startTime = -1;
         m_streamId = -1;
         m_sourceId = -1;
         m_sessionId = -1;
         m_decoderId = -1;
         m_converterId = -1;
         m_outboundId = -1;

         WYCast.DisconnectAll(m_streamId);
      }
   }
}
