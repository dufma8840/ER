using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PluginManager : MonoBehaviour
{
    public static PluginManager instance;
#if UNITY_ANDROID
    AndroidJavaObject m_AndroidJavaObject;
    AndroidJavaObject m_ActivityInstance;
#endif


    void ShowToastMsg(string msg, int time)
    {
#if UNITY_STANDALONE_WIN

#elif UNITY_ANDROID
        if (m_AndroidJavaObject != null)
        {
            m_AndroidJavaObject.Call("ShowToastMsg", m_ActivityInstance, msg, time);
        }
        else
        {
            Debug.LogError("PluginManager.AndroidJavaObejct is null - ShowToastMsg!");
        }
#else
#endif
    }

    public void ShowExitPopup(string msg, string tiltle)
    {
#if UNITY_STANDALONE_WIN
        WinfromPlugin.Plugin.MessageMsg(msg, tiltle, () => { Application.Quit(); });
#elif UNITY_ANDROID
        if (m_AndroidJavaObject != null)
        {
            m_AndroidJavaObject.Call("ShowDialogExit", m_ActivityInstance, msg, tiltle);
        }
        else
        {
            Debug.LogError("PluginManager.AndroidJavaObejct is null - ShowToastMsg!");
        }
#else

#endif
    }

    // Start is called before the first frame update
    void Start()
    {
#if UNITY_STANDALONE_WIN

#elif UNITY_ANDROID
        using (AndroidJavaObject unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            //m_ActivityInstance = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");
        }

        m_AndroidJavaObject = new AndroidJavaObject("com.sbsgame.androidplugin.Plugin");
        if (m_AndroidJavaObject != null)
            Debug.LogWarning("PluginManager.AndroidJavaObejct:" + m_AndroidJavaObject);
        else
            Debug.LogError("PluginManager.AndroidJavaObejct is null!");
#else
#endif
    }

    void OnGUI()
    {
#if UNITY_STANDALONE_WIN
        //if (GUI.Button(new Rect(0, 100, 300, 30), "종료"))
        //{
        //    WinfromPlugin.Plugin.MessageMsg("종료하시겠습니까", "종료", () => { Application.Quit(); });
        //}
#endif
    }
}
