using Excel;
using RTS;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class DataManager : MonoBehaviour
{
    // 存储所有 ItemData 的列表
    public List<EquipmentItemData> itemList = new List<EquipmentItemData>();
    //private Dictionary<string, EquipmentItemData> itemDictionary = new Dictionary<string, EquipmentItemData>();
    public Dictionary<string, List<EquipmentItemData>> itemDictionary = new Dictionary<string, List<EquipmentItemData>>();

    public string jsonName;
    private string jsonFilePath => Path.Combine(Application.streamingAssetsPath, jsonName + ".json");

    public static DataManager Instance { get; private set; }


    // 存储所有 ItemData 的列表
    //private Dictionary<string, EquipmentItemData> itemDictionary = new Dictionary<string, EquipmentItemData>();
    //private Dictionary<DataCategory, List<FoodKindItemData>> itemDictionary = new Dictionary<DataCategory, List<FoodKindItemData>>();
    public string excelFolderPath = Application.streamingAssetsPath;

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
        itemDictionary.Clear();

        // 获取完整路径
        string fullPath = Path.Combine(Application.dataPath, excelFolderPath);

        if (Directory.Exists(fullPath))
        {
            string path = "/ExperimentalHint/实验提示.xlsx";

            itemDictionary.Add("氨基酸提示", ParseSheet(excelFolderPath + path, "氨基酸提示"));
            itemDictionary.Add("香气提示", ParseSheet(excelFolderPath + path, "香气提示"));
            itemDictionary.Add("重金属提示", ParseSheet(excelFolderPath + path, "重金属提示"));

            string pathMoxing = "/ExperimentalHint/模型展示.xlsx";
            itemDictionary.Add("Anjisuan", ParseSheet(excelFolderPath + pathMoxing, "氨基酸"));
            itemDictionary.Add("Xiangqi", ParseSheet(excelFolderPath + pathMoxing, "香气"));
            itemDictionary.Add("Zhongjinshu", ParseSheet(excelFolderPath + pathMoxing, "重金属"));
            itemDictionary.Add("Shachongji", ParseSheet(excelFolderPath + pathMoxing, "杀虫剂"));
            itemDictionary.Add("Huanyuantang", ParseSheet(excelFolderPath + pathMoxing, "还原糖"));
            itemDictionary.Add("Zhifang", ParseSheet(excelFolderPath + pathMoxing, "脂肪"));
            itemDictionary.Add("Danbaizhi", ParseSheet(excelFolderPath + pathMoxing, "蛋白质"));

        }
    }

    public List<EquipmentItemData> ParseSheet(string tableName, string sheetName)
    {
        List<EquipmentItemData> dataList = new List<EquipmentItemData>();

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
                            // 反射或手动映射字段
                            EquipmentItemData data = new EquipmentItemData
                            {
                                id = reader.GetString(0),
                                parent = reader.GetString(1),
                                introduce = reader.GetString(2),
                                iconName = reader.GetString(3)
                            };
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
        List<EquipmentItemData> dataList = new List<EquipmentItemData>();
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
                    EquipmentItemData item = new EquipmentItemData
                    {
                        id = rowData[0].ToString(),
                        parent = rowData[1].ToString(),
                        introduce = rowData[2].ToString(),
                        iconName = rowData[3].ToString()
                        // 根据实际列顺序调整
                    };
                    dataList.Add(item);
                }
            }

            itemDictionary[tableName] = dataList;

        }
    }




    #region
    // 保存数据到 JSON 文件
    public void SaveDataToJson(EquipmentItemData data, string filePath)
    {
        List<SerializableItemData> serializableList = new List<SerializableItemData>();
        // 创建可序列化的数据类
        foreach (EquipmentItemData item in itemList)
        {
            serializableList.Add(new SerializableItemData
            {
                id = data.id,
                parent = data.parent,
                introduce = data.introduce,
                iconName = item.iconName // 保存 Sprite 名称
            });
        }


        // 转换为 JSON 字符串
        string json = JsonUtility.ToJson(new Wrapper { items = serializableList }, true);
        // 写入文件
        File.WriteAllText(jsonFilePath, json);
        Debug.Log($"数据已保存到: {jsonFilePath}");
    }


    // 从 JSON 加载数据到内存
    public void LoadAllFromJson()
    {
        itemDictionary.Clear();

        if (!File.Exists(jsonFilePath))
        {
            Debug.LogError($"文件不存在: {jsonFilePath}");
            return;
        }

        string json = File.ReadAllText(jsonFilePath);
        Wrapper root = JsonUtility.FromJson<Wrapper>(json);
        ForWraList(root.Anjisuan, "Anjisuan");
        ForWraList(root.Xiangqi, "Xiangqi");
        ForWraList(root.Zhongjinshu, "Zhongjinshu");
        ForWraList(root.Shachongji, "Shachongji");
        ForWraList(root.Huanyuantang, "Huanyuantang");
        ForWraList(root.Zhifang, "Zhifang");
        ForWraList(root.Danbaizhi, "Danbaizhi");

        ForWraList(root.ShipinAnjisuan, "ShipinAnjisuan");
        ForWraList(root.ShipinXiangqi, "ShipinXiangqi");
        ForWraList(root.NongchanpinZhongjinshu, "NongchanpinZhongjinshu");
        ForWraList(root.NongchanpinYoujilin, "NongchanpinYoujilin");
        ForWraList(root.HuanyuanDidingfa, "HuanyuanDidingfa");
        ForWraList(root.HuanyuanSuoshi, "HuanyuanSuoshi");
        ForWraList(root.HuanyuanDanbaizhi, "HuanyuanDanbaizhi");
        ForWraList(root.ShanshiFenxi, "ShanshiFenxi");
        ForWraList(root.GerenYinyang, "GerenYinyang");
        ForWraList(root.RentiShuzi, "RentiShuzi");


        Debug.Log($"已加载 {itemDictionary.Count} 条数据");
    }

    private void ForWraList(List<SerializableItemData> list, string id)
    {
        List<EquipmentItemData> equipmentList = new List<EquipmentItemData>();

        foreach (SerializableItemData sData in list)
        {
            EquipmentItemData item = ScriptableObject.CreateInstance<EquipmentItemData>();
            item.id = sData.id;
            item.parent = sData.parent;
            item.introduce = sData.introduce;

            // 加载 Sprite（假设图标在 Resources/Icons 下）
            if (!string.IsNullOrEmpty(sData.iconName))
            {
                item.iconName = sData.iconName;
            }
            equipmentList.Add(item);
        }
        itemDictionary.Add(id, equipmentList);

    }

    #endregion

    // 按 ID 查询数据
    public List<EquipmentItemData> GetItemById(string id)
    {
        if (itemDictionary.TryGetValue(id, out List<EquipmentItemData> data))
        {
            return data;
        }
        Debug.Log($"未找到 ID 为 {id} 的数据");
        return null;
    }

    [System.Serializable]
    private class SerializableItemData
    {
        public string id;
        public string parent;
        public string introduce;
        public string iconName; // 存储 Sprite 路径或名称
    }
    // JSON 数组需要包装类
    [System.Serializable]
    private class Wrapper
    {
        public List<SerializableItemData> items;
        public List<SerializableItemData> Anjisuan;
        public List<SerializableItemData> Xiangqi;
        public List<SerializableItemData> Zhongjinshu;
        public List<SerializableItemData> Shachongji;
        public List<SerializableItemData> Huanyuantang;
        public List<SerializableItemData> Zhifang;
        public List<SerializableItemData> Danbaizhi;

        public List<SerializableItemData> ShipinAnjisuan;
        public List<SerializableItemData> ShipinXiangqi;
        public List<SerializableItemData> NongchanpinZhongjinshu;
        public List<SerializableItemData> NongchanpinYoujilin;
        public List<SerializableItemData> HuanyuanDidingfa;
        public List<SerializableItemData> HuanyuanSuoshi;
        public List<SerializableItemData> HuanyuanDanbaizhi;
        public List<SerializableItemData> ShanshiFenxi;
        public List<SerializableItemData> GerenYinyang;
        public List<SerializableItemData> RentiShuzi;

    }


}
