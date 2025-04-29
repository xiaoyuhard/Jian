using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMgr
{
    /// <summary>
    /// ��ǰ��������
    /// </summary>
    public static string CurSceneName
    {
        get
        {
            var scene = GetCurrentScene();
            return scene.name;
        }
    }

    /// <summary>
    /// ���س���
    /// </summary>
    /// <param name="name">��������</param>
    public static void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    public static void LoadSceneAsync(string name)
    {
        SceneManager.LoadSceneAsync(name);
    }

    public static Scene GetCurrentScene()
    {
        return SceneManager.GetActiveScene();
    }
}