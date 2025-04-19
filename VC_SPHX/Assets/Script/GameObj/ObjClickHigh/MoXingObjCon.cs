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
            obj.SetActive(false);
            if (obj.name == objName)
            {
                obj.SetActive(true);
            }
        }
    }
    private void OnEnable()
    {
        foreach (GameObject obj in objects)
        {
            obj.SetActive(false);

        }
    }
}
