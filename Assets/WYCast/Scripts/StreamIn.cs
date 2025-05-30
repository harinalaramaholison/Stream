using System.Runtime.InteropServices;
using UnityEngine;

public class StreamIn
{
    private int m_streamId = -1;
    private int m_sourceId = -1;
    private int m_audioDecoderId = -1;
    private int m_videoDecoderId = -1;
    private int m_audioConverterId = -1;
    private int m_videoConverterId = -1;
    private int m_audioClipId = -1;
    private int m_textureId = -1;
    private bool m_audio = true;
    private bool m_video = true;
    private long m_startTime = -1;

    /// <summary>
    /// Init video and audio parameters
    /// </summary>
    /// <param name="t_URL"></param>
    /// <param name="t_audio"></param>
    /// <param name="t_video"></param>
    /// <param name="t_timeout"></param>
    /// <param name="t_format"></param>
    /// <param name="t_options"></param>
    /// <returns>true if all is OK</returns>
    public bool Init(string t_URL, bool t_audio = true, bool t_video = true, uint t_timeout = 10000, string t_format = "", string t_options = "")
    {
        Destroy();

        m_streamId = WYCast.CreateStream();

        if (m_streamId < 0)
            return false;

        m_audio = t_audio;
        m_video = t_video;

        m_sourceId = WYCast.AddSource(m_streamId, t_URL, m_audio, m_video, t_timeout, t_format, t_options);

        if (m_sourceId < 0)
            goto InitError;

        if (m_audio)
        {
            m_audioDecoderId = WYCast.AddAudioDecoder(m_streamId, m_sourceId);

            if (m_audioDecoderId < 0)
                goto InitError;
        }

        if (m_video)
        {
            m_videoDecoderId = WYCast.AddVideoDecoder(m_streamId, m_sourceId);

            if (m_videoDecoderId < 0)
                goto InitError;
        }

        return true;

    InitError:
        Destroy();
        return false;
    }

    /// <summary>
    /// Set playback delay to synchronize video and audio
    /// </summary>
    /// <param name="t_delay"></param>
    public void SetDelay(int t_delay)
    {
        m_startTime = WYCast.GetTimeNow(t_delay);
    }

    /// <summary>
    /// Set audio clip
    /// </summary>
    /// <param name="t_convert"></param>
    /// <returns>true if all is OK</returns>
    public bool SetAudioClip(AudioConvert t_convert)
    {
        if (m_streamId < 0 || m_audio == false)
            return false;

        RemoveAudioClip();

        if (m_startTime < 0)
            m_startTime = WYCast.GetTimeNow();

        m_audioClipId = WYCast.CreateAudioClipBuffer(t_convert.Format, t_convert.Channels, t_convert.Samplerate, t_convert.Samples, m_startTime);

        if (m_audioClipId < 0)
            goto AudioClipError;

        m_audioConverterId = WYCast.AddAudioConverterSink(m_streamId, m_audioDecoderId, m_audioClipId);

        if (m_audioConverterId < 0)
            goto AudioClipError;
        
        return true;

    AudioClipError:
        RemoveAudioClip();
        return false;
    }

    /// <summary>
    /// Add video to a texture
    /// </summary>
    /// <param name="t_texturePtr"></param>
    /// <param name="t_convert"></param>
    /// <returns>true if all is OK</returns>
    public bool SetTexture(System.IntPtr t_texturePtr, VideoConvert t_convert)
    {
        if (m_streamId < 0 || m_video == false)
            return false;

        RemoveTexture();

        if (m_startTime < 0)
            m_startTime = WYCast.GetTimeNow();

        m_textureId = WYCast.CreateTextureBuffer(t_texturePtr, t_convert.Format, t_convert.Width, t_convert.Height, m_startTime);

        if (m_textureId < 0)
            goto TextureError;

        m_videoConverterId = WYCast.AddVideoConverterSink(m_streamId, m_videoDecoderId, m_textureId);

        if (m_videoConverterId < 0)
            goto TextureError;

        return true;

    TextureError:
        RemoveTexture();
        return false;
    }

    /// <summary>
    /// Clear audio clip
    /// </summary>
    public void RemoveAudioClip()
    {
        if (m_audioConverterId >= 0)
            WYCast.Remove(m_streamId, m_audioConverterId);

        if (m_audioClipId >= 0)
            WYCast.DeleteAudioClipBuffer(m_audioClipId);

        m_audioConverterId = -1;
        m_audioClipId = -1;

        //if (m_textureId < 0)
        //    m_startTime = -1;
    }

    /// <summary>
    /// Clear texture
    /// </summary>
    public void RemoveTexture()
    {
        if (m_videoConverterId >= 0)
            WYCast.Remove(m_streamId, m_videoConverterId);

        if (m_textureId >= 0)
            WYCast.DeleteTextureBuffer(m_textureId);

        m_videoConverterId = -1;
        m_textureId = -1;

        //if (m_audioClipId < 0)
        //    m_startTime = -1;
    }

    /// <summary>
    /// Read audio
    /// </summary>
    /// <param name="t_data"></param>
    public void ReadAudio(ref float[] t_data)
    {
        GCHandle handle = GCHandle.Alloc(t_data, GCHandleType.Pinned);
        WYCast.ReadAudio(m_audioClipId, handle.AddrOfPinnedObject());
        handle.Free();
    }

    /// <summary>
    /// Render video
    /// </summary>
    public void RenderVideo()
    {
        GL.IssuePluginEvent(WYCast.RenderNextVideoFrame(), m_textureId);
    }

    /// <summary>
    /// Audio verification
    /// </summary>
    /// <returns></returns>
    public bool IsAudioReady()
    {
        return m_audio && m_audioConverterId >= 0 && m_audioClipId >= 0;
    }

    /// <summary>
    /// Video verification
    /// </summary>
    /// <returns></returns>
    public bool IsVideoReady()
    {
        return m_video && m_videoConverterId >= 0 && m_textureId >= 0;
    }

    public void Destroy()
    {
        if (m_streamId >= 0)
        {
            WYCast.DestroyStream(m_streamId);
            WYCast.DeleteAudioClipBuffer(m_audioClipId);
            WYCast.DeleteTextureBuffer(m_textureId);

            m_streamId = -1;
            m_sourceId = -1;
            m_audioDecoderId = -1;
            m_videoDecoderId = -1;
            m_audioConverterId = -1;
            m_videoConverterId = -1;
            m_audioClipId = -1;
            m_textureId = -1;
            m_startTime = -1;
        }
    }
}