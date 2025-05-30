using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ConfigPlaybackSetting : MonoBehaviour
{
    [SerializeField]
    private ConfigPlaybackDTO configPlaybackDto;

    [SerializeField]
    TMP_InputField playbackCamURLIF;
    [SerializeField]
    Toggle playbackAudioTg;
    [SerializeField]
    Toggle playbackVideoTg;
    [SerializeField]
    TMP_InputField playbackWidthIF;
    [SerializeField]
    TMP_InputField playbackHeightIF;
    [SerializeField]
    Toggle playbackFlipXTg;
    [SerializeField]
    Toggle playbackFlipYTg;
    [SerializeField]
    Toggle playbackEmitTg;
    [SerializeField]
    TMP_InputField playbackOptionIF;
    [SerializeField]
    TMP_InputField playbackTimeoutIF;
    [SerializeField]
    TMP_InputField playbackFormatIF;

    private void Start()
    {
        playbackCamURLIF.text = configPlaybackDto.m_cam_url;
        playbackAudioTg.isOn = configPlaybackDto.m_p_audio;
        playbackVideoTg.isOn = configPlaybackDto.m_p_video;
        playbackWidthIF.text = configPlaybackDto.m_p_width.ToString();
        playbackHeightIF.text = configPlaybackDto.m_p_height.ToString();
        playbackFlipXTg.isOn = configPlaybackDto.m_p_flipX;
        playbackFlipYTg.isOn = configPlaybackDto.m_p_flipY;
        playbackEmitTg.isOn = configPlaybackDto.m_p_emit;
        playbackOptionIF.text = configPlaybackDto.m_p_options;
        playbackTimeoutIF.text = configPlaybackDto.m_p_timeout.ToString();
        playbackFormatIF.text = configPlaybackDto.m_format;
    }

    public void SaveSetting()
    {
        configPlaybackDto.m_cam_url = playbackCamURLIF.text;
        configPlaybackDto.m_p_audio = playbackAudioTg.isOn;
        configPlaybackDto.m_p_video = playbackVideoTg.isOn;
        configPlaybackDto.m_p_width = int.Parse(playbackWidthIF.text);
        configPlaybackDto.m_p_height = int.Parse(playbackHeightIF.text);
        configPlaybackDto.m_p_flipX = playbackFlipXTg.isOn;
        configPlaybackDto.m_p_flipY = playbackFlipYTg.isOn;
        configPlaybackDto.m_p_emit = playbackEmitTg.isOn;
        configPlaybackDto.m_p_options = playbackOptionIF.text;
        configPlaybackDto.m_p_timeout = uint.Parse(playbackTimeoutIF.text);
        configPlaybackDto.m_format = playbackFormatIF.text;
    }
}
