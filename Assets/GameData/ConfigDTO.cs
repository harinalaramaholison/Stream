using UnityEngine;

[CreateAssetMenu(fileName = "ConfigDto", menuName = "Wycast/ConfigSW", order = 1)]
public class ConfigDTO : ScriptableObject
{
    public bool m_audio = true;
    public bool m_video = true;
    public int m_width = 1280;
    public int m_height = 720;
    public bool m_flipX = false;
    public bool m_flipY = true;
    public bool m_emit = false;
    public string m_options = "";
    public uint m_timeout = 10000;
    public int m_logLevel = 6;

    public string m_peerName = "user2_F4A6a";
    public string m_ConversionFormat = "yuv420p";
    public string m_Codec = "libx264";
    public string m_Destination = string.Empty;
    public string m_ConfigPath = "Config.txt";
    public string m_StreamName = "Christian_Stream";
        
    public string m_license = "2RGFL5YHGVWPLPQR6D4RNPCXSBUBE529F42FD825";
    //public string m_token = "{\"token\":\"eyJhbGciOiJFQ0RILUVTK0EyNTZLVyIsImVuYyI6IkEyNTZHQ00iLCJlcGsiOnsiY3J2IjoiUC0yNTYiLCJrdHkiOiJFQyIsIngiOiJXRzlyYll2dzNNd2F3Qy1XLXN2TDNfUllUU0diZy1mcVhoRDhxdVVBSzdvIiwieSI6IkhkaWk4dlFhTWJTN0tUbENIQ21zZ08yUHhod0k1OVFjSzB3cUIyZXptMWsifSwia2lkIjoiZGdJMG9nWUdrdndOTDg0dExQQVhQWXdkSlhMYkF4allhNGVzSERXY0VPWSJ9.amDalM2T3tI2258oe33mdQSLiWL7nEc5ElaaMu-Am6V2O_yM2zdG2g.oddXgkB7qAbflUU5.8640Qo8QVWpKQ4rVnM374oaigYjck7UIw1xbwMH78HPx0q1QOObcBAnmiVDkzJuPW581OBgZGw_53WysXzSWSY7Fn6uKywaB6GGCOl4OqDJUtT-6oC9OWxdXWwUEbwWMqyvZDUIKTZq3t2CJNTbMvKP9o5pba_k_B7k0_wuIbRGBfLyNGFDj_oowfMgd3GXsZpE0RYSWwTb8Ftu83I5qon1GAoEAT2ag7w-JKyx9H3vtW6eFQfyXWC_McINU31MnNakqYRAOiDytYbrc1RvROEMsrOyZAJmYQI2ooBJGVTwBRdEC0H3fZbNWQojZyFQoiQaMPMir5Rf3_HfHdBEBDcRC3tF6Fq4hQ_T5IBgWYryJLjStEIwonq2Y2IaqGcsNLi4C0SpGxCNt3gWO-TrmsYXJbVHxKnfbI2VGlTylfMhaPgpCgHLL.5IufvnoqAgG0kJSB1ZkirQ\"}";
    public string m_token = "";
}
