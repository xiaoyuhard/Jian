using RTS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjMouseClick : MonoSingletonBase<ObjMouseClick>
{
    [SerializeField] private LayerMask doorLayer; // ָ���ŵĲ㼶


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
                        MessageCenter.Instance.Send("SendMouseToCabinet", ""); //�����ҹ���
                        break;
                    case "AnjisuanPing":
                        MessageCenter.Instance.Send("SendMouseToAnjisuanPing", ""); //������ƿ
                        break;
                    case "AnjisuanZhen":
                        MessageCenter.Instance.Send("SendMouseToAnjisuanZhen", ""); //������ƿ
                        break;
                    case "AnjisuanRongye":
                        MessageCenter.Instance.Send("SendMouseToAnjisuanBiaoZhun", ""); //������ƿ
                        break;
                    case "Workbench":
                        MessageCenter.Instance.Send("SendMouseToWorkbench", ""); //�����Ṥ��̨
                        break;
                    case "AnjisuanChaoSheng":
                        MessageCenter.Instance.Send("SendMouseToAnjisuanChaoSheng", ""); //�����ᳬ��������
                        break;
                    case "AnjisuanComputer":
                        MessageCenter.Instance.Send("SendMouseToAnjisuanComputer", ""); //���������
                        break;
                    case "SM_ʵ������ƿ��001 (5)":
                        MessageCenter.Instance.Send("SendMouseToXiangQiGui", ""); //������
                        break;
                    case "XiangqiQipingNiu":
                        MessageCenter.Instance.Send("SendMouseToXiangQiNiu", ""); //����Ť
                        break;
                    case "XiangqiQipingFa":
                        MessageCenter.Instance.Send("SendMouseToXiangQiFa", ""); //������
                        break;
                    case "XiangqiQipingRongye":
                        MessageCenter.Instance.Send("SendMouseToXiangQiRongye", ""); //������Һ
                        break;
                    case "XiangqiQipingCuiquyi":
                        MessageCenter.Instance.Send("SendMouseToXiangQiCuiquyi", ""); //������Һ
                        break;
                    default:
                        break;
                }
            }

        }
    }

}
