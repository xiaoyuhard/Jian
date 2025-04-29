using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;
using RTS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XCharts;

/// <summary>
/// 膳食最后出现的图像分析
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
    /// 进行显示图像
    /// </summary>
    public void SetPieChat(EveryMealEnergy totalEnergy, EveryMealEnergy recEnergyIntake, CompareResult compareResult)
    {
        float proteinAll = float.Parse(totalEnergy.protein) * 4;
        float fatAll = float.Parse(totalEnergy.fat) * 9;
        float choAll = float.Parse(totalEnergy.cho) * 4;
        float allEnverMeal = proteinAll + fatAll + choAll;

        PieChart pieChart = pieUI.GetComponent<PieChart>();//获取饼状图
        //pieChart.RefreshChart();//进行刷新
        var serie = pieChart.series.GetSerie(0);//获取series下的第一个
        serie.data[0].UpdateData(1, BackEnergyIntake(allEnverMeal, choAll));//进行修改data data【0】是第一个 updatedata（1，30）第一个element0的第一个数据的值
        textList[0].text = BackEnergyIntake(allEnverMeal, choAll).ToString() + "%";
        serie.data[1].UpdateData(1, BackEnergyIntake(allEnverMeal, proteinAll));
        textList[1].text = BackEnergyIntake(allEnverMeal, choAll).ToString() + "%";
        serie.data[2].UpdateData(1, BackEnergyIntake(allEnverMeal, fatAll));
        textList[2].text = BackEnergyIntake(allEnverMeal, choAll).ToString() + "%";

        pieChart.RefreshChart();//修改完成后进行刷新


        RadarChart radarChart = radarUI.GetComponent<RadarChart>();//获取雷达图
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
