using Excel;
using OfficeOpenXml.DataValidation;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using RTS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
//using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Networking;
//using static UnityEditor.Progress;
using static UnityEngine.Rendering.DebugUI;
using Newtonsoft.Json;
//using UnityEditorInternal.Profiling.Memory.Experimental;

/// <summary>
/// 食物管理 接收保存所有食物数据
/// </summary>
public class FoodManager : MonoBehaviour
{
    public static FoodManager Instance { get; private set; }

    // 存储所有 ItemData 的列表
    public List<FoodKindItemData> itemList = new List<FoodKindItemData>();
    //private Dictionary<string, EquipmentItemData> itemDictionary = new Dictionary<string, EquipmentItemData>();
    public Dictionary<string, List<FoodKindItemData>> itemDictionary = new Dictionary<string, List<FoodKindItemData>>();
    //private Dictionary<DataCategory, List<FoodKindItemData>> itemDictionary = new Dictionary<DataCategory, List<FoodKindItemData>>();
    public string excelFolderPath = Application.streamingAssetsPath;
    public string jsonName;
    private string jsonFilePath => Path.Combine(Application.streamingAssetsPath, jsonName + ".xlsx");

    // 服务器API地址（示例）
    //private const string API_URL = "https://your-api-domain.com/food/items";


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
            //LoadAllExcelData();
            LoadFoodData();
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



    public static string ReadServerAddress()
    {

        string path = Path.Combine(Application.streamingAssetsPath, "Netaddress.txt");

        try
        {
            if (File.Exists(path))
            {
                return File.ReadAllText(path).Trim();
            }
            Debug.LogError("配置文件不存在: " + path);
        }
        catch (System.Exception e)
        {
            Debug.LogError("读取配置失败: " + e.Message);
        }

        return null; // 或返回默认地址
    }

    // 加载数据的公共方法
    public void LoadFoodData()
    {
        itemDictionary.Clear();
        StartCoroutine(LoadFoodPageQueryCoroutine("谷薯类", 138));
        StartCoroutine(LoadFoodPageQueryCoroutine("蔬菜水果类", 561));
        StartCoroutine(LoadFoodPageQueryCoroutine("畜禽鱼蛋类", 786));
        StartCoroutine(LoadFoodPageQueryCoroutine("大豆坚果奶类", 145));
        StartCoroutine(LoadFoodPageQueryCoroutine("油脂类", 26));
        StartCoroutine(LoadFoodPageQueryCoroutine("水", 22));
    }

    private IEnumerator LoadFoodPageQueryCoroutine(string categoryName, int pageSize)
    {
        // 获取基础地址
        string baseUrl = ReadServerAddress();
        if (string.IsNullOrEmpty(baseUrl))
        {
            Debug.LogError("服务器地址配置错误");
            yield break;
        }
    
        // 拼接完整地址
        //string fullUrl = $"{CombineUrl(baseUrl, "/food/pageQuery")}?CategoryName={categoryName}&pageSize={pageSize}";
        string fullUrl = $"{CombineUrl("http://172.28.67.73:9090", "/food/pageQuery")}?CategoryName={categoryName}&pageSize={pageSize}";
        using (UnityWebRequest request = UnityWebRequest.Get(fullUrl))
        {
            // 设置请求头（如果需要）
            //request.SetRequestHeader("Content-Type", "application/json");
            request.timeout = 3; // 设置超时时间
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                try
                {
                    // 解析JSON数据
                    string jsonResponse = request.downloadHandler.text;
                    FoodResponse response = JsonConvert.DeserializeObject<FoodResponse>(jsonResponse);

                    //FoodResponse response = JsonUtility.FromJson<FoodResponse>(jsonResponse);
                    //itemDictionary.Add(categoryName, response.rows);
                    // 检查键是否存在
                    if (!itemDictionary.ContainsKey(categoryName))
                    {
                        // 创建新键并初始化列表
                        itemDictionary.Add(categoryName, new List<FoodKindItemData>());
                    }

                    // 向列表中添加数据
                    itemDictionary[categoryName] = (response.rows);

                    //foreach (var item in response.rows)
                    //{
                    //    if (itemDictionary.ContainsKey(item.categoryName))
                    //    {
                    //        itemDictionary[item.categoryName].Add(item);
                    //    }
                    //    else
                    //    {
                    //        itemDictionary[item.categoryName] = new List<FoodKindItemData> { item };
                    //    }
                    //}
                    //onSuccess?.Invoke(itemDictionary);
                }
                catch (Exception e)
                {
                    //onError?.Invoke($"JSON解析失败: {e.Message}");
                    Debug.LogError($"JSON解析失败: {e.Message}");
                }
            }
            else
            {
                //onError?.Invoke($"网络请求失败: {request.error}");
                Debug.LogError($"网络请求失败: {request.error}");

            }
        }
    }


    private string CombineUrl(string baseUrl, string path)
    {
        return $"{baseUrl.TrimEnd('/')}/{path.TrimStart('/')}";
    }


    public void LoadAllExcelData()
    {
        itemDictionary.Clear();

        //// 获取完整路径
        //string fullPath = Path.Combine(Application.dataPath, excelFolderPath);

        //if (Directory.Exists(fullPath))
        //{
        //    //Debug.Log(Directory.GetFiles(fullPath, "/*.xlsx")+"   "+ Directory.GetFiles(fullPath, "*.xlsx"));
        //    foreach (var filePath in Directory.GetFiles(fullPath, "*.xlsx"))
        //    {
        //        string fileName = Path.GetFileNameWithoutExtension(filePath);
        //        //ParseExcel(fileName, File.ReadAllText(filePath));
        //        ParseExcel(fileName, filePath, File.ReadAllText(filePath));
        //    }
        //}
        // 获取完整路径
        string fullPath = Path.Combine(Application.dataPath, excelFolderPath);

        if (Directory.Exists(fullPath))
        {
            string path = "/食物成分表.xlsx";

            itemDictionary.Add("grain", ParseExcel(excelFolderPath + path, "grain", 0));
            itemDictionary.Add("vegetable", ParseExcel(excelFolderPath + path, "vegetable", 1));
            itemDictionary.Add("oil", ParseExcel(excelFolderPath + path, "oil", 2));
            itemDictionary.Add("meat", ParseExcel(excelFolderPath + path, "meat", 3));
            itemDictionary.Add("bean", ParseExcel(excelFolderPath + path, "bean", 4));
            itemDictionary.Add("water", ParseExcel(excelFolderPath + path, "water", 5));
        }
    }

    public void ResFoodOnClick()
    {
        foreach (var item in itemDictionary.Values)
        {
            List<FoodKindItemData> foodKindItems = item;
            foreach (var item2 in foodKindItems)
            {
                item2.isOnClick = false;
            }
        }
    }

    //public List<FoodKindItemData> ParseExcel(string tableName, string tablePath, string excelContent)
    public List<FoodKindItemData> ParseExcel(string tableName, string excelContent, int index)
    {
        List<FoodKindItemData> dataList = new List<FoodKindItemData>();
        //using (var stream = File.Open(tablePath, FileMode.Open, FileAccess.Read))
        using (var stream = File.Open(tableName, FileMode.Open, FileAccess.Read))
        {
            // 创建 Reader（指定读取 .xlsx 文件）
            using (var reader = ExcelReaderFactory.CreateOpenXmlReader(stream))
            {
                // 读取数据为 DataSet
                var result = reader.AsDataSet();
                var dataTable = result.Tables[excelContent]; // 假设数据在第一个工作表

                // 遍历行（跳过表头）
                for (int row = 1; row < dataTable.Rows.Count; row++)
                {
                    var rowData = dataTable.Rows[row];
                    // 将 Excel 行数据映射到 FoodKindItemData
                    FoodKindItemData item = new FoodKindItemData
                    {
                        //id = rowData[0],
                        foodCode = rowData[1].ToString(),
                        foodName = rowData[2].ToString(),
                        edible = rowData[3].ToString(),
                        water = rowData[4].ToString(),
                        heat = rowData[5].ToString(),
                        protein = rowData[6].ToString(),
                        fat = rowData[7].ToString(),
                        cho = rowData[8].ToString()
                        // 根据实际列顺序调整
                    };
                    dataList.Add(item);
                }
            }

            return dataList;

        }
    }

    #region
    private string SafeGetField(string[] fields, int index)
    {
        return (index < fields.Length) ? fields[index].Trim() : "";
    }



    // 保存数据到 JSON 文件
    public void SaveDataToJson(FoodKindItemData data, string filePath)
    {
        List<SerializableItemData> serializableList = new List<SerializableItemData>();
        // 创建可序列化的数据类
        //foreach (FoodKindItemData item in itemList)
        //{
        //    serializableList.Add(new SerializableItemData
        //    {
        //        id = data.id,
        //        code = data.code,
        //        iconName = data.iconName,
        //        unit = data.unit,
        //        heat = data.heat,
        //        protein = data.protein,
        //        fat = data.fat,
        //        carbohydrate = data.carbohydrate
        //    });
        //}


        // 转换为 JSON 字符串
        //string json = JsonUtility.ToJson(new Wrapper { items = serializableList }, true);
        // 写入文件
        //File.WriteAllText(jsonFilePath, json);
        Debug.Log($"数据已保存到: {jsonFilePath}");
    }


    //// 从 JSON 加载数据到内存
    //public void LoadAllFromJson()
    //{
    //    itemDictionary.Clear();

    //    if (!File.Exists(jsonFilePath))
    //    {
    //        Debug.LogError($"文件不存在: {jsonFilePath}");
    //        return;
    //    }

    //    string json = File.ReadAllText(jsonFilePath);
    //    Wrapper root = JsonUtility.FromJson<Wrapper>(json);
    //    ForWraList(root.grain, "grain");
    //    ForWraList(root.bean, "bean");
    //    ForWraList(root.vegetable, "vegetable");
    //    ForWraList(root.oil, "oil");
    //    ForWraList(root.meat, "meat");
    //    ForWraList(root.water, "water");



    //    Debug.Log($"已加载 {itemDictionary.Count} 条数据");

    //}

    private void ForWraList(List<SerializableItemData> list, string id)
    {
        List<FoodKindItemData> foodKindList = new List<FoodKindItemData>();

        foreach (SerializableItemData sData in list)
        {
            //FoodKindItemData item = ScriptableObject.CreateInstance<FoodKindItemData>();
            //item.id = sData.id;
            //item.code = sData.code;
            //item.iconName = sData.iconName;
            //item.unit = sData.unit;
            //item.heat = sData.heat;
            //item.protein = sData.protein;
            //item.fat = sData.fat;
            //item.carbohydrate = sData.carbohydrate;

            //// 加载 Sprite（假设图标在 Resources/Icons 下）
            //if (!string.IsNullOrEmpty(sData.iconName))
            //{
            //    item.iconName = sData.iconName;
            //}
            //foodKindList.Add(item);
        }
        //itemDictionary.Add(id, foodKindList);

    }

    #endregion

    //按 ID 查询数据
    public List<FoodKindItemData> GetItemById(string id)
    {
        if (itemDictionary.TryGetValue(id, out List<FoodKindItemData> data))
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
        public string code; //编号
        public string iconName; // 物体名字
        public string Edible;
        public string unit;// 计量单位
        public string heat;//热量
        public string field7;
        public string protein;//蛋白质
        public string fat;//脂肪
        public string carbohydrate;//碳水化合物
        //public int isReachStandard;//达标计数 1为不 2为达 3为超


    }
    // JSON 数组需要包装类
    [System.Serializable]
    private class Wrapper
    {
        public List<SerializableItemData> grain;//谷
        public List<SerializableItemData> bean;//豆
        public List<SerializableItemData> vegetable;//蔬菜
        public List<SerializableItemData> oil;//油
        public List<SerializableItemData> meat;//肉
        public List<SerializableItemData> water;//水


    }


}


