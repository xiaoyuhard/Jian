using RTS;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class DataDisplayUI : MonoSingletonBase<DataDisplayUI>
{
    public  DataManager dataManager;
    public  FoodManager foodManager;


    void Awake()
    {

        //dataManager.LoadAllFromJson(); // ����ʱ��������
        //foodManager.InitializeData(); // ����ʱ��������

    }

  
}
