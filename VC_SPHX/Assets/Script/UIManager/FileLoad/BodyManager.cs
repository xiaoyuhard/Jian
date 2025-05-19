using Excel;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;


/// <summary>
/// 人体解剖数据管理器 进行读取excel数据
/// </summary>
public class BodyManager : MonoBehaviour
{
    public static BodyManager Instance { get; private set; }

    // 存储所有 ItemData 的列表
    //private Dictionary<string, EquipmentItemData> itemDictionary = new Dictionary<string, EquipmentItemData>();
    public Dictionary<string, List<BodyItem>> itemBodyExplic = new Dictionary<string, List<BodyItem>>();
    public Dictionary<string, List<EndocrineSystemData>> itemEndocrDic = new Dictionary<string, List<EndocrineSystemData>>();
    public Dictionary<string, List<BoneHierarchy>> itemBoneDic = new Dictionary<string, List<BoneHierarchy>>();
    //private Dictionary<DataCategory, List<FoodKindItemData>> itemDictionary = new Dictionary<DataCategory, List<FoodKindItemData>>();
    public string excelFolderPath = Application.dataPath;
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
        //string fullPath = Path.Combine(Application.dataPath, excelFolderPath);
        string fullPath = Path.Combine(Application.dataPath);

        //if (Directory.Exists(fullPath))
        {
            ////Debug.Log(Directory.GetFiles(fullPath, "/*.xlsx")+"   "+ Directory.GetFiles(fullPath, "*.xlsx"));
            //foreach (var filePath in Directory.GetFiles(fullPath, "*.xlsx"))
            //{
            //    string fileName = Path.GetFileNameWithoutExtension(filePath);
            //    //ParseExcel(fileName, File.ReadAllText(filePath));
            //    ParseExcel(fileName, filePath, File.ReadAllText(filePath));
            //}

            itemBodyExplic.Clear();
            //string streamingAssetsPath = Application.streamingAssetsPath;
            List<BodyItem> man = LoadCSVData<BodyItem>("Data/BodyCsv/Man", true, 53);
            List<BodyItem> woman = LoadCSVData<BodyItem>("Data/BodyCsv/Woman", true, 54);
            itemBodyExplic.Add("Man", man);
            itemBodyExplic.Add("Woman", woman);

            //string path = "/StreamingAssets/人体/人体解剖.xlsx";

            //// 解析内分泌系统
            //var endocrineData = ParseSheet<EndocrineSystemData>(fullPath + path, "内分泌系统");
            //var breatheData = ParseSheet<EndocrineSystemData>(fullPath + path, "呼吸系统");
            //var loopData = ParseSheet<EndocrineSystemData>(fullPath + path, "循环系统");
            //var urinaryData = ParseSheet<EndocrineSystemData>(fullPath + path, "泌尿系统");
            //var digestionData = ParseSheet<EndocrineSystemData>(fullPath + path, "消化系统");
            //var lymphData = ParseSheet<EndocrineSystemData>(fullPath + path, "淋巴系统");
            //var procreationData = ParseSheet<EndocrineSystemData>(fullPath + path, "生殖系统");
            //var nerveData = ParseSheet<EndocrineSystemData>(fullPath + path, "神经系统");
            //var epitheliumData = ParseSheet<EndocrineSystemData>(fullPath + path, "上皮组织");

            //itemEndocrDic.Add("内分泌系统", endocrineData);
            //itemEndocrDic.Add("呼吸系统", breatheData);
            //itemEndocrDic.Add("循环系统", loopData);
            //itemEndocrDic.Add("泌尿系统", urinaryData);
            //itemEndocrDic.Add("消化系统", digestionData);
            //itemEndocrDic.Add("淋巴系统", lymphData);
            //itemEndocrDic.Add("生殖系统", procreationData);
            //itemEndocrDic.Add("神经系统", nerveData);
            //itemEndocrDic.Add("上皮组织", epitheliumData);

            //// 解析骨骼系统
            //var skeletalData = ParseSheet<BoneHierarchy>(fullPath + path, "运动系统_骨骼");
            //var feelData = ParseSheet<BoneHierarchy>(fullPath + path, "感觉系统");
            //var muscleData = ParseSheet<BoneHierarchy>( fullPath + path, "肌肉组织");
            //itemBoneDic.Add("运动系统_骨骼", skeletalData);
            //itemBoneDic.Add("感觉系统", feelData);
            //itemBoneDic.Add("肌肉组织", muscleData);

            string pathBodyExpl = "/StreamingAssets/人体/BodyExpl.xlsx";

            //var man = ParseSheet<BodyItem>(fullPath + pathBodyExpl, "Man");
            //var woman = ParseSheet<BodyItem>(fullPath + pathBodyExpl, "Woman");
            //itemBodyExplic.Add("Man", man);
            //itemBodyExplic.Add("Woman", woman);
        }
    }


    /// <summary>
    /// 通用CSV加载方法
    /// </summary>
    /// <param name="filePath">文件路径（不需要扩展名，相对于Resources文件夹）</param>
    /// <param name="skipHeader">是否跳过标题行（默认跳过）</param>
    public static List<T> LoadCSVData<T>(string filePath, bool skipHeader, int count) where T : new()
    {
        List<T> dataList = new List<T>();

        try
        {

            // 加载CSV文本文件 ---------------------------------------------------
            TextAsset csvFile = Resources.Load<TextAsset>(filePath);

            if (csvFile == null)
            {
                Debug.LogError($"CSV文件未找到: {filePath}");
                return dataList;
            }

            // 使用正则表达式拆分复杂CSV格式 ----------------------------------------
            // 正则说明：处理包含逗号、换行符的字段（例如："aaa,bbb"）
            Regex csvSplit = new Regex("(?:^|,)(\"(?:[^\"]+|\"\")*\"|[^,]*)");
            //Debug.Log(csvFile.text);

            // 逐行解析 ----------------------------------------------------------
            using (StringReader reader = new StringReader(csvFile.text))
            {

                bool isFirstLine = true;
                string line;
                for (int l = 0; l < count; l++)
                {

                    //while ((line = reader.ReadLine()) != null)
                    {
                        line = reader.ReadLine();
                        //var line=reader.ReadLine();
                        // 跳过空行和注释行（以#开头）
                        if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#")) continue;
                        // 跳过标题行
                        if (isFirstLine && skipHeader)
                        {
                            isFirstLine = false;
                            continue;
                        }

                        // 使用正则匹配拆分字段 ----------------------------------------
                        List<string> fields = new List<string>();
                        MatchCollection matches = csvSplit.Matches(line);

                        foreach (Match match in matches)
                        {
                            string field = match.Value;

                            // 处理开头逗号的情况
                            if (field.StartsWith(","))
                                field = field.Substring(1);

                            //// 去除字段两端的引号（如果存在）
                            //if (field.StartsWith("\"") && field.EndsWith("\""))
                            //    field = field.Substring(1, field.Length - 2);
                            // 合并处理步骤：直接提取核心内容 + 去除两端逗号/引号
                            //string field = match.Groups[2].Value                     // 从正则捕获组提取核心内容
                            //    .TrimStart(',')                                      // 去除开头的逗号
                            //    .Trim('"');                                          // 去除两端的引号

                            fields.Add(field);
                        }

                        // 将字段映射到数据类 ------------------------------------------
                        T dataItem = new T();
                        System.Type type = typeof(T);

                        // 通过反射自动映射字段（要求字段顺序与CSV列顺序一致）
                        System.Reflection.FieldInfo[] fieldsInfo = type.GetFields();

                        for (int i = 0; i < Mathf.Min(fields.Count, fieldsInfo.Length); i++)
                        {
                            try
                            {
                                // 类型转换处理
                                object value = ConvertValue(fields[i], fieldsInfo[i].FieldType);
                                fieldsInfo[i].SetValue(dataItem, value);
                            }
                            catch (System.Exception e)
                            {
                                Debug.LogError($"字段解析错误 行: {line}\n错误: {e.Message}");
                            }
                        }

                        dataList.Add(dataItem);
                    }
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"CSV加载失败: {e.Message}");
        }

        return dataList;
    }
    /// <summary>
    /// 类型转换辅助方法
    /// </summary>
    private static object ConvertValue(string valueString, System.Type targetType)
    {
        if (string.IsNullOrEmpty(valueString)) return null;

        try
        {
            // 处理特殊类型
            if (targetType == typeof(string))
                return valueString;

            if (targetType == typeof(int))
                return int.Parse(valueString);

            if (targetType == typeof(float))
                return float.Parse(valueString);

            if (targetType == typeof(bool))
                return valueString.ToLower() == "true";

            // 添加更多类型转换规则...
        }
        catch
        {
            Debug.LogError($"无法转换值: {valueString} 到类型 {targetType.Name}");
            return null;
        }

        return null;
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
                            if (typeof(T) == typeof(BodyItem))
                            {
                                ((BodyItem)(object)data).bodyId = reader.GetString(0);
                                ((BodyItem)(object)data).bodyExplanation = reader.GetString(1);
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
                    //// 将 Excel 行数据映射到 FoodKindItemData
                    //FoodKindItemData item = new FoodKindItemData
                    //{
                    //    id = rowData[0].ToString(),
                    //    code = rowData[1].ToString(),
                    //    iconName = rowData[2].ToString(),
                    //    Edible = rowData[3].ToString(),
                    //    unit = rowData[4].ToString(),
                    //    heat = rowData[5].ToString(),
                    //    protein = rowData[6].ToString(),
                    //    fat = rowData[7].ToString(),
                    //    carbohydrate = rowData[8].ToString()
                    //    // 根据实际列顺序调整
                    //};
                    //dataList.Add(item);
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

    //按 ID 查询数据
    public List<BodyItem> GetBodyExplItemById(string id)
    {
        if (itemBodyExplic.TryGetValue(id, out List<BodyItem> data))
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

