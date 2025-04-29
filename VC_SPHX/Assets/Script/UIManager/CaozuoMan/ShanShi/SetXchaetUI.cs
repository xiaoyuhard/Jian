using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;
using RTS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XCharts;

/// <summary>
/// ��ʳ�����ֵ�ͼ�����
/// </summary>
public class SetXchaetUI : MonoSingletonBase<SetXchaetUI>
{
    public GameObject pieUI;
    public GameObject radarUI;

    public List<Text> textList;
    // Start is called before the first frame update
    void Start()
    {
        //RadarChart radarChart = gameObject.AddComponent<RadarChart>();
        //radarChart.SetSize(580, 300);
        //radarChart.ClearData();
        ////for (int i = 0; i < 7; i++)
        ////{
        ////    radarChart.AddSerie("ss" + i);
        ////    radarChart.AddData(i, Random.Range(1, 5));
        ////}
        //radarChart.AddSerie("ss" + 4);
        //radarChart.AddData(5, Random.Range(1, 5));
        //radarChart.AnimationReset();
        //SetPieChat();
    }

    /// <summary>
    /// ������ʾͼ��
    /// </summary>
    public void SetPieChat(EveryMealEnergy totalEnergy, EveryMealEnergy recEnergyIntake, CompareResult compareResult)
    {
        float proteinAll = float.Parse(totalEnergy.protein) * 4;
        float fatAll = float.Parse(totalEnergy.fat) * 9;
        float choAll = float.Parse(totalEnergy.cho) * 4;
        float allEnverMeal = proteinAll + fatAll + choAll;

        PieChart pieChart = pieUI.GetComponent<PieChart>();//��ȡ��״ͼ
        //pieChart.RefreshChart();//����ˢ��
        var serie = pieChart.series.GetSerie(0);//��ȡseries�µĵ�һ��
        serie.data[0].UpdateData(1, BackEnergyIntake(allEnverMeal, choAll));//�����޸�data data��0���ǵ�һ�� updatedata��1��30����һ��element0�ĵ�һ�����ݵ�ֵ
        textList[0].text = BackEnergyIntake(allEnverMeal, choAll).ToString() + "%";
        serie.data[1].UpdateData(1, BackEnergyIntake(allEnverMeal, proteinAll));
        textList[1].text = BackEnergyIntake(allEnverMeal, choAll).ToString() + "%";
        serie.data[2].UpdateData(1, BackEnergyIntake(allEnverMeal, fatAll));
        textList[2].text = BackEnergyIntake(allEnverMeal, choAll).ToString() + "%";

        pieChart.RefreshChart();//�޸���ɺ����ˢ��


        RadarChart radarChart = radarUI.GetComponent<RadarChart>();//��ȡ�״�ͼ
        //radarChart.RefreshChart();
        var radar1 = radarChart.series.GetSerie(0);
        radar1.data[0].UpdateData(0, RecEnergyIntakeSplit(recEnergyIntake.totalEnergyKcal, 0));
        radar1.data[0].UpdateData(1, RecEnergyIntakeSplit(recEnergyIntake.protein, 0));
        radar1.data[0].UpdateData(2, RecEnergyIntakeSplit(recEnergyIntake.fat, 0));
        radar1.data[0].UpdateData(3, RecEnergyIntakeSplit(recEnergyIntake.cho, 0));
        var radar2 = radarChart.series.GetSerie(1);
        radar2.data[0].UpdateData(0, RecEnergyIntakeSplit(recEnergyIntake.totalEnergyKcal, 1));
        radar2.data[0].UpdateData(1, RecEnergyIntakeSplit(recEnergyIntake.protein, 1));
        radar2.data[0].UpdateData(2, RecEnergyIntakeSplit(recEnergyIntake.fat, 1));
        radar2.data[0].UpdateData(3, RecEnergyIntakeSplit(recEnergyIntake.cho, 1));
        var radar3 = radarChart.series.GetSerie(2);
        radar3.data[0].UpdateData(0, float.Parse(totalEnergy.totalEnergyKcal));
        radar3.data[0].UpdateData(1, float.Parse(totalEnergy.protein));
        radar3.data[0].UpdateData(2, float.Parse(totalEnergy.fat));
        radar3.data[0].UpdateData(3, float.Parse(totalEnergy.cho));
        radarChart.RefreshChart();

    }

    public float BackEnergyIntake(float allEnverMeal, float every)
    {
        return Mathf.Round(((every / allEnverMeal) * 100) / 100) * 100;
    }

    public float RecEnergyIntakeSplit(string recEnergyIntake, int index)
    {
        char separator = '~';
        string[] result = recEnergyIntake.Split(separator);

        return float.Parse(result[index]);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
