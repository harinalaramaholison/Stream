using System;
using System.Drawing;
using System.Runtime.InteropServices;

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
#else
//using UnityEngine;
#endif

/// <summary>
/// Exposed C functions of WYCast as C# methods for Unity scripts
/// </summary>
internal static class WYCast
{
    /// <summary>
    /// Allocate resources for a stream
    /// </summary>
    /// <returns>Stream ID</returns>
#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
    [DllImport("wycast_client_sdk")]
#endif
    public static extern int CreateStream();


#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
    [DllImport("wycast_client_sdk")]
#endif
    public static extern int Remove(int streamId, int id);


    /// <summary>
    /// Adds a source of audio/video packets to a stream
    /// </summary>
    /// <param name="streamId">ID of the stream to add this decoder to</param>
    /// <param name="uri">Address of the multimedia file or stream</param>
    /// <param name="audio">Receive audio frames</param>
    /// <param name="video">Receive video frames</param>
    /// <param name="format">Format of the multimedia file or stream</param>
    /// <param name="options">FFmpeg options for reading multimedia file or stream</param>
    /// <param name="packetCount">Maximum number of packets in the queue</param>
    /// <returns>ID</returns>
#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
    [DllImport("wycast_client_sdk")]
#endif
    public static extern int AddSource(int streamId, string uri, bool audio = true, bool video = true, uint timeout = 10000, string format = "", string options = "", ulong packetCount = 8);


#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
    [DllImport("wycast_client_sdk")]
#endif
    public static extern int AddDestination(int streamId, int audioSrcId, int videoSrcId, string uri, uint timeout = 10000, string format = "", string options = "");


    /// <summary>
    /// Adds an audio decoder to a stream
    /// </summary>
    /// <param name="streamId">identifier of the stream where this decoder will be added to</param>
    /// <param name="srcId">identifier of a source of encoded audio packets in the stream</param>
    /// <param name="frameCount">maximum number of frames to buffer</param>
    /// <returns>audio decoder identifier</returns>
#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
    [DllImport("wycast_client_sdk")]
#endif
    public static extern int AddAudioDecoder(int streamId, int srcId, uint frameCount = 4);


    /// <summary>
    /// Adds a video decoder to a stream
    /// </summary>
    /// <param name="streamId">identifier of the stream where this decoder will be added to</param>
    /// <param name="srcId">identifier of a source of video packets in the stream</param>
    /// <param name="frameCount">maximum number of frames to buffer</param>
    /// <returns>video decoder identifier</returns>
#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
    [DllImport("wycast_client_sdk")]
#endif
    public static extern int AddVideoDecoder(int streamId, int srcId, uint frameCount = 4);


#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
    [DllImport("wycast_client_sdk")]
#endif
    public static extern int AddAudioEncoder(int streamId, int srcId, string codec, uint packetCount = 4);


#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
    [DllImport("wycast_client_sdk")]
#endif
    public static extern int AddVideoEncoder(int streamId, int srcId, string codec, uint packetCount = 4);


    //Audio converter for a specific audio decoder.
#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
    [DllImport("wycast_client_sdk")]
#endif
    public static extern int AddAudioConverter(int streamId, int srcId, string format, int channels, int samplerate, uint frameCount = 4);


    //Video converter for a specific video decoder.
#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
    [DllImport("wycast_client_sdk")]
#endif
    public static extern int AddVideoConverter(int streamId, int srcId, string format, int width, int height, uint frameCount = 4);


    /// <summary>
    /// Adds a converter of decoded audio frames to a stream. This sink will dump all data to a audioclip buffer. It will convert the data to be suited to the format of that buffer
    /// </summary>
    /// <param name="streamId">identifier of the stream where this converter will be added to</param>
    /// <param name="srcId">identifier of a source of audio frames in the stream</param>
    /// <param name="audioClipId">identifier of a audioclip buffer where the converted frames will be dumped to</param>
    /// <returns>audio converter sink identifier</returns>
#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
    [DllImport("wycast_client_sdk")]
#endif
    public static extern int AddAudioConverterSink(int streamId, int srcId, int audioClipId);


    /// <summary>
    /// Adds a converter of decoded video frames to a stream. This sink will dump all data to a texture buffer. It will convert the data to be suited to the format of that buffer
    /// </summary>
    /// <param name="streamId">identifier of the stream where this converter will be added to</param>
    /// <param name="srcId">identifier of a source of video frames in the stream</param>
    /// <param name="textureId">identifier of a texture buffer where the converted frames will be dumped to</param>
    /// <returns>video converter sink identifier</returns>
#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
    [DllImport("wycast_client_sdk")]
#endif
    public static extern int AddVideoConverterSink(int streamId, int srcId, int textureId);

#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
    [DllImport("wycast_client_sdk")]
#endif
    public static extern int AddVideoTranscoder(int streamId, int srcId, string codec, uint frameCount = 4);

    /// <summary>
    /// Creates a buffer queueing raw audio frame data
    /// </summary>
    /// <param name="format">format of the audio</param>
    /// <param name="channels">number of channels</param>
    /// <param name="samplerate">samples per second</param>
    /// <param name="samples">number of samples per frame</param>
    /// <param name="startTime">time to start audio playback</param>
    /// <param name="frameCount">maximum number of frames to buffer</param>
    /// <returns>audioclip buffer identifier</returns>
#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
    [DllImport("wycast_client_sdk")]
#endif
    public static extern int CreateAudioClipBuffer(string format, int channels, int samplerate, int samples, long startTime = 0, uint frameCount = 4);


#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
    [DllImport("wycast_client_sdk")]
#endif
    public static extern ulong GetAudioClipSize(int audioClipId);


    /// <summary>
    /// Creates a buffer for queueing raw video frame data
    /// </summary>
    /// <param name="texture">pointer to a texture in Unity returned by Texture2D.GetNativeTexturePtr()</param>
    /// <param name="format">pixel format of the texture</param>
    /// <param name="width">width of the texture</param>
    /// <param name="height">height of the texture</param>
    /// <param name="startTime">time to start video playback</param>
    /// <param name="frameCount">maximum number of frames to buffer</param>
    /// <returns>texture buffer identifier</returns>
#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
    [DllImport("wycast_client_sdk")]
#endif
    public static extern int CreateTextureBuffer(System.IntPtr texture, string format, int width, int height, long startTime = 0, uint frameCount = 4);


#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
    [DllImport("wycast_client_sdk")]
#endif
    public static extern ulong GetTextureSize(int textureId);


    //Copy frame from audio clip buffer.
#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
    [DllImport("wycast_client_sdk")]
#endif
    public static extern bool ReadAudio(int audioClipId, System.IntPtr buffer);


#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
    [DllImport("wycast_client_sdk")]
#endif
    public static extern void DropNextAudioFrame(int audioClipId);


    //Copy frame from texture buffer.
#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
    [DllImport("wycast_client_sdk")]
#endif
    public static extern bool ReadVideo(int textureId, System.IntPtr buffer);


#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
    [DllImport("wycast_client_sdk")]
#endif
    public static extern void DropNextVideoFrame(int textureId);


    //Copy frame from audio clip buffer.
#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
    [DllImport("wycast_client_sdk")]
#endif
    public static extern bool ReadAudioNow(int audioClipId, System.IntPtr buffer);


    //Copy frame from texture buffer.
#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
    [DllImport("wycast_client_sdk")]
#endif
    public static extern bool ReadVideoNow(int textureId, System.IntPtr buffer);


    //Render next frame of the texture buffer.
#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
    [DllImport("wycast_client_sdk")]
#endif
    public static extern System.IntPtr RenderNextVideoFrame();


    //Render next frame of the texture buffer.
#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
    [DllImport("wycast_client_sdk")]
#endif
    public static extern void UpdateTexture(int texId);


#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
    [DllImport("wycast_client_sdk")]
#endif
    public static extern void UpdateTextureNow(int texId);


#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
    [DllImport("wycast_client_sdk")]
#endif
    public static extern bool IsTextureReady(int texId);


#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
    [DllImport("wycast_client_sdk")]
#endif
    public static extern System.IntPtr RenderVideoFrameNow();


#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
    [DllImport("wycast_client_sdk")]
#endif
    public static extern void DestroyStream(int streamId);


#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
    [DllImport("wycast_client_sdk")]
#endif
    public static extern void DeleteAudioClipBuffer(int audioClipId);



#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
    [DllImport("wycast_client_sdk")]
#endif
    public static extern int DeleteTextureBuffer(int textureId);


#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
    [DllImport("wycast_client_sdk")]
#endif
    public static extern void DeleteAllAudioClipBuffers();


#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
    [DllImport("wycast_client_sdk")]
#endif
    public static extern void DeleteAllTextureBuffers();


#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
    [DllImport("wycast_client_sdk")]
#endif
    public static extern void DestroyAllStreams();


#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
    [DllImport("wycast_client_sdk")]
#endif
    public static extern void Run();


#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
    [DllImport("wycast_client_sdk")]
#endif
    public static extern void Suspend();


    //Increase thread count.
#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
    [DllImport("wycast_client_sdk")]
#endif
    public static extern bool MoreThreads();


    //Decrease thread count.
#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
    [DllImport("wycast_client_sdk")]
#endif
    public static extern bool LessThreads();


    //Get current time since epoch in nanoseconds.
#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
    [DllImport("wycast_client_sdk")]
#endif
    public static extern long GetTimeNow(int offset = 0);


    //Load the plugin. No need to call this unless for special reasons.
#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
    [DllImport("wycast_client_sdk")]
#endif
    public static extern void Load();


    //Unload the plugin. No need to call this unless for special reasons.
#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
    [DllImport("wycast_client_sdk")]
#endif
    public static extern void Unload();


    //ShareScreen
#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
    [DllImport("wycast_client_sdk")]
#endif
    public static extern int CreateSourceBuffer(string t_format, int t_width, int t_height, long t_startTime, uint t_frameCount = 4);

#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
    [DllImport("wycast_client_sdk")]
#endif
    public static extern int AddVideoSourceBuffer(int t_streamId, int t_bufferSourceId);

#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
    [DllImport("wycast_client_sdk")]
#endif
    public static extern void DeleteAllSourceBuffers();

#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
    [DllImport("wycast_client_sdk")]
#endif
    public static extern void DeleteSourceBuffer(int t_bufferSourceId);

#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
    [DllImport("wycast_client_sdk")]
#endif
    public static extern bool WriteBuffer(int t_bufferSourceId, System.IntPtr t_buffer);
   //End ShareScreen

   //SW API
#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
   [DllImport("wycast_client_sdk")]
#endif
   public static extern bool CreateSessionMgr(string t_lic, int log_lvl = 4);

#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
   [DllImport("wycast_client_sdk")]
#endif
   public static extern int AddPeer(int t_streamId, int t_sessionId, string t_peer, string t_audio, string t_video, int t_textureId);


#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
   [DllImport("wycast_client_sdk")]
#endif
   public static extern int ConnectRoom(int t_streamId,string t_token);

#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
   [DllImport("wycast_client_sdk")]
#endif
   public static extern int GetExistingSessionTeam(); 

#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
      [DllImport("wycast_client_sdk")]
#endif
   public static extern System.IntPtr GetListOfPeers(int t_streamId);

#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
   [DllImport("wycast_client_sdk")]
#endif
   public static extern string GetListOfStreams(int t_streamId);

#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
   [DllImport("wycast_client_sdk")]
#endif
   public static extern int ConnectPeer(int t_streamId, int t_peerId);

#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
   [DllImport("wycast_client_sdk")]
#endif
   public static extern void DisconnectAll(int t_streamId);

#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
   [DllImport("wycast_client_sdk")]
#endif
   public static extern int CreateOutboundStream(int t_streamId, int t_srcId, int t_sessionTeam, int t_sessionId, string t_streamName);

#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
   [DllImport("wycast_client_sdk")]
#endif
   public static extern int AddInboundStream(int t_streamId, int t_srcId, string t_peerOwner);

   //Call Back
   //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
   //public delegate void NewInBoundStreamFunc(string PeerId);

   //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
   //public delegate void DelInBoundStreamFunc(string PeerId);

#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
   [DllImport("wycast_client_sdk")]
#endif
   public static extern void RegisterNewInboundStreamFunc(IntPtr callback);

#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
   [DllImport("wycast_client_sdk")]
#endif
   public static extern void RegisterDelInboundStreamFunc(IntPtr callback);
   //End SW API

   //Logging
   [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
   public delegate void PrintLog(string msg);

#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
   [DllImport("wycast_client_sdk")]
#endif
   public static extern void RegisterPrinter(PrintLog callback);

#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
   [DllImport("wycast_client_sdk")]
#endif
   public static extern void InitLogVariable();


#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
   [DllImport("wycast_client_sdk")]
#endif
   public static extern void EnableLogging(bool a_bEnable);

#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
   [DllImport("wycast_client_sdk")]
#endif
   public static extern void WriteLogToFile(bool a_bEnable);

#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
   [DllImport("wycast_client_sdk")]
#endif
   public static extern void ReInitializeLogFile();

#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
   [DllImport("wycast_client_sdk")]
#endif
   public static extern void SetLogLevelforTrace(int a_log_lvl);

#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
   [DllImport("wycast_client_sdk")]
#endif
   public static extern void SetLogFilePath(string a_logFilePath);

#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
   [DllImport("wycast_client_sdk")]
#endif
   public static extern void SetLogFilterbyStreamId(int a_StreamId);

#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
   [DllImport("wycast_client_sdk")]
#endif
   public static extern void AddSDKLogFilterByProcess(int a_NewProcess);   
}

#if UNITY_EDITOR
[InitializeOnLoad]
public class EditorPluginCallback
{
   static EditorPluginCallback()
    {
        EditorApplication.playModeStateChanged += PlayModeStateChanged;
        EditorApplication.quitting += Quit;
        EditorApplication.wantsToQuit += WantsToQuit;
    }

    static void PlayModeStateChanged(PlayModeStateChange state)
    {
        switch (state)
        {
            case PlayModeStateChange.EnteredEditMode:
                WYCast.Unload();

                break;

            case PlayModeStateChange.ExitingEditMode:
                WYCast.Load();
                //WYCast.EnableLogging(false);
            break;

            default:
                break;
        }
    }

    static bool WantsToQuit()
    {
        if (EditorApplication.isPlaying)
        {
            EditorApplication.ExitPlaymode();

            return false;
        }
        else
            return true;
    }

    static void Quit()
    {
        WYCast.Unload(); //UnityPluginUnload not being called by Unity
    }
}
#else
/*
public class PluginCallback
{
    static PluginCallback()
    {
        Application.quitting += Quit;
    }

    static void Quit()
    {
        WYCastAPI.Unload(); //UnityPluginUnload not being called by Unity
    }
}
*/
#endif