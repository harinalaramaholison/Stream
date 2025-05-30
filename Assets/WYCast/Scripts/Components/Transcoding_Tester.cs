using System;
using System.Collections;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Rendering;

public class Transcoding_Tester : MonoBehaviour
{
   private int m_streamId = -1;
   private int m_sourceId = -1;
   private int m_decoderId = -1;
   //private int m_converterId = -1;
   private int m_TranscoderId = -1;
   private int m_EncoderId = -1;

   //public
   public string m_video_src_format = "rgba";
   public string m_video_codec = "libx264";
   public string m_input = "";
   public string m_output = "SampleMedia\\output.mp4";
   public string m_option = "";
   public string m_format = "";

    private void Start()
    {
        m_streamId = WYCast.CreateStream();

        if(m_streamId >= 0 )
        {
            m_sourceId = WYCast.AddSource(m_streamId, m_input, false);

            if (m_sourceId >= 0)
            {
               m_decoderId = WYCast.AddVideoDecoder(m_streamId, m_sourceId, 8);               

               //m_converterId = WYCast.AddVideoConverter(m_streamId, m_decoderId, "yuv420p", 1280, 720 );

               m_TranscoderId = WYCast.AddVideoTranscoder(m_streamId,  m_decoderId, m_video_codec );

               m_EncoderId = WYCast.AddVideoEncoder(m_streamId,  m_TranscoderId, m_video_codec );
                   
               WYCast.AddDestination(m_streamId , -1, m_EncoderId, m_output);
            }
        }
    }


   public void Destroy()
   {
      if (m_streamId >= 0)
      {
         WYCast.DestroyStream(m_streamId);
         WYCast.InitLogVariable();
         WYCast.DisconnectAll(m_streamId);

         m_streamId = -1;
         m_sourceId = -1;
         m_decoderId = -1;
         //m_converterId = -1;
         m_TranscoderId= -1;
         m_EncoderId = -1;
      }
   }

    private void OnDisable()
    {
        Destroy();
    }
}
