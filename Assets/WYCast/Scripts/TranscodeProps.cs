public class AudioTranscode
{
    private string m_codec;

    public string Codec
    {
        get => m_codec == null ? "" : m_codec;
        set => m_codec = value;
    }
}

public class VideoTranscode
{
    private string m_codec;

    public string Codec
    {
        get => m_codec == null ? "" : m_codec;
        set => m_codec = value;
    }
}

