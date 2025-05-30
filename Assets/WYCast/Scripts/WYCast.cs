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
    /// Allocate resources engine for a stream ( threads )
    /// </summary>
    /// <returns>Stream identifier</returns>
#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
    [DllImport("wycast_client_sdk")]
#endif
    public static extern int CreateStream();

    /// <summary>
    /// Release a component of a stream (stop thread associated with this component)
    /// </summary>
    /// <param name="streamId">Identifier of the stream resource container</param>
    /// <param name="id">Identifier of the component to release</param>
#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
    [DllImport("wycast_client_sdk")]
#endif
    public static extern void Remove(int streamId, int id);

    /// <summary>
    /// Adds a source of audio/video to a stream
    /// </summary>
    /// <param name="streamId">Identifier of the stream to add this source</param>
    /// <param name="uri">Address of the multimedia file - input device - uri - rtsp url...</param>
    /// <param name="audio">enable audio frames</param>
    /// <param name="video">enable video frames</param>
    /// <param name="timeout">Accepted time out for processing</param>
    /// <param name="format">Format of the multimedia (ex: use dshow when the input is a device from windows. Empty if it is a media file...)</param>
    /// <param name="options">FFmpeg options for reading multimedia file or stream</param>
    /// <param name="packetCount">Maximum number of packets in the queue</param>
    /// <returns>Identifier of the component added (related of the thread)</returns>
#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
    [DllImport("wycast_client_sdk")]
#endif
    public static extern int AddSource(int streamId, string uri, bool audio = true, bool video = true, uint timeout = 10000, string format = "", string options = "", ulong packetCount = 8);

    /// <summary>
    /// Adds a destination of audio/video processed to a stream
    /// </summary>
    /// <param name="streamId">Identifier of the stream to add this destination component</param>
    /// <param name="audioSrcId">Identifier of the audio source for the destination</param>
    /// <param name="videoSrcId">Identifier of the video source for the destination</param>
    /// <param name="uri">Address of the multimedia file or stream as destination</param>
    /// <param name="timeout">Accepted time out for processing</param>
    /// <param name="format">Format of the multimedia</param>
    /// <param name="options">FFmpeg options for writing multimedia file or stream</param>
    /// <returns>Identifier of the component added (related of the thread)</returns>
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

    /// <summary>
    /// Adds an audio encoder to a stream
    /// </summary>
    /// <param name="streamId">identifier of the stream where this encoder will be added to</param>
    /// <param name="srcId">identifier of a source of decoded audio frames in the stream</param>
    /// <param name="codec">codec name to encode the audio</param>
    /// <param name="packetCount">maximum number of packets to buffer</param>
    /// <returns>audio encoder identifier</returns>
#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
    [DllImport("wycast_client_sdk")]
#endif
    public static extern int AddAudioEncoder(int streamId, int srcId, string codec, uint packetCount = 4);

    /// <summary>
    /// Adds a video encoder to a stream
    /// </summary>
    /// <param name="streamId">identifier of the stream where this encoder will be added to</param>
    /// <param name="srcId">identifier of a source of decoded audio frames in the stream</param>
    /// <param name="codec">codec name to encode the video</param>
    /// <param name="packetCount">maximum number of packets to buffer</param>
    /// <returns>video encoder identifier</returns>
#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
    [DllImport("wycast_client_sdk")]
#endif
    public static extern int AddVideoEncoder(int streamId, int srcId, string codec, uint packetCount = 4);

    /// <summary>
    /// Adds an audio format converter for a specific audio decoded
    /// </summary>
    /// <param name="streamId">identifier of the stream where this converter will be added to</param>
    /// <param name="srcId">identifier of a source of decoded audio frames to convert in the stream</param>
    /// <param name="format">audio format destination</param>
    /// <param name="channels">audio channels number (1:mono, 2:stereo...)</param>
    /// <param name="samplerate">audio samplerate in Hz (ex: 44.1kHz => 44100 ...)</param>
    /// <param name="frameCount">maximum number of frames to buffer</param>
    /// <returns>audio converter identifier</returns>
#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
    [DllImport("wycast_client_sdk")]
#endif
    public static extern int AddAudioConverter(int streamId, int srcId, string format, int channels, int samplerate, uint frameCount = 4);

    /// <summary>
    /// Adds an video format converter for a specific video decoded
    /// </summary>
    /// <param name="streamId">identifier of the stream where this converter will be added to</param>
    /// <param name="srcId">identifier of a source of decoded video frames to convert in the stream</param>
    /// <param name="format">video format destination (rgba, argb...) </param>
    /// <param name="width">new width of the video</param>
    /// <param name="height">new height of the video</param>
    /// <param name="frameCount">maximum number of frames to buffer</param>
    /// <returns>video converter identifier</returns>
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

    /// <summary>
    /// Adds a video transcoder to a stream. It converts only the codec of the media (ex: mpeg4 to h264)
    /// </summary>
    /// <param name="streamId">identifier of the stream where this transcoder will be added to</param>
    /// <param name="srcId">identifier of a source of decoded video frames in the stream</param>
    /// <param name="codec">new codec of the video</param>
    /// <param name="frameCount">maximum number of frames to buffer</param>
    /// <returns>video transcoder identifier</returns>
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

    /// <summary>
    /// Get the size of the audio (in bytes)
    /// </summary>
    /// <param name="audioClipId">index of the audio clip (in memory array)</param>
    /// <returns>size of the audio clip in byte</returns>
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

    /// <summary>
    /// Get the size of the video (in bytes)
    /// </summary>
    /// <param name="textureId">index of the video texture (in the memory array)</param>
    /// <returns>size of the video in byte</returns>
#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
    [DllImport("wycast_client_sdk")]
#endif
    public static extern ulong GetTextureSize(int textureId);

    /// <summary>
    /// Read the raw audio in the buffer
    /// </summary>
    /// <param name="audioClipId">index of the audio clip (in memory array)</param>
    /// <param name="buffer">pointer of the buffer to read</param>
    /// <returns>result of the reading : success or failed</returns>
#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
    [DllImport("wycast_client_sdk")]
#endif
    public static extern bool ReadAudio(int audioClipId, System.IntPtr buffer);

    /// <summary>
    /// Drop the next raw audio frame in the buffer container
    /// </summary>
    /// <param name="audioClipId">index of the audio clip (in memory array)</param>
#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
    [DllImport("wycast_client_sdk")]
#endif
    public static extern void DropNextAudioFrame(int audioClipId);


    /// <summary>
    /// Read the raw video in the buffer
    /// </summary>
    /// <param name="textureId">index of the video texture (in memory array)</param>
    /// <param name="buffer">pointer of the buffer to read</param>
    /// <returns>result of the reading : success or failed</returns>
#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
    [DllImport("wycast_client_sdk")]
#endif
    public static extern bool ReadVideo(int textureId, System.IntPtr buffer);

    /// <summary>
    /// Drop the next raw video frame in the buffer container
    /// </summary>
    /// <param name="textureId">index of the video texture (in memory array)</param>
#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
    [DllImport("wycast_client_sdk")]
#endif
    public static extern void DropNextVideoFrame(int textureId);

    /// <summary>
    /// Read the raw audio in the buffer now whatever is there
    /// </summary>
    /// <param name="audioClipId">index of the audio clip (in memory array)</param>
    /// <param name="buffer">pointer of the buffer to read</param>
    /// <returns>result of the reading : success or failed</returns>
 #if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
    [DllImport("wycast_client_sdk")]
#endif
    public static extern bool ReadAudioNow(int audioClipId, System.IntPtr buffer);

    /// <summary>
    /// Read the raw video in the buffer now whatever is there
    /// </summary>
    /// <param name="textureId">index of the video texture (in memory array)</param>
    /// <param name="buffer">pointer of the buffer to read</param>
    /// <returns>result of the reading : success or failed</returns>
#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
    [DllImport("wycast_client_sdk")]
#endif
    public static extern bool ReadVideoNow(int textureId, System.IntPtr buffer);

    /// <summary>
    /// Read/Render the next raw video in the buffer
    /// </summary>
    /// <returns>pointer of the buffer to read/render</returns>
#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
    [DllImport("wycast_client_sdk")]
#endif
    public static extern System.IntPtr RenderNextVideoFrame();

    /// <summary>
    /// Read/Render the next raw video in the texture buffer
    /// </summary>
    /// <param name="textureId">index of the video texture (in memory array)</param>
#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
    [DllImport("wycast_client_sdk")]
#endif
    public static extern void UpdateTexture(int textureId);

    /// <summary>
    /// Read/Render the next raw video in the texture buffer now whatever is there
    /// </summary>
    /// <param name="textureId">index of the video texture (in memory array)</param>
#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
    [DllImport("wycast_client_sdk")]
#endif
    public static extern void UpdateTextureNow(int textureId);

    /// <summary>
    /// To know if there is raw video data to read/render in the texture buffer
    /// </summary>
    /// <param name="textureId">index of the video texture (in memory array)</param>
    /// <returns>result yes or no</returns>
#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
    [DllImport("wycast_client_sdk")]
#endif
    public static extern bool IsTextureReady(int textureId);

    /// <summary>
    /// Read/Render the next raw frame video in the buffer
    /// </summary>
    /// <returns>pointer of the frame buffer to read/render</returns>
#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
    [DllImport("wycast_client_sdk")]
#endif
    public static extern System.IntPtr RenderVideoFrameNow();

    /// <summary>
    /// Deallocate resources engine for a stream ( threads )
    /// </summary>
    /// <param name="streamId">identifier of the stream to destroy</param>
    /// <returns>Stream identifier</returns>
#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
    [DllImport("wycast_client_sdk")]
#endif
    public static extern void DestroyStream(int streamId);

    /// <summary>
    /// Release an audio clip buffer
    /// </summary>
    /// <param name="audioClipId">index of the audio clip to release (in memory array)</param>
#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
    [DllImport("wycast_client_sdk")]
#endif
    public static extern void DeleteAudioClipBuffer(int audioClipId);

    /// <summary>
    /// Release a video texture buffer
    /// </summary>
    /// <param name="textureId">index of the video texture to release (in memory array)</param>
#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
    [DllImport("wycast_client_sdk")]
#endif
    public static extern void DeleteTextureBuffer(int textureId);

    /// <summary>
    /// Release all audios clip buffers
    /// </summary>
#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
    [DllImport("wycast_client_sdk")]
#endif
    public static extern void DeleteAllAudioClipBuffers();

    /// <summary>
    /// Release all videos clip buffers
    /// </summary>
#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
    [DllImport("wycast_client_sdk")]
#endif
    public static extern void DeleteAllTextureBuffers();

    /// <summary>
    /// Release all resources streams (stop all engine and threads)
    /// </summary>
#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
    [DllImport("wycast_client_sdk")]
#endif
    public static extern void DestroyAllStreams();

    /// <summary>
    /// Start all engines (threads) : prepare to receive command
    /// </summary>
#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
    [DllImport("wycast_client_sdk")]
#endif
    public static extern void Run();

    /// <summary>
    /// Suspend all engines (threads)
    /// </summary>
#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
    [DllImport("wycast_client_sdk")]
#endif
    public static extern void Suspend();


    /// <summary>
    /// Increase engine started count (Manual launch)
    /// </summary>
    /// <returns>result success or fialed to increase</returns>
#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
    [DllImport("wycast_client_sdk")]
#endif
    public static extern bool MoreThreads();

    /// <summary>
    /// Decrease engine started count (Manual launch)
    /// </summary>
    /// <returns>result success or fialed to increase</returns>
#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
    [DllImport("wycast_client_sdk")]
#endif
    public static extern bool LessThreads();

    /// <summary>
    /// Get current time since epoch in nanoseconds
    /// </summary>
    /// <returns>time value in nanoseconds (long format)</returns>
#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
    [DllImport("wycast_client_sdk")]
#endif
    public static extern long GetTimeNow(int offset = 0);

    /// <summary>
    /// Load the plugin. No need to call this unless for special reasons
    /// </summary>
#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
    [DllImport("wycast_client_sdk")]
#endif
    public static extern void Load();

    /// <summary>
    /// Unload the plugin. No need to call this unless for special reasons
    /// </summary>
#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
    [DllImport("wycast_client_sdk")]
#endif
    public static extern void Unload();

    //Share screen api overview
    /// <summary>
    /// Allocate a stream source buffer (video raw data)
    /// </summary>
    /// <param name="t_format">Pixel format of the data (rgba, yuv420,...)</param>
    /// <param name="t_width">width of the video frame</param>
    /// <param name="t_height">height of the video frame</param>
    /// <param name="t_startTime">time to start the diffusion of the frame</param>
    /// <param name="t_frameCount">Maximum number of frmae in the queue</param>
    /// <returns>Index of buffer source</returns>
#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
    [DllImport("wycast_client_sdk")]
#endif
    public static extern int CreateSourceBuffer(string t_format, int t_width, int t_height, long t_startTime, uint t_frameCount = 4);

    /// <summary>
    /// Adds a video source buffer to a stream
    /// </summary>
    /// <param name="t_streamId">identifier of the stream where this source buffer will be added to</param>
    /// <param name="t_bufferSourceId">index of the buffer source</param>
    /// <returns>Video source buffer identifier</returns>
#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
    [DllImport("wycast_client_sdk")]
#endif
    public static extern int AddVideoSourceBuffer(int t_streamId, int t_bufferSourceId);

    /// <summary>
    /// Release all sources buffers
    /// </summary>
#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
    [DllImport("wycast_client_sdk")]
#endif
    public static extern void DeleteAllSourceBuffers();

    /// <summary>
    /// Release an source buffer
    /// </summary>
    /// <param name="t_bufferSourceId">index of the source buffer to release (in memory array)</param>
#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
    [DllImport("wycast_client_sdk")]
#endif
    public static extern void DeleteSourceBuffer(int t_bufferSourceId);

    /// <summary>
    /// Send a buffer to source buffer
    /// </summary>
    /// <param name="t_bufferSourceId">index of the source buffer to push a buffer (in memory array)</param>
    /// <returns>results : success or failed to push a buffer</returns>
#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
    [DllImport("wycast_client_sdk")]
#endif
    public static extern bool WriteBuffer(int t_bufferSourceId, System.IntPtr t_buffer);
   //End ShareScreen

    //WYCast engine sdk (WebRTC) API overview
    /// <summary>
    /// Create wycast sdk engine session management
    /// </summary>
    /// <param name="t_lic">validation license to use the wycast sdk</param>
    /// <param name="log_lvl">level of log tracing : 0 - NO TRACE, 1 - ERROR, 2 - WARNING, 3 - INFO, 4 - DEBUG, 5 - VERBOSE, 6 - NOISY</param>
    /// <returns>results : success or failed to create session manager</returns>
#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
   [DllImport("wycast_client_sdk")]
#endif
   public static extern bool CreateSessionMgr(string t_lic, int log_lvl = 4);

    /// <summary>
    /// Adds a peer component management to a stream container (Not implemented yet since we don't have list of peer)
    /// </summary>
    /// <param name="streamId">identifier of the stream where this peer component will be added to</param>
    /// <param name="t_sessionId">the session where this peer joined</param>
    /// <param name="t_peer">peer name</param>
    /// <param name="t_audio">audio description</param>
    /// <param name="t_video">video description</param>
    /// <param name="t_textureId">identifier of texture</param>
    /// <returns>peer component identifier</returns>
#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
   [DllImport("wycast_client_sdk")]
#endif
   public static extern int AddPeer(int t_streamId, int t_sessionId, string t_peer, string t_audio, string t_video, int t_textureId);

    /// <summary>
    /// Join the wycast room, this create a session for this user
    /// </summary>
    /// <param name="streamId">identifier of the stream where this session will be added to</param>
    /// <param name="token">token as credential validation of using plugin and webrtc</param>
    /// <returns>session bot component identifier</returns>
#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
   [DllImport("wycast_client_sdk")]
#endif
   public static extern int ConnectRoom(int streamId,string token);

    /// <summary>
    /// Get existing session team if there is already team for the session
    /// </summary>
    /// <returns>existing session container identifier</returns>
#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
   [DllImport("wycast_client_sdk")]
#endif
   public static extern int GetExistingSessionTeam(); 

    /// <summary>
    /// Get the list of peers actually joined the room
    /// </summary>
    /// <param name="streamId">identifier of the stream where to get this list</param>
    /// <returns>pointer of the string buffer containing the list of peers</returns>
#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
      [DllImport("wycast_client_sdk")]
#endif
   public static extern System.IntPtr GetListOfPeers(int streamId);

    /// <summary>
    /// Get the list of streams played in the room
    /// </summary>
    /// <param name="streamId">identifier of the stream where to get this list</param>
    /// <returns>string containing the list of streams</returns>
#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
   [DllImport("wycast_client_sdk")]
#endif
   public static extern string GetListOfStreams(int streamId);

    /// <summary>
    /// Disconnect all connection of current user to the peers in a stream
    /// </summary>
    /// <param name="streamId">identifier of the stream where to disconnect all connection</param>
#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
   [DllImport("wycast_client_sdk")]
#endif
   public static extern void DisconnectAll(int streamId);

    /// <summary>
    /// Adds a component to manage outbound stream
    /// </summary>
    /// <param name="streamId">identifier of the stream where this outbound bot will be added to</param>
    /// <param name="t_srcId">component stream source to send</param>
    /// <param name="t_sessionTeam">identifier of the stream of the webrtc session</param>
    /// <param name="t_sessionId">identifier of the component session bot</param>
    /// <param name="t_streamName">stream name of this stream (use to identify this stream in the room, should be unique in the room)</param>
    /// <returns>outbound component identifier</returns>
#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
   [DllImport("wycast_client_sdk")]
#endif
   public static extern int CreateOutboundStream(int streamId, int t_srcId, int t_sessionTeam, int t_sessionId, string t_streamName);

    /// <summary>
    /// Adds a component to manage inbound stream
    /// </summary>
    /// <param name="streamId">identifier of the stream where this inbound bot will be added to</param>
    /// <param name="t_srcId">component stream source to receive (bot that managed a buffer)</param>
    /// <param name="t_peerOwner">peer name who sent this stream</param>
    /// <returns>inbound component identifier</returns>
#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
   [DllImport("wycast_client_sdk")]
#endif
   public static extern int AddInboundStream(int streamId, int t_srcId, string t_peerOwner);

    /// <summary>
    /// Register a callback to notify user when new inbound stream is added in the room
    /// </summary>
    /// <param name="callback">unmanaged function pointer of the callback</param>
#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
   [DllImport("wycast_client_sdk")]
#endif
   public static extern void RegisterNewInboundStreamFunc(IntPtr callback);

    /// <summary>
    /// Register a callback to notify user when an inbound stream is removed in the room
    /// </summary>
    /// <param name="callback">unmanaged function pointer of the callback</param>
#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
   [DllImport("wycast_client_sdk")]
#endif
   public static extern void RegisterDelInboundStreamFunc(IntPtr callback);

   //Logging api overview
   [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
   public delegate void PrintLog(string msg);

    /// <summary>
    /// Register a callback to send msg from log to the client application
    /// </summary>
    /// <param name="callback">unmanaged function pointer of the callback</param>
#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
   [DllImport("wycast_client_sdk")]
#endif
   public static extern void RegisterPrinter(PrintLog callback);

    /// <summary>
    /// Initialize log management
    /// </summary>
#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
   [DllImport("wycast_client_sdk")]
#endif
   public static extern void InitLogVariable();

    /// <summary>
    /// Enable or disable using log
    /// </summary>
    /// <param name="a_bEnable">enable or disable log</param>
#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
   [DllImport("wycast_client_sdk")]
#endif
   public static extern void EnableLogging(bool a_bEnable);

    /// <summary>
    /// Write log to a file
    /// </summary>
    /// <param name="a_bEnable">enable or disable writing log to the file</param>
#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport ("__Internal")]
#else
   [DllImport("wycast_client_sdk")]
#endif
   public static extern void WriteLogToFile(bool a_bEnable);
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
                WYCast.EnableLogging(true);
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