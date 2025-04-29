using OfficeOpenXml;
using RTS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
//using static UnityEditor.Progress;
using static UnityEngine.Rendering.ReloadAttribute;

/// <summary>
/// ������ʳ���ձ���excel
/// </summary>
public class ExcelExporter : MonoSingletonBase<ExcelExporter>
{
    public string filePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
    public string fileName = "ExcelData.xlsx";
    public FoodRecriveConverDay response = new FoodRecriveConverDay();
    // Start is called before the first frame update
    public void SaveToExcel()
    {
        List<List<string>> data = new List<List<string>>();
        List<string> header = new List<string>();
        //header.Add("һ�ձ���");
        header.Add("����");
        header.Add("������");
        header.Add("������");
        header.Add("֬��");
        header.Add("̼ˮ������");
        for (int i = 0; i < response.data.elementResult.Count; i++)
        {
            header.Add(response.data.elementResult[i].zhName);
        }
        data.Add(header);

        List<string> rowBr = new List<string>();
        rowBr.Add("���");
        rowBr.Add(response.data.breakfastEnergy.totalEnergyKcal.ToString());
        rowBr.Add(response.data.breakfastEnergy.protein.ToString());
        rowBr.Add(response.data.breakfastEnergy.fat.ToString());
        rowBr.Add(response.data.breakfastEnergy.cho.ToString());
        data.Add(rowBr);

        List<string> rowLu = new List<string>();
        rowLu.Add("���");
        rowLu.Add(response.data.lunchEnergy.totalEnergyKcal.ToString());
        rowLu.Add(response.data.lunchEnergy.protein.ToString());
        rowLu.Add(response.data.lunchEnergy.fat.ToString());
        rowLu.Add(response.data.lunchEnergy.cho.ToString());
        data.Add(rowLu);

        List<string> rowDi = new List<string>();
        rowDi.Add("���");
        rowDi.Add(response.data.dinnerEnergy.totalEnergyKcal.ToString());
        rowDi.Add(response.data.dinnerEnergy.protein.ToString());
        rowDi.Add(response.data.dinnerEnergy.fat.ToString());
        rowDi.Add(response.data.dinnerEnergy.cho.ToString());
        data.Add(rowDi);

        List<string> rowAll = new List<string>();
        rowAll.Add("������");
        rowAll.Add(response.data.totalEnergy.totalEnergyKcal.ToString());
        rowAll.Add(response.data.totalEnergy.protein.ToString());
        rowAll.Add(response.data.totalEnergy.fat.ToString());
        rowAll.Add(response.data.totalEnergy.cho.ToString());
        for (int i = 0; i < response.data.elementResult.Count; i++)
        {
            rowAll.Add(response.data.elementResult[i].totalContent);
        }
        data.Add(rowAll);

        // ���� CSV ����
        string csvContent = "";
        foreach (var row in data)
        {
            foreach (var item in row)
            {
                csvContent += string.Join(",", item);
            }
            csvContent += "\n";

        }
        // ���浽�ļ�
        string filePath = Path.Combine(this.filePath, fileName);
        File.WriteAllText(filePath, csvContent);

        Debug.Log("CSV �ļ��ѱ�������" + filePath);

        // ����������
        //IRow headerRow = sheet.CreateRow(0);
        //headerRow.CreateCell(0).SetCellValue("����");
        //headerRow.CreateCell(1).SetCellValue("������");
        //headerRow.CreateCell(2).SetCellValue("������");
        //headerRow.CreateCell(3).SetCellValue("֬��");
        //headerRow.CreateCell(4).SetCellValue("̼ˮ������");
        //for (int i = 0; i < response.data.elementResult.Count; i++)
        //{
        //    headerRow.CreateCell(5 + i).SetCellValue(response.data.elementResult[i].zhName);
        //}
        //IRow rowBr = sheet.CreateRow(1);
        //rowBr.CreateCell(0).SetCellValue("���");
        //rowBr.CreateCell(1).SetCellValue(response.data.breakfastEnergy.totalEnergyKcal);
        //rowBr.CreateCell(2).SetCellValue(response.data.breakfastEnergy.protein);
        //rowBr.CreateCell(3).SetCellValue(response.data.breakfastEnergy.fat);
        //rowBr.CreateCell(3).SetCellValue(response.data.breakfastEnergy.cho);
        //IRow rowLu = sheet.CreateRow(2);
        //rowLu.CreateCell(0).SetCellValue("���");
        //rowLu.CreateCell(1).SetCellValue(response.data.lunchEnergy.totalEnergyKcal);
        //rowLu.CreateCell(2).SetCellValue(response.data.lunchEnergy.protein);
        //rowLu.CreateCell(3).SetCellValue(response.data.lunchEnergy.fat);
        //rowLu.CreateCell(3).SetCellValue(response.data.lunchEnergy.cho);
        //IRow rowDi = sheet.CreateRow(3);
        //rowDi.CreateCell(0).SetCellValue("���");
        //rowDi.CreateCell(1).SetCellValue(response.data.dinnerEnergy.totalEnergyKcal);
        //rowDi.CreateCell(2).SetCellValue(response.data.dinnerEnergy.protein);
        //rowDi.CreateCell(3).SetCellValue(response.data.dinnerEnergy.fat);
        //rowDi.CreateCell(3).SetCellValue(response.data.dinnerEnergy.cho);
        //IRow rowAll = sheet.CreateRow(4);
        //rowAll.CreateCell(0).SetCellValue("������");
        //rowAll.CreateCell(1).SetCellValue(response.data.totalEnergy.totalEnergyKcal);
        //rowAll.CreateCell(2).SetCellValue(response.data.totalEnergy.protein);
        //rowAll.CreateCell(3).SetCellValue(response.data.totalEnergy.fat);
        //rowAll.CreateCell(3).SetCellValue(response.data.totalEnergy.cho);
        //IRow rowElement = sheet.CreateRow(5);
        //// ���������
        //for (int i = 0; i < response.data.elementResult.Count; i++)
        //{
        //    rowElement.CreateCell(i + 5).SetCellValue(response.data.elementResult[i].totalContent);
        //}
    }
    // Update is called once per frame
    void Update()
    {

    }
}
