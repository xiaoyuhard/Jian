using RTS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoXingObjCon : MonoSingletonBase<MoXingObjCon>
{
    public List<GameObject> objects;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowObj(string objName)
    {
        foreach (GameObject obj in objects)
        {
            string name = GetMiddleChars(obj.name);
            obj.SetActive(false);
            if (objName.Trim().Equals(name))
            {
                Debug.Log(obj.name + "   objName:   " + objName);

                obj.SetActive(true);
            }
        }
    }

    // 示例：提取第3到最后字符（索引从0开始）
    string GetMiddleChars(string objName)
    {
        //Debug.Log(objName.Substring(3) + "sssss");
        return objName.Substring(3);

    }

    private void OnEnable()
    {
        foreach (GameObject obj in objects)
        {
            obj.SetActive(false);

        }
    }
}
