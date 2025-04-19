using RTS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjMouseClick : MonoSingletonBase<ObjMouseClick>
{
    [SerializeField] private LayerMask doorLayer; // 指定门的层级


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {

                switch (hit.transform.name)
                {
                    case "Cabinet":
                        MessageCenter.Instance.Send("SendMouseToCabinet", ""); //更衣室柜子
                        break;
                    case "AnjisuanPing":
                        MessageCenter.Instance.Send("SendMouseToAnjisuanPing", ""); //氨基酸瓶
                        break;
                    case "AnjisuanZhen":
                        MessageCenter.Instance.Send("SendMouseToAnjisuanZhen", ""); //氨基酸瓶
                        break;
                    case "AnjisuanRongye":
                        MessageCenter.Instance.Send("SendMouseToAnjisuanBiaoZhun", ""); //氨基酸瓶
                        break;
                    case "Workbench":
                        MessageCenter.Instance.Send("SendMouseToWorkbench", ""); //氨基酸工作台
                        break;
                    case "AnjisuanChaoSheng":
                        MessageCenter.Instance.Send("SendMouseToAnjisuanChaoSheng", ""); //氨基酸超声脱气机
                        break;
                    case "AnjisuanComputer":
                        MessageCenter.Instance.Send("SendMouseToAnjisuanComputer", ""); //氨基酸电脑
                        break;
                    case "SM_实验室气瓶柜001 (5)":
                        MessageCenter.Instance.Send("SendMouseToXiangQiGui", ""); //香气柜
                        break;
                    case "XiangqiQipingNiu":
                        MessageCenter.Instance.Send("SendMouseToXiangQiNiu", ""); //香气扭
                        break;
                    case "XiangqiQipingFa":
                        MessageCenter.Instance.Send("SendMouseToXiangQiFa", ""); //香气阀
                        break;
                    case "XiangqiQipingRongye":
                        MessageCenter.Instance.Send("SendMouseToXiangQiRongye", ""); //香气溶液
                        break;
                    case "XiangqiQipingCuiquyi":
                        MessageCenter.Instance.Send("SendMouseToXiangQiCuiquyi", ""); //香气溶液
                        break;
                    default:
                        break;
                }
            }

        }
    }

}
