using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;
using RTS;
using System;
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
    public GameObject pieUI;            //全天优质蛋白占比饼图 
    public GameObject radarUI;          //全天营养合计分析雷达图
    public GameObject bar1;              //三餐热量比例柱形图
    public GameObject bar2;              //膳食纤维与蛋白质摄入达标分析柱形图
    public GameObject bar3;              //全天三大营养素比例柱形图
    public GameObject summaryUI;        //总结UI
    public List<Text> tipList;

    public List<Text> textList;
    public Button closeBtn;
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
        closeBtn.onClick.AddListener(CloseXchaetUI);
    }

    private void CloseXchaetUI()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 进行显示图像
    /// </summary>
    public void SetPieChat(Dictionary<string, List<FoodKindItemData>> allFoodDic, string score, string foodNum, PromptInfo promptInfo, EveryMealEnergy totalEnergy, EveryMealEnergy recEnergyIntake, CompareResult compareResult, List<EveryMealEnergy> everyMealEnergies, FiberAndFineProtein fiberAndFineProtein, ThreeMeals user, UserInfo userInfo)
    {
        //float proteinAll = float.Parse(totalEnergy.protein) * 4;
        //float fatAll = float.Parse(totalEnergy.fat) * 9;
        //float choAll = float.Parse(totalEnergy.cho) * 4;
        //float allEnverMeal = proteinAll + fatAll + choAll;
        foreach (var item in tipList)
        {
            item.text = "";
        }
        if (userInfo.isBaby)
        {
            pieUI.SetActive(false);
            bar2.SetActive(false);
            //summaryUI.SetActive(false);
        }
        else
        {
            pieUI.SetActive(true);
            bar2.SetActive(true);
            //summaryUI.SetActive(true);
        }

        PieChart pieChart = pieUI.GetComponent<PieChart>();//获取饼状图


        for (int i = 0; i < promptInfo.fineProteinMessage.Count; i++)
        {
            tipList[0].text += promptInfo.fineProteinMessage[i] + "\n";

        }

        //pieChart.RefreshChart();//进行刷新
        //var serie = pieChart.series.GetSerie(0);//获取series下的第一个
        //serie.data[0].UpdateData(1, BackEnergyIntake(allEnverMeal, choAll));//进行修改data data【0】是第一个 updatedata（1，30）第一个element0的第一个数据的值
        //textList[0].text = BackEnergyIntake(allEnverMeal, choAll).ToString() + "%";
        //serie.data[1].UpdateData(1, BackEnergyIntake(allEnverMeal, proteinAll));
        //textList[1].text = BackEnergyIntake(allEnverMeal, proteinAll).ToString() + "%";
        //serie.data[2].UpdateData(1, BackEnergyIntake(allEnverMeal, fatAll));
        //textList[2].text = BackEnergyIntake(allEnverMeal, fatAll).ToString() + "%";

        foreach (var item in allFoodDic.Values)
        {
            float proteinAll = 0;
            float fatAll = 0;
            float choAll = 0;
            float allEnverMeal = 0;
            int index = 0;
            foreach (var food in item)
            {
                proteinAll += float.Parse(food.protein);
                fatAll += float.Parse(food.fat);
                choAll += float.Parse(food.cho);
            }
            proteinAll = proteinAll * 4;
            fatAll = fatAll * 9;
            choAll = choAll * 4;
            allEnverMeal = proteinAll + fatAll + choAll;
            var serie = pieChart.series.GetSerie(0);//获取series下的第一个
            serie.data[index].UpdateData(1, BackEnergyIntake(allEnverMeal, proteinAll));//进行修改data data【0】是第一个 updatedata（1，30）第一个element0的第一个数据的值
            textList[index].text = BackEnergyIntake(allEnverMeal, proteinAll).ToString() + "%";

            index++;
            if (index == 5)
            {
                break;
            }
        }

        pieChart.RefreshChart();//修改完成后进行刷新

        //全天计划
        float totalTodayProteinPlan = float.Parse(user.data.breakfastPlan.protein) + float.Parse(user.data.lunchPlan.protein) + float.Parse(user.data.dinnerPlan.protein);
        float totalTodayFatPlan = float.Parse(user.data.breakfastPlan.fat) + float.Parse(user.data.lunchPlan.fat) + float.Parse(user.data.dinnerPlan.fat);
        float totalTodayChoPlan = float.Parse(user.data.breakfastPlan.cho) + float.Parse(user.data.lunchPlan.cho) + float.Parse(user.data.dinnerPlan.cho);
        float proteinTodayAllPlan = totalTodayProteinPlan * 4;
        float fatTodayAllPlan = totalTodayFatPlan * 9;
        float choTodayAllPlan = totalTodayChoPlan * 4;
        float allTodayEnverMealPlan = proteinTodayAllPlan + fatTodayAllPlan + choTodayAllPlan;


        RadarChart radarChart = radarUI.GetComponent<RadarChart>();//获取雷达图


        for (int i = 0; i < promptInfo.caloricIntakeMessage.Count; i++)
        {
            tipList[1].text += promptInfo.caloricIntakeMessage[i] + "\n";

        }
        //radarChart.RefreshChart();
        var radar1 = radarChart.series.GetSerie(0);
        radar1.data[0].UpdateData(0, RecEnergyIntakeSplit(recEnergyIntake.totalEnergyKcal, 0));
        radar1.data[0].UpdateData(1, RecEnergyIntakeSplit(recEnergyIntake.protein, 0));
        radar1.data[0].UpdateData(2, RecEnergyIntakeSplit(recEnergyIntake.fat, 0));
        radar1.data[0].UpdateData(3, RecEnergyIntakeSplit(recEnergyIntake.cho, 0));
        var radar2 = radarChart.series.GetSerie(1);
        radar2.data[0].UpdateData(0, RecEnergyIntakeSplit(recEnergyIntake.totalEnergyKcal, 0));
        //radar2.data[0].UpdateData(0, RecEnergyIntakeSplit(recEnergyIntake.totalEnergyKcal, 1));
        radar2.data[0].UpdateData(1, RecEnergyIntakeSplit(recEnergyIntake.protein, 1));
        radar2.data[0].UpdateData(2, RecEnergyIntakeSplit(recEnergyIntake.fat, 1));
        radar2.data[0].UpdateData(3, RecEnergyIntakeSplit(recEnergyIntake.cho, 1));
        var radar3 = radarChart.series.GetSerie(2);
        radar3.data[0].UpdateData(0, Mathf.Round(float.Parse(totalEnergy.totalEnergyKcal) * 100) / 100);
        radar3.data[0].UpdateData(1, Mathf.Round(float.Parse(totalEnergy.protein) * 100) / 100);
        radar3.data[0].UpdateData(2, Mathf.Round(float.Parse(totalEnergy.fat) * 100) / 100);
        radar3.data[0].UpdateData(3, Mathf.Round(float.Parse(totalEnergy.cho) * 100) / 100);
        var radar4 = radarChart.series.GetSerie(3);
        radar4.data[0].UpdateData(0, Mathf.Round(allTodayEnverMealPlan * 100) / 100);
        radar4.data[0].UpdateData(1, Mathf.Round(totalTodayProteinPlan * 100) / 100);
        radar4.data[0].UpdateData(2, Mathf.Round(totalTodayFatPlan * 100) / 100);
        radar4.data[0].UpdateData(3, Mathf.Round(totalTodayChoPlan * 100) / 100);
        radarChart.RefreshChart();


        BarChart barChart1 = bar1.GetComponent<BarChart>();

        for (int i = 0; i < promptInfo.mealRatioMessage.Count; i++)
        {
            tipList[2].text += promptInfo.mealRatioMessage[i] + "\n";

        }

        barChart1.UpdateData(0, 0, Mathf.Round(float.Parse(everyMealEnergies[0].totalEnergyKcal) * 100) / 100);
        barChart1.UpdateData(0, 1, Mathf.Round(float.Parse(everyMealEnergies[1].totalEnergyKcal) * 100) / 100);
        barChart1.UpdateData(0, 2, Mathf.Round(float.Parse(everyMealEnergies[1].totalEnergyKcal) * 100) / 100);
        barChart1.UpdateData(1, 0, Mathf.Round(float.Parse(user.data.breakfastPlan.totalEnergyKcal) * 100) / 100);
        barChart1.UpdateData(1, 1, Mathf.Round(float.Parse(user.data.lunchPlan.totalEnergyKcal) * 100) / 100);
        barChart1.UpdateData(1, 2, Mathf.Round(float.Parse(user.data.dinnerPlan.totalEnergyKcal) * 100) / 100);

        //var bar1Ser1 = barChart1.series.GetSerie(0);
        //bar1Ser1.data[0].UpdateData(0, Mathf.Round(float.Parse(everyMealEnergies[0].totalEnergyKcal) * 100) / 100);
        //bar1Ser1.data[1].UpdateData(0, Mathf.Round(float.Parse(everyMealEnergies[1].totalEnergyKcal) * 100) / 100);
        //bar1Ser1.data[2].UpdateData(0, Mathf.Round(float.Parse(everyMealEnergies[2].totalEnergyKcal) * 100) / 100);
        //var bar1Ser2 = barChart1.series.GetSerie(1);
        //bar1Ser2.data[0].UpdateData(0, Mathf.Round(float.Parse(user.data.breakfastPlan.totalEnergyKcal) * 100) / 100);
        //bar1Ser2.data[1].UpdateData(0, Mathf.Round(float.Parse(user.data.lunchPlan.totalEnergyKcal) * 100) / 100);
        //bar1Ser2.data[2].UpdateData(0, Mathf.Round(float.Parse(user.data.dinnerPlan.totalEnergyKcal) * 100) / 100);
        //barChart1.RefreshChart();
        //Debug.Log(Mathf.Round(float.Parse(everyMealEnergies[0].totalEnergyKcal) * 100) / 100 + "  " + Mathf.Round(float.Parse(everyMealEnergies[1].totalEnergyKcal) * 100) / 100 + "  " + Mathf.Round(float.Parse(everyMealEnergies[2].totalEnergyKcal) * 100) / 100);
        //Debug.Log(everyMealEnergies[0].totalEnergyKcal + "   " + everyMealEnergies[1].totalEnergyKcal + "   " + everyMealEnergies[2].totalEnergyKcal);
        //Debug.Log(Mathf.Round(float.Parse(user.data.breakfastPlan.totalEnergyKcal) * 100) / 100 + "   " + Mathf.Round(float.Parse(user.data.lunchPlan.totalEnergyKcal) * 100) / 100 + "  " + Mathf.Round(float.Parse(user.data.dinnerPlan.totalEnergyKcal) * 100) / 100);
        //Debug.Log(user.data.breakfastPlan.totalEnergyKcal + "   " + user.data.lunchPlan.totalEnergyKcal + "   " + user.data.dinnerPlan.totalEnergyKcal);
        barChart1.RefreshChart();


        BarChart barChart2 = bar2.GetComponent<BarChart>();
        barChart2.UpdateData(0, 0, Mathf.Round(float.Parse(fiberAndFineProtein.plan.fiberIntake) * 100) / 100);
        barChart2.UpdateData(0, 1, Mathf.Round(float.Parse(fiberAndFineProtein.plan.fineProteinIntake) * 100) / 100);
        barChart2.UpdateData(0, 2, Mathf.Round(float.Parse(fiberAndFineProtein.plan.totalProteinIntake) * 100) / 100);
        barChart2.UpdateData(1, 0, Mathf.Round(float.Parse(fiberAndFineProtein.actual.fiberIntake) * 100) / 100);
        barChart2.UpdateData(1, 1, Mathf.Round(float.Parse(fiberAndFineProtein.actual.fineProteinIntake) * 100) / 100);
        barChart2.UpdateData(1, 2, Mathf.Round(float.Parse(fiberAndFineProtein.actual.totalProteinIntake) * 100) / 100);
        barChart2.UpdateData(2, 0, (Mathf.Round(float.Parse(fiberAndFineProtein.plan.totalProteinIntake) * 100) / 100) / 3);
        barChart2.UpdateData(3, 0, (Mathf.Round(float.Parse(fiberAndFineProtein.actual.totalProteinIntake) * 100) / 100) / 3);

        //var bar2Ser1 = barChart2.series.GetSerie(0);
        //bar2Ser1.data[0].UpdateData(0, Mathf.Round(float.Parse(fiberAndFineProtein.plan.fiberIntake) * 100) / 100);
        //bar2Ser1.data[1].UpdateData(0, Mathf.Round(float.Parse(fiberAndFineProtein.plan.fineProteinIntake) * 100) / 100);
        //bar2Ser1.data[2].UpdateData(0, Mathf.Round(float.Parse(fiberAndFineProtein.plan.totalProteinIntake) * 100) / 100);
        //var bar2Ser2 = barChart2.series.GetSerie(1);
        //bar2Ser2.data[0].UpdateData(0, Mathf.Round(float.Parse(fiberAndFineProtein.actual.fiberIntake) * 100) / 100);
        //bar2Ser2.data[1].UpdateData(0, Mathf.Round(float.Parse(fiberAndFineProtein.actual.fineProteinIntake) * 100) / 100);
        //bar2Ser2.data[2].UpdateData(0, Mathf.Round(float.Parse(fiberAndFineProtein.actual.totalProteinIntake) * 100) / 100);
        barChart2.RefreshChart();



        BarChart barChart3 = bar3.GetComponent<BarChart>();

        for (int i = 0; i < promptInfo.energySupplyMessage.Count; i++)
        {
            tipList[3].text += promptInfo.energySupplyMessage[i] + "\n";

        }
        //全天当前
        float proteinTodayAllPresent = float.Parse(totalEnergy.protein) * 4;
        float fatTodayAllPresent = float.Parse(totalEnergy.fat) * 9;
        float choTodayAllPresent = float.Parse(totalEnergy.cho) * 4;
        float allTodayEnverMealPresent = proteinTodayAllPresent + fatTodayAllPresent + choTodayAllPresent;

        //计划早餐
        float proteinBreakPlan = float.Parse(user.data.breakfastPlan.protein) * 4;
        float fatBreakPlan = float.Parse(user.data.breakfastPlan.fat) * 9;
        float choBreakPlan = float.Parse(user.data.breakfastPlan.cho) * 4;
        float allBreakEnverMealPlan = proteinBreakPlan + fatBreakPlan + choBreakPlan;

        //当前早餐
        float proteinBreakPresent = float.Parse(everyMealEnergies[0].protein) * 4;
        float fatBreakPresent = float.Parse(everyMealEnergies[0].fat) * 9;
        float choBreakPresent = float.Parse(everyMealEnergies[0].cho) * 4;
        float allBreakEnverMealPresent = proteinBreakPresent + fatBreakPresent + choBreakPresent;

        //计划午餐
        float proteinLunchPlan = float.Parse(user.data.lunchPlan.protein) * 4;
        float fatLunchPlan = float.Parse(user.data.lunchPlan.fat) * 9;
        float choLunchPlan = float.Parse(user.data.lunchPlan.cho) * 4;
        float allLunchEnverMealPlan = proteinLunchPlan + fatLunchPlan + choLunchPlan;

        //当前午餐
        float proteinLunchPresent = float.Parse(everyMealEnergies[1].protein) * 4;
        float fatLunchPresent = float.Parse(everyMealEnergies[1].fat) * 9;
        float choLunchPresent = float.Parse(everyMealEnergies[1].cho) * 4;
        float allLunchEnverMealPresent = proteinLunchPresent + fatLunchPresent + choLunchPresent;

        //计划晚餐
        float proteinDinnerPlan = float.Parse(user.data.dinnerPlan.protein) * 4;
        float fatDinnerPlan = float.Parse(user.data.dinnerPlan.fat) * 9;
        float choDinnerPlan = float.Parse(user.data.dinnerPlan.cho) * 4;
        float allDinnerEnverMealPlan = proteinDinnerPlan + fatDinnerPlan + choDinnerPlan;

        //当前晚餐
        float proteinDinnerPresent = float.Parse(everyMealEnergies[2].protein) * 4;
        float fatDinnerPresent = float.Parse(everyMealEnergies[2].fat) * 9;
        float choDinnerPresent = float.Parse(everyMealEnergies[2].cho) * 4;
        float allDinnerEnverMealPresent = proteinDinnerPresent + fatDinnerPresent + choDinnerPresent;


        barChart3.UpdateData(0, 0, BackEnergyIntake(allTodayEnverMealPlan, proteinTodayAllPlan));       //全天计划
        barChart3.UpdateData(0, 1, BackEnergyIntake(allTodayEnverMealPresent, proteinTodayAllPresent));    //全天当前
        barChart3.UpdateData(0, 2, BackEnergyIntake(allBreakEnverMealPlan, proteinBreakPlan));       //计划早餐
        barChart3.UpdateData(0, 3, BackEnergyIntake(allBreakEnverMealPresent, proteinBreakPresent));    //当前早餐
        barChart3.UpdateData(0, 4, BackEnergyIntake(allLunchEnverMealPlan, proteinLunchPlan));       //计划午餐
        barChart3.UpdateData(0, 5, BackEnergyIntake(allLunchEnverMealPresent, proteinLunchPresent));    //当前午餐
        barChart3.UpdateData(0, 6, BackEnergyIntake(allDinnerEnverMealPlan, proteinDinnerPlan));      //计划晚餐
        barChart3.UpdateData(0, 7, BackEnergyIntake(allDinnerEnverMealPresent, proteinDinnerPresent));   //当前晚餐

        barChart3.UpdateData(1, 0, BackEnergyIntake(allTodayEnverMealPlan, fatTodayAllPlan));           //全天计划
        barChart3.UpdateData(1, 1, BackEnergyIntake(allTodayEnverMealPresent, fatTodayAllPresent));    //全天当前
        barChart3.UpdateData(1, 2, BackEnergyIntake(allBreakEnverMealPlan, fatBreakPlan));       //计划早餐
        barChart3.UpdateData(1, 3, BackEnergyIntake(allBreakEnverMealPresent, fatBreakPresent));    //当前早餐
        barChart3.UpdateData(1, 4, BackEnergyIntake(allLunchEnverMealPlan, fatLunchPlan));       //计划午餐
        barChart3.UpdateData(1, 5, BackEnergyIntake(allLunchEnverMealPresent, fatLunchPresent));    //当前午餐
        barChart3.UpdateData(1, 6, BackEnergyIntake(allDinnerEnverMealPlan, fatDinnerPlan));      //计划晚餐
        barChart3.UpdateData(1, 7, BackEnergyIntake(allDinnerEnverMealPresent, fatDinnerPresent));   //当前晚餐

        barChart3.UpdateData(2, 0, BackEnergyIntake(allTodayEnverMealPlan, choTodayAllPlan));           //全天计划
        barChart3.UpdateData(2, 1, BackEnergyIntake(allTodayEnverMealPresent, choTodayAllPresent));    //全天当前
        barChart3.UpdateData(2, 2, BackEnergyIntake(allBreakEnverMealPlan, choBreakPlan));       //计划早餐
        barChart3.UpdateData(2, 3, BackEnergyIntake(allBreakEnverMealPresent, choBreakPresent));    //当前早餐
        barChart3.UpdateData(2, 4, BackEnergyIntake(allLunchEnverMealPlan, choLunchPlan));       //计划午餐
        barChart3.UpdateData(2, 5, BackEnergyIntake(allLunchEnverMealPresent, choLunchPresent));    //当前午餐
        barChart3.UpdateData(2, 6, BackEnergyIntake(allDinnerEnverMealPlan, choDinnerPlan));      //计划晚餐
        barChart3.UpdateData(2, 7, BackEnergyIntake(allDinnerEnverMealPresent, choDinnerPresent));   //当前晚餐

        //var bar3Ser1 = barChart3.series.GetSerie(0);

        //bar3Ser1.data[0].UpdateData(0, BackEnergyIntake(allTodayEnverMealPlan, proteinTodayAllPlan));       //全天计划
        //bar3Ser1.data[1].UpdateData(0, BackEnergyIntake(allTodayEnverMealPresent, proteinTodayAllPresent));    //全天当前
        //bar3Ser1.data[2].UpdateData(0, BackEnergyIntake(allBreakEnverMealPlan, proteinBreakPlan));       //计划早餐
        //bar3Ser1.data[3].UpdateData(0, BackEnergyIntake(allBreakEnverMealPresent, proteinBreakPresent));    //当前早餐
        //bar3Ser1.data[4].UpdateData(0, BackEnergyIntake(allLunchEnverMealPlan, proteinLunchPlan));       //计划午餐
        //bar3Ser1.data[5].UpdateData(0, BackEnergyIntake(allLunchEnverMealPresent, proteinLunchPresent));    //当前午餐
        //bar3Ser1.data[6].UpdateData(0, BackEnergyIntake(allDinnerEnverMealPlan, proteinDinnerPlan));      //计划晚餐
        //bar3Ser1.data[7].UpdateData(0, BackEnergyIntake(allDinnerEnverMealPresent, proteinDinnerPresent));   //当前晚餐
        //var bar3Ser2 = barChart3.series.GetSerie(1);
        //bar3Ser2.data[0].UpdateData(0, BackEnergyIntake(allTodayEnverMealPlan, fatTodayAllPlan));           //全天计划
        //bar3Ser2.data[1].UpdateData(0, BackEnergyIntake(allTodayEnverMealPresent, fatTodayAllPresent));    //全天当前
        //bar3Ser2.data[2].UpdateData(0, BackEnergyIntake(allBreakEnverMealPlan, fatBreakPlan));       //计划早餐
        //bar3Ser1.data[3].UpdateData(0, BackEnergyIntake(allBreakEnverMealPresent, fatBreakPresent));    //当前早餐
        //bar3Ser1.data[4].UpdateData(0, BackEnergyIntake(allLunchEnverMealPlan, fatLunchPlan));       //计划午餐
        //bar3Ser1.data[5].UpdateData(0, BackEnergyIntake(allLunchEnverMealPresent, fatLunchPresent));    //当前午餐
        //bar3Ser1.data[6].UpdateData(0, BackEnergyIntake(allDinnerEnverMealPlan, fatDinnerPlan));      //计划晚餐
        //bar3Ser1.data[7].UpdateData(0, BackEnergyIntake(allDinnerEnverMealPresent, fatDinnerPresent));   //当前晚餐
        //var bar3Ser3 = barChart3.series.GetSerie(2);
        //bar3Ser3.data[0].UpdateData(0, BackEnergyIntake(allTodayEnverMealPlan, choTodayAllPlan));           //全天计划
        //bar3Ser3.data[1].UpdateData(0, BackEnergyIntake(allTodayEnverMealPresent, choTodayAllPresent));    //全天当前
        //bar3Ser3.data[2].UpdateData(0, BackEnergyIntake(allBreakEnverMealPlan, choBreakPlan));       //计划早餐
        //bar3Ser1.data[3].UpdateData(0, BackEnergyIntake(allBreakEnverMealPresent, choBreakPresent));    //当前早餐
        //bar3Ser1.data[4].UpdateData(0, BackEnergyIntake(allLunchEnverMealPlan, choLunchPlan));       //计划午餐
        //bar3Ser1.data[5].UpdateData(0, BackEnergyIntake(allLunchEnverMealPresent, choLunchPresent));    //当前午餐
        //bar3Ser1.data[6].UpdateData(0, BackEnergyIntake(allDinnerEnverMealPlan, choDinnerPlan));      //计划晚餐
        //bar3Ser1.data[7].UpdateData(0, BackEnergyIntake(allDinnerEnverMealPresent, choDinnerPresent));   //当前晚餐
        barChart3.RefreshChart();

        summaryUI.transform.Find("Score").GetComponent<Text>().text = score;
        summaryUI.transform.Find("FoodNum").GetComponent<Text>().text = foodNum;


    }

    /// <summary>
    /// 进行显示图像 群体单天
    /// </summary>
    public void SetPieChatGroupDay(Dictionary<string, List<FoodRecipeGroupItem>> allFoodDic, string score, string foodNum, PromptInfo promptInfo, EveryMealEnergy totalEnergy, EveryMealEnergy recEnergyIntake, CompareResult compareResult, List<EveryMealEnergy> everyMealEnergies, FiberAndFineProtein fiberAndFineProtein, ThreeMeals user, UserInfo userInfo)
    {

        foreach (var item in tipList)
        {
            item.text = "";
        }
        if (userInfo.isBaby)
        {
            pieUI.SetActive(false);
            bar2.SetActive(false);
            //summaryUI.SetActive(false);
        }
        else
        {
            pieUI.SetActive(true);
            bar2.SetActive(true);
            //summaryUI.SetActive(true);
        }

        PieChart pieChart = pieUI.GetComponent<PieChart>();//获取饼状图


        for (int i = 0; i < promptInfo.fineProteinMessage.Count; i++)
        {
            tipList[0].text += promptInfo.fineProteinMessage[i] + "\n";

        }

        foreach (var item in allFoodDic.Values)
        {
            float proteinAll = 0;
            float fatAll = 0;
            float choAll = 0;
            float allEnverMeal = 0;
            int index = 0;
            foreach (var food in item)
            {
                proteinAll += float.Parse(food.protein);
                fatAll += float.Parse(food.fat);
                choAll += float.Parse(food.cho);
            }
            proteinAll = proteinAll * 4;
            fatAll = fatAll * 9;
            choAll = choAll * 4;
            allEnverMeal = proteinAll + fatAll + choAll;
            var serie = pieChart.series.GetSerie(0);//获取series下的第一个
            serie.data[index].UpdateData(1, BackEnergyIntake(allEnverMeal, proteinAll));//进行修改data data【0】是第一个 updatedata（1，30）第一个element0的第一个数据的值
            textList[index].text = BackEnergyIntake(allEnverMeal, proteinAll).ToString() + "%";

            index++;
            if (index == 5)
            {
                break;
            }
        }

        pieChart.RefreshChart();//修改完成后进行刷新

        //全天计划
        float totalTodayProteinPlan = float.Parse(user.data.breakfastPlan.protein) + float.Parse(user.data.lunchPlan.protein) + float.Parse(user.data.dinnerPlan.protein);
        float totalTodayFatPlan = float.Parse(user.data.breakfastPlan.fat) + float.Parse(user.data.lunchPlan.fat) + float.Parse(user.data.dinnerPlan.fat);
        float totalTodayChoPlan = float.Parse(user.data.breakfastPlan.cho) + float.Parse(user.data.lunchPlan.cho) + float.Parse(user.data.dinnerPlan.cho);
        float proteinTodayAllPlan = totalTodayProteinPlan * 4;
        float fatTodayAllPlan = totalTodayFatPlan * 9;
        float choTodayAllPlan = totalTodayChoPlan * 4;
        float allTodayEnverMealPlan = proteinTodayAllPlan + fatTodayAllPlan + choTodayAllPlan;


        RadarChart radarChart = radarUI.GetComponent<RadarChart>();//获取雷达图


        for (int i = 0; i < promptInfo.caloricIntakeMessage.Count; i++)
        {
            tipList[1].text += promptInfo.caloricIntakeMessage[i] + "\n";

        }
        //radarChart.RefreshChart();
        var radar1 = radarChart.series.GetSerie(0);
        radar1.data[0].UpdateData(0, RecEnergyIntakeSplit(recEnergyIntake.totalEnergyKcal, 0));
        radar1.data[0].UpdateData(1, RecEnergyIntakeSplit(recEnergyIntake.protein, 0));
        radar1.data[0].UpdateData(2, RecEnergyIntakeSplit(recEnergyIntake.fat, 0));
        radar1.data[0].UpdateData(3, RecEnergyIntakeSplit(recEnergyIntake.cho, 0));
        var radar2 = radarChart.series.GetSerie(1);
        radar2.data[0].UpdateData(0, RecEnergyIntakeSplit(recEnergyIntake.totalEnergyKcal, 0));
        //radar2.data[0].UpdateData(0, RecEnergyIntakeSplit(recEnergyIntake.totalEnergyKcal, 1));
        radar2.data[0].UpdateData(1, RecEnergyIntakeSplit(recEnergyIntake.protein, 1));
        radar2.data[0].UpdateData(2, RecEnergyIntakeSplit(recEnergyIntake.fat, 1));
        radar2.data[0].UpdateData(3, RecEnergyIntakeSplit(recEnergyIntake.cho, 1));
        var radar3 = radarChart.series.GetSerie(2);
        radar3.data[0].UpdateData(0, Mathf.Round(float.Parse(totalEnergy.totalEnergyKcal) * 100) / 100);
        radar3.data[0].UpdateData(1, Mathf.Round(float.Parse(totalEnergy.protein) * 100) / 100);
        radar3.data[0].UpdateData(2, Mathf.Round(float.Parse(totalEnergy.fat) * 100) / 100);
        radar3.data[0].UpdateData(3, Mathf.Round(float.Parse(totalEnergy.cho) * 100) / 100);
        var radar4 = radarChart.series.GetSerie(3);
        radar4.data[0].UpdateData(0, Mathf.Round(allTodayEnverMealPlan * 100) / 100);
        radar4.data[0].UpdateData(1, Mathf.Round(totalTodayProteinPlan * 100) / 100);
        radar4.data[0].UpdateData(2, Mathf.Round(totalTodayFatPlan * 100) / 100);
        radar4.data[0].UpdateData(3, Mathf.Round(totalTodayChoPlan * 100) / 100);
        radarChart.RefreshChart();


        BarChart barChart1 = bar1.GetComponent<BarChart>();

        for (int i = 0; i < promptInfo.mealRatioMessage.Count; i++)
        {
            tipList[2].text += promptInfo.mealRatioMessage[i] + "\n";

        }

        barChart1.UpdateData(0, 0, Mathf.Round(float.Parse(everyMealEnergies[0].totalEnergyKcal) * 100) / 100);
        barChart1.UpdateData(0, 1, Mathf.Round(float.Parse(everyMealEnergies[1].totalEnergyKcal) * 100) / 100);
        barChart1.UpdateData(0, 2, Mathf.Round(float.Parse(everyMealEnergies[1].totalEnergyKcal) * 100) / 100);
        barChart1.UpdateData(1, 0, Mathf.Round(float.Parse(user.data.breakfastPlan.totalEnergyKcal) * 100) / 100);
        barChart1.UpdateData(1, 1, Mathf.Round(float.Parse(user.data.lunchPlan.totalEnergyKcal) * 100) / 100);
        barChart1.UpdateData(1, 2, Mathf.Round(float.Parse(user.data.dinnerPlan.totalEnergyKcal) * 100) / 100);

        barChart1.RefreshChart();


        BarChart barChart2 = bar2.GetComponent<BarChart>();
        barChart2.UpdateData(0, 0, Mathf.Round(float.Parse(fiberAndFineProtein.plan.fiberIntake) * 100) / 100);
        barChart2.UpdateData(0, 1, Mathf.Round(float.Parse(fiberAndFineProtein.plan.fineProteinIntake) * 100) / 100);
        barChart2.UpdateData(0, 2, Mathf.Round(float.Parse(fiberAndFineProtein.plan.totalProteinIntake) * 100) / 100);
        barChart2.UpdateData(1, 0, Mathf.Round(float.Parse(fiberAndFineProtein.actual.fiberIntake) * 100) / 100);
        barChart2.UpdateData(1, 1, Mathf.Round(float.Parse(fiberAndFineProtein.actual.fineProteinIntake) * 100) / 100);
        barChart2.UpdateData(1, 2, Mathf.Round(float.Parse(fiberAndFineProtein.actual.totalProteinIntake) * 100) / 100);
        barChart2.UpdateData(2, 0, (Mathf.Round(float.Parse(fiberAndFineProtein.plan.totalProteinIntake) * 100) / 100) / 3);
        barChart2.UpdateData(3, 0, (Mathf.Round(float.Parse(fiberAndFineProtein.actual.totalProteinIntake) * 100) / 100) / 3);

        barChart2.RefreshChart();



        BarChart barChart3 = bar3.GetComponent<BarChart>();

        for (int i = 0; i < promptInfo.energySupplyMessage.Count; i++)
        {
            tipList[3].text += promptInfo.energySupplyMessage[i] + "\n";

        }
        //全天当前
        float proteinTodayAllPresent = float.Parse(totalEnergy.protein) * 4;
        float fatTodayAllPresent = float.Parse(totalEnergy.fat) * 9;
        float choTodayAllPresent = float.Parse(totalEnergy.cho) * 4;
        float allTodayEnverMealPresent = proteinTodayAllPresent + fatTodayAllPresent + choTodayAllPresent;

        //计划早餐
        float proteinBreakPlan = float.Parse(user.data.breakfastPlan.protein) * 4;
        float fatBreakPlan = float.Parse(user.data.breakfastPlan.fat) * 9;
        float choBreakPlan = float.Parse(user.data.breakfastPlan.cho) * 4;
        float allBreakEnverMealPlan = proteinBreakPlan + fatBreakPlan + choBreakPlan;

        //当前早餐
        float proteinBreakPresent = float.Parse(everyMealEnergies[0].protein) * 4;
        float fatBreakPresent = float.Parse(everyMealEnergies[0].fat) * 9;
        float choBreakPresent = float.Parse(everyMealEnergies[0].cho) * 4;
        float allBreakEnverMealPresent = proteinBreakPresent + fatBreakPresent + choBreakPresent;

        //计划午餐
        float proteinLunchPlan = float.Parse(user.data.lunchPlan.protein) * 4;
        float fatLunchPlan = float.Parse(user.data.lunchPlan.fat) * 9;
        float choLunchPlan = float.Parse(user.data.lunchPlan.cho) * 4;
        float allLunchEnverMealPlan = proteinLunchPlan + fatLunchPlan + choLunchPlan;

        //当前午餐
        float proteinLunchPresent = float.Parse(everyMealEnergies[1].protein) * 4;
        float fatLunchPresent = float.Parse(everyMealEnergies[1].fat) * 9;
        float choLunchPresent = float.Parse(everyMealEnergies[1].cho) * 4;
        float allLunchEnverMealPresent = proteinLunchPresent + fatLunchPresent + choLunchPresent;

        //计划晚餐
        float proteinDinnerPlan = float.Parse(user.data.dinnerPlan.protein) * 4;
        float fatDinnerPlan = float.Parse(user.data.dinnerPlan.fat) * 9;
        float choDinnerPlan = float.Parse(user.data.dinnerPlan.cho) * 4;
        float allDinnerEnverMealPlan = proteinDinnerPlan + fatDinnerPlan + choDinnerPlan;

        //当前晚餐
        float proteinDinnerPresent = float.Parse(everyMealEnergies[2].protein) * 4;
        float fatDinnerPresent = float.Parse(everyMealEnergies[2].fat) * 9;
        float choDinnerPresent = float.Parse(everyMealEnergies[2].cho) * 4;
        float allDinnerEnverMealPresent = proteinDinnerPresent + fatDinnerPresent + choDinnerPresent;


        barChart3.UpdateData(0, 0, BackEnergyIntake(allTodayEnverMealPlan, proteinTodayAllPlan));       //全天计划
        barChart3.UpdateData(0, 1, BackEnergyIntake(allTodayEnverMealPresent, proteinTodayAllPresent));    //全天当前
        barChart3.UpdateData(0, 2, BackEnergyIntake(allBreakEnverMealPlan, proteinBreakPlan));       //计划早餐
        barChart3.UpdateData(0, 3, BackEnergyIntake(allBreakEnverMealPresent, proteinBreakPresent));    //当前早餐
        barChart3.UpdateData(0, 4, BackEnergyIntake(allLunchEnverMealPlan, proteinLunchPlan));       //计划午餐
        barChart3.UpdateData(0, 5, BackEnergyIntake(allLunchEnverMealPresent, proteinLunchPresent));    //当前午餐
        barChart3.UpdateData(0, 6, BackEnergyIntake(allDinnerEnverMealPlan, proteinDinnerPlan));      //计划晚餐
        barChart3.UpdateData(0, 7, BackEnergyIntake(allDinnerEnverMealPresent, proteinDinnerPresent));   //当前晚餐

        barChart3.UpdateData(1, 0, BackEnergyIntake(allTodayEnverMealPlan, fatTodayAllPlan));           //全天计划
        barChart3.UpdateData(1, 1, BackEnergyIntake(allTodayEnverMealPresent, fatTodayAllPresent));    //全天当前
        barChart3.UpdateData(1, 2, BackEnergyIntake(allBreakEnverMealPlan, fatBreakPlan));       //计划早餐
        barChart3.UpdateData(1, 3, BackEnergyIntake(allBreakEnverMealPresent, fatBreakPresent));    //当前早餐
        barChart3.UpdateData(1, 4, BackEnergyIntake(allLunchEnverMealPlan, fatLunchPlan));       //计划午餐
        barChart3.UpdateData(1, 5, BackEnergyIntake(allLunchEnverMealPresent, fatLunchPresent));    //当前午餐
        barChart3.UpdateData(1, 6, BackEnergyIntake(allDinnerEnverMealPlan, fatDinnerPlan));      //计划晚餐
        barChart3.UpdateData(1, 7, BackEnergyIntake(allDinnerEnverMealPresent, fatDinnerPresent));   //当前晚餐

        barChart3.UpdateData(2, 0, BackEnergyIntake(allTodayEnverMealPlan, choTodayAllPlan));           //全天计划
        barChart3.UpdateData(2, 1, BackEnergyIntake(allTodayEnverMealPresent, choTodayAllPresent));    //全天当前
        barChart3.UpdateData(2, 2, BackEnergyIntake(allBreakEnverMealPlan, choBreakPlan));       //计划早餐
        barChart3.UpdateData(2, 3, BackEnergyIntake(allBreakEnverMealPresent, choBreakPresent));    //当前早餐
        barChart3.UpdateData(2, 4, BackEnergyIntake(allLunchEnverMealPlan, choLunchPlan));       //计划午餐
        barChart3.UpdateData(2, 5, BackEnergyIntake(allLunchEnverMealPresent, choLunchPresent));    //当前午餐
        barChart3.UpdateData(2, 6, BackEnergyIntake(allDinnerEnverMealPlan, choDinnerPlan));      //计划晚餐
        barChart3.UpdateData(2, 7, BackEnergyIntake(allDinnerEnverMealPresent, choDinnerPresent));   //当前晚餐

        barChart3.RefreshChart();

        summaryUI.transform.Find("Score").GetComponent<Text>().text = score;
        summaryUI.transform.Find("FoodNum").GetComponent<Text>().text = foodNum;


    }

    public float BackEnergyIntake(float allEnverMeal, float every)
    {
        if (allEnverMeal == 0 && every == 0)
        {
            return 0;
        }
        return Mathf.Round(((every / allEnverMeal) * 100) * 100) / 100;
    }

    public float RecEnergyIntakeSplit(string recEnergyIntake, int index)
    {
        char separator = '~';
        string[] result = recEnergyIntake.Split(separator);
        if (result.Length == 1)
        {
            return float.Parse(result[0]);
        }
        return Mathf.Round(float.Parse(result[index]) * 100) / 100;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
