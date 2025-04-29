using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMgr
{
    /// <summary>
    /// 当前场景名字
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

    public static Scene GetCurrentScene()
    {
        return SceneManager.GetActiveScene();
    }
}