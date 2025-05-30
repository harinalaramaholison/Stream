using System.Collections;
using Unity.Collections;
using UnityEngine;

public class SWTester_InBound : MonoBehaviour
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

   public bool m_audio = true;
   public bool m_video = true;
   public int m_width = DEFAULT_TEXTURE_FORMAT.width;
   public int m_height = DEFAULT_TEXTURE_FORMAT.height;
   public bool m_flipX = false;
   public bool m_flipY = false;
   public bool m_emit = false;
   public string m_options = "";
   public uint m_timeout = 5000;
   public string m_format = "";
   private string m_config = "{\"user\":\"unity_in\",\"password\":\"\",\"session\":\"sdkdemo\",\"signaling\":\"https://swdemo.evostream.com:5555\"}";
   //private string m_config = "{\"user\":\"user2_3A2b9\",\"password\":\"K3nA0ZRP\",\"session\":\"sdkdemo\",\"signaling\":\"https://swdemo.evostream.com:5555\"}";
   private int m_textureId = -1;
   private long m_startTime = -1;
   private int m_streamId = -1;
   private int m_sessionTeam = -1;
   private int m_sessionId = -1;
   private int m_peerId = -1;
   private int m_videoConverterId = -1;
   private int m_inboundId = -1;

   private Coroutine m_videoRenderer = null;

   //We need them when we save to a file
   //public string m_ConversionFormat = "yuv420p";
   //public string m_Codec = "libx264";
   //public string m_Destination = string.Empty;

   private int DELAY = 200; //Playback delay in milliseconds. Can help in syncing audio and video. 
   static void PrinterMsg(string msg)
   {
      if (msg != null)
         Debug.Log(msg);
   }

   void Start()
   {
      WYCast.RegisterPrinter(PrinterMsg);

      if (!WYCast.CreateSessionMgr(""))
         return;

      m_streamId = WYCast.CreateStream();

      if (m_streamId >= 0)
      {
         //Create Session to connect to SW
         m_sessionTeam = WYCast.GetExistingSessionTeam();
         m_sessionId = WYCast.ConnectRoom(m_streamId, m_config);
         if (m_sessionId >= 0 && m_sessionTeam == -1)
            m_sessionTeam = m_streamId;

         if (m_sessionId >= 0)
         {
            if (m_video) //test only for video audio later
            {
               //Add peer
               m_peerId = WYCast.AddPeer( m_streamId, m_sessionId, "sw_tester", "", "", -1 );

               if( m_peerId > 0 )
                  m_inboundId = WYCast.AddInboundStream(m_sessionTeam, m_sessionId, "sw_tester" );

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
                     m_videoConverterId = WYCast.AddVideoConverterSink(m_streamId, m_inboundId, m_textureId);

                     if (m_videoConverterId > 0)
                     {
                        Renderer renderer = gameObject.GetComponent<Renderer>();
                        Material material = GetComponent<Renderer>().materials[0];

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
   public void Destroy()
   {
      if (m_streamId >= 0)
      {
         WYCast.DestroyStream(m_streamId);
         WYCast.DeleteTextureBuffer(m_textureId);

         m_streamId = -1;
         m_inboundId = -1;
         m_videoConverterId = -1;
         m_textureId = -1;
         m_startTime = -1;

         WYCast.DisconnectAll(m_streamId);
      }
   }    
   private void OnDisable()
   {
      if (m_videoRenderer != null)
         StopCoroutine(m_videoRenderer); //Stop video loop first before destroying m_player.

      Destroy();
   }
}
