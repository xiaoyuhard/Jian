using RTS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaoGaoDataCon : MonoSingletonBase<BaoGaoDataCon>
{

    public List<ReportData> reportDatas = new List<ReportData>();
    // Start is called before the first frame update
    void Start()
    {

        for (int i = 0; i < 10; i++)
        {
            ReportData reportData = new ReportData();
            reportData.id = i.ToString();
            reportData.serialT1 = "";
            reportData.serialT2 = "";
            reportData.operationT1 = "";
            reportData.operationT2 = "";
            reportData.markT1 = "";
            reportData.markT2 = "";
            switch (i)
            {
                case 0:
                    reportData.titleT = "一、食品中氨基酸含量的检测 (100分)";
                    break;
                case 1:
                    reportData.titleT = "二、食品中香气成分分析 (100分)";

                    break;
                case 2:
                    reportData.titleT = "三、农产品重金属含量检测 (100分)";

                    break;
                case 3:
                    reportData.titleT = "四、农产品有机磷杀虫剂残留检测 (100分)";

                    break;
                case 4:
                    reportData.titleT = "五、测定食品中还原糖含量 (100分)";

                    break;
                case 5:
                    reportData.titleT = "六、还原索氏提取法测定脂肪含量检测 (100分)";

                    break;
                case 6:
                    reportData.titleT = "七、还原蛋白质含量测定 (100分)";

                    break;
                case 7:
                    reportData.titleT = "八、膳食分析与营养配餐 (100分)";

                    break;
                case 8:
                    reportData.titleT = "九、个人营养配餐 (100分)";

                    break;
                case 9:
                    reportData.titleT = "十、人体数字解剖 (100分)";

                    break;
                default:
                    break;
            }
            reportDatas.Add(reportData);
        }
    }
    // Update is called once per frame
    void Update()
    {

    }

    public void SetReportData(string id, string name, string data)
    {
        foreach (ReportData reportData in reportDatas)
        {
            if (reportData.id == id)
            {
                switch (name)
                {
                    case "serialT1":
                        reportData.serialT1 = data;
                        break;
                    case "serialT2":
                        reportData.serialT2 = data;
                        break;
                    case "operationT1":
                        reportData.operationT1 = data;
                        break;
                    case "operationT2":
                        reportData.operationT2 = data;
                        break;
                    case "markT1":
                        reportData.markT1 = data;
                        break;
                    case "markT2":
                        reportData.markT2 = data;
                        break;
                    default:
                        break;
                }
            }

        }
    }




}
public class ReportData
{
    public string id;
    public string titleT;
    public string serialT1;
    public string serialT2;
    public string operationT1;
    public string operationT2;
    public string markT1;
    public string markT2;
}
