using System.Runtime.InteropServices;
using UnityEngine;

public class StreamOut
{
    private int m_streamId = -1;
    private int m_sourceId = -1;
    private int m_audioDecoderId = -1;
    private int m_videoDecoderId = -1;
    private int m_audioConverterId = -1;
    private int m_videoConverterId = -1;
    private int m_audioEncoderId = -1;
    private int m_videoEncoderId = -1;
    private int m_destinationId = -1;
    private bool m_audio = true;
    private bool m_video = true;

    public bool Init(string t_URL, bool t_audio = true, bool t_video = true)
    {
        Destroy();

        m_streamId = WYCast.CreateStream();

        if (m_streamId < 0)
            return false;

        m_audio = t_audio;
        m_video = t_video;

        m_sourceId = WYCast.AddSource(m_streamId, t_URL, m_audio, m_video, 10000, "dshow");

        if (m_sourceId < 0)
        {
            Destroy();

            return false;
        }
        else
            return true;
    }

    public bool SetDestination(string t_URL, AudioTranscode t_aTranscode = null, VideoTranscode t_vTranscode = null, AudioConvert t_aConvert = null, VideoConvert t_vConvert = null)
    {
        if (m_streamId < 0)
            return false;

        RemoveDestination();

        int audioSource = -1;

        if (m_audio)
        {
            if (t_aTranscode != null)
            {
                m_audioDecoderId = WYCast.AddAudioDecoder(m_streamId, m_sourceId);

                if (m_audioDecoderId < 0)
                    goto DestinationError;

                if (t_aConvert != null) //If there is no transcoder then ignore converter.
                {
                    m_audioConverterId = WYCast.AddAudioConverter(m_streamId, m_audioDecoderId, t_aConvert.Format, t_aConvert.Channels, t_aConvert.Samplerate);

                    if (m_audioConverterId < 0)
                        goto DestinationError;
                }

                m_audioEncoderId = WYCast.AddAudioEncoder(m_streamId, t_aConvert != null ? m_audioConverterId : m_audioDecoderId, t_aTranscode.Codec);

                if (m_audioEncoderId < 0)
                    goto DestinationError;

                audioSource = m_audioEncoderId;
            }
            else
                audioSource = m_sourceId;
        }

        int videoSource = -1;

        if(m_video)
        {
            if (t_vTranscode != null)
            {
                m_videoDecoderId = WYCast.AddVideoDecoder(m_streamId, m_sourceId);

                if (m_videoDecoderId < 0)
                    goto DestinationError;

                if (t_vConvert != null) //If there is no transcoder then ignore converter.
                {
                    m_videoConverterId = WYCast.AddVideoConverter(m_streamId, m_videoDecoderId, t_vConvert.Format, t_vConvert.Width, t_vConvert.Height);

                    if (m_videoConverterId < 0)
                        goto DestinationError;
                }

                m_videoEncoderId = WYCast.AddVideoEncoder(m_streamId, t_vConvert != null ? m_videoConverterId : m_videoDecoderId, t_vTranscode.Codec);

                if (m_videoEncoderId < 0)
                    goto DestinationError;

                videoSource = m_videoEncoderId;
            }
            else
                videoSource = m_sourceId;
        }

        m_destinationId = WYCast.AddDestination(m_streamId, audioSource, videoSource, t_URL);

        if (m_destinationId < 0)
            goto DestinationError;

        return true;

    DestinationError:
        RemoveDestination();
        return false;
    }

    public void RemoveDestination()
    {
        if (m_audioDecoderId >= 0)
            WYCast.Remove(m_streamId, m_audioDecoderId);

        if (m_audioConverterId >= 0)
            WYCast.Remove(m_streamId, m_audioConverterId);

        if (m_audioEncoderId >= 0)
            WYCast.Remove(m_streamId, m_audioEncoderId);

        if (m_videoDecoderId >= 0)
            WYCast.Remove(m_streamId, m_videoDecoderId);

        if (m_videoConverterId >= 0)
            WYCast.Remove(m_streamId, m_videoConverterId);

        if (m_videoEncoderId >= 0)
            WYCast.Remove(m_streamId, m_videoEncoderId);

        if (m_destinationId >= 0)
            WYCast.Remove(m_streamId, m_destinationId);

        m_audioDecoderId = -1;
        m_audioConverterId = -1;
        m_audioEncoderId = -1;
        m_videoDecoderId = -1;
        m_videoConverterId = -1;
        m_videoEncoderId = -1;
        m_destinationId = -1;
    }

    public void Destroy()
    {
        if (m_streamId >= 0)
        {
            WYCast.DestroyStream(m_streamId);

            m_streamId = -1;
            m_sourceId = -1;
            m_audioDecoderId = -1;
            m_videoDecoderId = -1;
            m_audioConverterId = -1;
            m_videoConverterId = -1;
            m_audioEncoderId = -1;
            m_videoEncoderId = -1;
            m_destinationId = -1;
        }
    }
}