using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ConfigSetting : MonoBehaviour
{
    [SerializeField]
    private ConfigDTO configDto;

    [SerializeField]
    string sceneToLoadName;

    [SerializeField]
    Toggle audioTg;
    [SerializeField]
    Toggle videoTg;
    [SerializeField]
    TMP_InputField widthIF;
    [SerializeField]
    TMP_InputField heightIF;
    [SerializeField]
    Toggle flipXTg;
    [SerializeField]
    Toggle flipYTg;
    [SerializeField]
    Toggle emitTg;
    [SerializeField]
    TMP_InputField optionIF;
    [SerializeField]
    TMP_InputField timeoutIF;
    [SerializeField]
    TMP_InputField logLevelIF;
    [SerializeField]
    TMP_InputField peerNameIF;
    [SerializeField]
    TMP_InputField conversionFormatIF;
    [SerializeField]
    TMP_InputField codecIF;
    [SerializeField]
    TMP_InputField destinationIF;
    [SerializeField]
    TMP_InputField configPathIF;
    [SerializeField]
    TMP_InputField streamNameIF;
    [SerializeField]
    TMP_InputField licenseIF;
    
    private void Start()
    {
        audioTg.isOn = configDto.m_audio;
        videoTg.isOn = configDto.m_video;
        widthIF.text = configDto.m_width.ToString();
        heightIF.text = configDto.m_height.ToString();
        flipXTg.isOn = configDto.m_flipX;
        flipYTg.isOn = configDto.m_flipY;
        emitTg.isOn = configDto.m_emit;
        optionIF.text = configDto.m_options;
        timeoutIF.text = configDto.m_timeout.ToString();
        logLevelIF.text = configDto.m_logLevel.ToString();
        peerNameIF.text = configDto.m_peerName;
        conversionFormatIF.text = configDto.m_ConversionFormat;
        codecIF.text = configDto.m_Codec;
        destinationIF.text = configDto.m_Destination;
        configPathIF.text = configDto.m_ConfigPath;
        streamNameIF.text = configDto.m_StreamName;
        licenseIF.text = configDto.m_license;
        //roomTokenIF.text = configDto.m_token;
    }

    public void SaveSetting()
    {
        configDto.m_audio = audioTg.isOn;
        configDto.m_video = videoTg.isOn;
        configDto.m_width = int.Parse(widthIF.text);
        configDto.m_height = int.Parse(heightIF.text);
        configDto.m_flipX = flipXTg.isOn;
        configDto.m_flipY = flipYTg.isOn;
        configDto.m_emit = emitTg.isOn;
        configDto.m_options = optionIF.text;
        configDto.m_timeout = uint.Parse(timeoutIF.text);
        configDto.m_logLevel = int.Parse(logLevelIF.text);
        configDto.m_peerName = peerNameIF.text;
        configDto.m_ConversionFormat = conversionFormatIF.text;
        configDto.m_Codec = codecIF.text;
        configDto.m_Destination = destinationIF.text;
        configDto.m_ConfigPath = configPathIF.text;
        configDto.m_StreamName = streamNameIF.text;
        configDto.m_license = licenseIF.text;
        //configDto.m_token = roomTokenIF.text; 
    }

    public void LoadInGame()
    {
        SceneManager.LoadScene(sceneToLoadName);
    }
}
