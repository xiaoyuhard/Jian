using RTS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CaozuoSceneCon : MonoSingletonBase<CaozuoSceneCon>
{
    public List<GameObject> sceneList;

    public void OpenObj(int index)
    {
        sceneList[index].SetActive(true);
    }

    public void CloseObj()
    {
        foreach (var item in sceneList)
        {
            item.SetActive(false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
