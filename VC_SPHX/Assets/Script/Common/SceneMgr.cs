using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMgr
{
    /// <summary>
    /// 加载场景
    /// </summary>
    /// <param name="name">场景名字</param>
    public static void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    public static void LoadSceneAsync(string name)
    {
        SceneManager.LoadSceneAsync(name);
    }
}
