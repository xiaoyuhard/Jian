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
                    reportData.titleT = "һ��ʳƷ�а����Ậ���ļ�� (100��)";
                    break;
                case 1:
                    reportData.titleT = "����ʳƷ�������ɷַ��� (100��)";

                    break;
                case 2:
                    reportData.titleT = "����ũ��Ʒ�ؽ���������� (100��)";

                    break;
                case 3:
                    reportData.titleT = "�ġ�ũ��Ʒ�л���ɱ���������� (100��)";

                    break;
                case 4:
                    reportData.titleT = "�塢�ⶨʳƷ�л�ԭ�Ǻ��� (100��)";

                    break;
                case 5:
                    reportData.titleT = "������ԭ������ȡ���ⶨ֬��������� (100��)";

                    break;
                case 6:
                    reportData.titleT = "�ߡ���ԭ�����ʺ����ⶨ (100��)";

                    break;
                case 7:
                    reportData.titleT = "�ˡ���ʳ������Ӫ����� (100��)";

                    break;
                case 8:
                    reportData.titleT = "�š�����Ӫ����� (100��)";

                    break;
                case 9:
                    reportData.titleT = "ʮ���������ֽ��� (100��)";

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
