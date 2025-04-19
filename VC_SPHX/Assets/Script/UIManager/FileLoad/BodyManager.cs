using Excel;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class BodyManager : MonoBehaviour
{
    public static BodyManager Instance { get; private set; }

    // 存储所有 ItemData 的列表
    public List<FoodKindItemData> itemList = new List<FoodKindItemData>();
    //private Dictionary<string, EquipmentItemData> itemDictionary = new Dictionary<string, EquipmentItemData>();
    public Dictionary<string, List<EndocrineSystemData>> itemEndocrDic = new Dictionary<string, List<EndocrineSystemData>>();
    public Dictionary<string, List<BoneHierarchy>> itemBoneDic = new Dictionary<string, List<BoneHierarchy>>();
    //private Dictionary<DataCategory, List<FoodKindItemData>> itemDictionary = new Dictionary<DataCategory, List<FoodKindItemData>>();
    public string excelFolderPath = Application.streamingAssetsPath;
    public string jsonName;
    private string jsonFilePath => Path.Combine(Application.streamingAssetsPath, jsonName + ".xlsx");

    void Awake()
    {
        InitializeManager();
    }

    // 显式初始化方法（供编辑器调用）
    public void InitializeManager()
    {
        if (Instance == null)
        {
            Instance = this;
            LoadAllExcelData();

            // 确保编辑器退出时清理实例
#if UNITY_EDITOR
            UnityEditor.EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
#endif
        }
    }

#if UNITY_EDITOR
    private void OnPlayModeStateChanged(UnityEditor.PlayModeStateChange state)
    {
        if (state == UnityEditor.PlayModeStateChange.ExitingEditMode)
        {
            Instance = null;
        }
    }
#endif

    public void LoadAllExcelData()
    {
        //itemDictionary.Clear();

        // 获取完整路径
        string fullPath = Path.Combine(Application.dataPath, excelFolderPath);

        if (Directory.Exists(fullPath))
        {
            ////Debug.Log(Directory.GetFiles(fullPath, "/*.xlsx")+"   "+ Directory.GetFiles(fullPath, "*.xlsx"));
            //foreach (var filePath in Directory.GetFiles(fullPath, "*.xlsx"))
            //{
            //    string fileName = Path.GetFileNameWithoutExtension(filePath);
            //    //ParseExcel(fileName, File.ReadAllText(filePath));
            //    ParseExcel(fileName, filePath, File.ReadAllText(filePath));
            //}
            string path = "/人体/人体解剖.xlsx";

            // 解析内分泌系统
            var endocrineData = ParseSheet<EndocrineSystemData>(excelFolderPath + path, "内分泌系统");
            var breatheData = ParseSheet<EndocrineSystemData>(excelFolderPath + path, "呼吸系统");
            var loopData = ParseSheet<EndocrineSystemData>(excelFolderPath + path, "循环系统");
            var urinaryData = ParseSheet<EndocrineSystemData>(excelFolderPath + path, "泌尿系统");
            var digestionData = ParseSheet<EndocrineSystemData>(excelFolderPath + path, "消化系统");
            var lymphData = ParseSheet<EndocrineSystemData>(excelFolderPath + path, "淋巴系统");
            var procreationData = ParseSheet<EndocrineSystemData>(excelFolderPath + path, "生殖系统");
            var nerveData = ParseSheet<EndocrineSystemData>(excelFolderPath + path, "神经系统");
            var epitheliumData = ParseSheet<EndocrineSystemData>(excelFolderPath + path, "上皮组织");

            itemEndocrDic.Add("内分泌系统", endocrineData);
            itemEndocrDic.Add("呼吸系统", breatheData);
            itemEndocrDic.Add("循环系统", loopData);
            itemEndocrDic.Add("泌尿系统", urinaryData);
            itemEndocrDic.Add("消化系统", digestionData);
            itemEndocrDic.Add("淋巴系统", lymphData);
            itemEndocrDic.Add("生殖系统", procreationData);
            itemEndocrDic.Add("神经系统", nerveData);
            itemEndocrDic.Add("上皮组织", epitheliumData);

            // 解析骨骼系统
            var skeletalData = ParseSheet<BoneHierarchy>(excelFolderPath + path, "运动系统_骨骼");
            var feelData = ParseSheet<BoneHierarchy>(excelFolderPath + path, "感觉系统");
            var muscleData = ParseSheet<BoneHierarchy>(excelFolderPath + path, "肌肉组织");
            itemBoneDic.Add("运动系统_骨骼", skeletalData);
            itemBoneDic.Add("感觉系统", feelData);
            itemBoneDic.Add("肌肉组织", muscleData);

        }
    }

    public List<T> ParseSheet<T>(string tableName, string sheetName) where T : new()
    {
        List<T> dataList = new List<T>();

        using (var stream = File.Open(tableName, FileMode.Open, FileAccess.Read))
        {
            using (var reader = ExcelReaderFactory.CreateOpenXmlReader(stream))
            {
                do
                {
                    if (reader.Name == sheetName)
                    {
                        // 跳过表头（根据实际情况调整）
                        reader.Read(); // 跳过第一行（列名）

                        while (reader.Read())
                        {
                            T data = new T();
                            // 反射或手动映射字段
                            if (typeof(T) == typeof(EndocrineSystemData))
                            {
                                ((EndocrineSystemData)(object)data).Male = reader.GetString(0);
                                ((EndocrineSystemData)(object)data).Female = reader.GetString(1);
                            }
                            else if (typeof(T) == typeof(BoneHierarchy))
                            {
                                ((BoneHierarchy)(object)data).Level1 = reader.GetString(0);
                                ((BoneHierarchy)(object)data).Level2 = reader.GetString(1);
                                ((BoneHierarchy)(object)data).Level3 = reader.GetString(2);
                                ((BoneHierarchy)(object)data).Level4 = reader.GetString(3);
                            }
                            dataList.Add(data);
                        }
                    }
                } while (reader.NextResult());
            }
        }
        return dataList;
    }


    void ParseExcel(string tableName, string tablePath, string excelContent)
    {
        List<FoodKindItemData> dataList = new List<FoodKindItemData>();
        using (var stream = File.Open(tablePath, FileMode.Open, FileAccess.Read))
        {
            // 创建 Reader（指定读取 .xlsx 文件）
            using (var reader = ExcelReaderFactory.CreateOpenXmlReader(stream))
            {
                // 读取数据为 DataSet
                var result = reader.AsDataSet();
                var dataTable = result.Tables[0]; // 假设数据在第一个工作表

                // 遍历行（跳过表头）
                for (int row = 1; row < dataTable.Rows.Count; row++)
                {
                    var rowData = dataTable.Rows[row];
                    // 将 Excel 行数据映射到 FoodKindItemData
                    FoodKindItemData item = new FoodKindItemData
                    {
                        id = rowData[0].ToString(),
                        code = rowData[1].ToString(),
                        iconName = rowData[2].ToString(),
                        Edible = rowData[3].ToString(),
                        unit = rowData[4].ToString(),
                        heat = rowData[5].ToString(),
                        protein = rowData[6].ToString(),
                        fat = rowData[7].ToString(),
                        carbohydrate = rowData[8].ToString()
                        // 根据实际列顺序调整
                    };
                    dataList.Add(item);
                }
            }

            //itemDictionary[tableName] = dataList;

        }
    }


    //按 ID 查询数据
    public List<EndocrineSystemData> GetEndocrItemById(string id)
    {
        if (itemEndocrDic.TryGetValue(id, out List<EndocrineSystemData> data))
        {
            return data;
        }
        Debug.Log($"未找到 ID 为 {id} 的数据");
        return null;
    }
    //按 ID 查询数据
    public List<BoneHierarchy> GetBoneItemById(string id)
    {
        if (itemBoneDic.TryGetValue(id, out List<BoneHierarchy> data))
        {
            return data;
        }
        Debug.Log($"未找到 ID 为 {id} 的数据");
        return null;
    }


}

[System.Serializable]
public class EndocrineSystemData
{
    public string Male;
    public string Female;

}
[System.Serializable]
public class BoneHierarchy
{
    public string Level1;
    public string Level2;
    public string Level3;
    public string Level4;
}

