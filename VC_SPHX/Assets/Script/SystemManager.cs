using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SystemMode
{
    Main,Experiment1, Experiment2, Experiment3, Experiment4, Experiment5, Experiment6, Experiment7, Experiment8, Experiment9, Experiment10
}

public class SystemManager : MonoBehaviour
{

    public static SystemMode systemMode;
    private void Awake()
    {
        GameObject.DontDestroyOnLoad(this);
    }
}
