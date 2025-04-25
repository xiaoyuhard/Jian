using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreeMenu : MonoBehaviour
{
    public Button[] btns;
    ScrollRect scrollRect;
    public LayoutGroup layout;

    // Start is called before the first frame update
    void Start()
    {

        for (int i = 0; i < btns.Length; i++)
        {
            var idx = i;

            btns[i].onClick.AddListener(() => {
                print(btns[idx]);
                var scroll = btns[idx].transform.Find("Scroll View");

                if(scroll)
                {
                    scroll.gameObject.SetActive(!scroll.gameObject.activeSelf);
                    layout.CalculateLayoutInputVertical();
                }
            });
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
