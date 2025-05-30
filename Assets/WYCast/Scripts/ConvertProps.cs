public class AudioConvert
{
    private string m_format;
    private int m_channels;
    private int m_samplerate;
    private int m_samples;

    public string Format
    {
        get => m_format == null ? "" : m_format;
        set => m_format = value;
    }

    public int Channels
    {
        get => m_channels;
        set => m_channels = value;
    }

    public int Samplerate
    {
        get => m_samplerate;
        set => m_samplerate = value;
    }

    public int Samples
    {
        get => m_samples;
        set => m_samples = value;
    }
}

public class VideoConvert
{
    private string m_format;
    private int m_width;
    private int m_height;

    public string Format
    {
        get => m_format == null ? "" : m_format;
        set => m_format = value;
    }

    public int Width
    {
        get => m_width;
        set => m_width = value;
    }

    public int Height
    {
        get => m_height;
        set => m_height = value;
    }
}