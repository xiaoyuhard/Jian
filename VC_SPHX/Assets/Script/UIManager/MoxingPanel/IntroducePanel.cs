using RTS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroducePanel : MonoSingletonBase<IntroducePanel>
{
    public Text introduce;
    public Text introduceName;
    public Image picture;

    public void SetInformation(string name, string spriteNamw,string intName)
    {
        introduce.text = name;
        introduceName.text = intName;
        picture.sprite = Resources.Load<Sprite>("Icons/" + spriteNamw);
    }

}
