using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class RoomTokenManager : MonoBehaviour
{
    [SerializeField]
    private ConfigDTO configDto;
    [SerializeField]
    TMP_InputField roomTokenIF;

    void OnEnable()
    {

        configDto.m_token = (configDto.m_ConfigPath != null && configDto.m_ConfigPath != "") ? File.ReadAllText(configDto.m_ConfigPath) : "";
        GetToken();
    }

    public void GetToken()
    {
        if (configDto.m_token != null && configDto.m_token != "")
        {
            roomTokenIF.text = configDto.m_token;
        }
        else
        {
            roomTokenIF.text = string.Empty;
        }
    }

    public void SetToken()
    {
        if (roomTokenIF.text != null && roomTokenIF.text != "")
        {
            configDto.m_token = roomTokenIF.text;
        }
    }

}
