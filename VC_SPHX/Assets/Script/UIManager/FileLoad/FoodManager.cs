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
/// ʳ����� ���ձ�������ʳ������
/// </summary>
public class FoodManager : MonoBehaviour
{
    public static FoodManager Instance { get; private set; }

    // �洢���� ItemData ���б�
    public List<FoodKindItemData> itemList = new List<FoodKindItemData>();
    //private Dictionary<string, EquipmentItemData> itemDictionary = new Dictionary<string, EquipmentItemData>();
    public Dictionary<string, List<FoodKindItemData>> itemDictionary = new Dictionary<string, List<FoodKindItemData>>();
    //private Dictionary<DataCategory, List<FoodKindItemData>> itemDictionary = new Dictionary<DataCategory, List<FoodKindItemData>>();
    public string excelFolderPath = Application.streamingAssetsPath;
    public string jsonName;
    private string jsonFilePath => Path.Combine(Application.streamingAssetsPath, jsonName + ".xlsx");

    // ������API��ַ��ʾ����
    //private const string API_URL = "https://your-api-domain.com/food/items";


    void Awake()
    {
        InitializeManager();
    }

    // ��ʽ��ʼ�����������༭�����ã�
    public void InitializeManager()
    {
        if (Instance == null)
        {
            Instance = this;
            //LoadAllExcelData();
            LoadFoodData();
            // ȷ���༭���˳�ʱ����ʵ��
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
            Debug.LogError("�����ļ�������: " + path);
        }
        catch (System.Exception e)
        {
            Debug.LogError("��ȡ����ʧ��: " + e.Message);
        }

        return null; // �򷵻�Ĭ�ϵ�ַ
    }

    // �������ݵĹ�������
    public void LoadFoodData()
    {
        itemDictionary.Clear();
        StartCoroutine(LoadFoodPageQueryCoroutine("������", 138));
        StartCoroutine(LoadFoodPageQueryCoroutine("�߲�ˮ����", 561));
        StartCoroutine(LoadFoodPageQueryCoroutine("�����㵰��", 786));
        StartCoroutine(LoadFoodPageQueryCoroutine("�󶹼������", 145));
        StartCoroutine(LoadFoodPageQueryCoroutine("��֬��", 26));
        StartCoroutine(LoadFoodPageQueryCoroutine("ˮ", 22));
    }

    private IEnumerator LoadFoodPageQueryCoroutine(string categoryName, int pageSize)
    {
        // ��ȡ������ַ
        string baseUrl = ReadServerAddress();
        if (string.IsNullOrEmpty(baseUrl))
        {
            Debug.LogError("��������ַ���ô���");
            yield break;
        }
    
        // ƴ��������ַ
        //string fullUrl = $"{CombineUrl(baseUrl, "/food/pageQuery")}?CategoryName={categoryName}&pageSize={pageSize}";
        string fullUrl = $"{CombineUrl("http://172.28.67.73:9090", "/food/pageQuery")}?CategoryName={categoryName}&pageSize={pageSize}";
        using (UnityWebRequest request = UnityWebRequest.Get(fullUrl))
        {
            // ��������ͷ�������Ҫ��
            //request.SetRequestHeader("Content-Type", "application/json");
            request.timeout = 3; // ���ó�ʱʱ��
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                try
                {
                    // ����JSON����
                    string jsonResponse = request.downloadHandler.text;
                    FoodResponse response = JsonConvert.DeserializeObject<FoodResponse>(jsonResponse);

                    //FoodResponse response = JsonUtility.FromJson<FoodResponse>(jsonResponse);
                    //itemDictionary.Add(categoryName, response.rows);
                    // �����Ƿ����
                    if (!itemDictionary.ContainsKey(categoryName))
                    {
                        // �����¼�����ʼ���б�
                        itemDictionary.Add(categoryName, new List<FoodKindItemData>());
                    }

                    // ���б����������
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
                    //onError?.Invoke($"JSON����ʧ��: {e.Message}");
                    Debug.LogError($"JSON����ʧ��: {e.Message}");
                }
            }
            else
            {
                //onError?.Invoke($"��������ʧ��: {request.error}");
                Debug.LogError($"��������ʧ��: {request.error}");

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

        //// ��ȡ����·��
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
        // ��ȡ����·��
        string fullPath = Path.Combine(Application.dataPath, excelFolderPath);

        if (Directory.Exists(fullPath))
        {
            string path = "/ʳ��ɷֱ�.xlsx";

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
            // ���� Reader��ָ����ȡ .xlsx �ļ���
            using (var reader = ExcelReaderFactory.CreateOpenXmlReader(stream))
            {
                // ��ȡ����Ϊ DataSet
                var result = reader.AsDataSet();
                var dataTable = result.Tables[excelContent]; // ���������ڵ�һ��������

                // �����У�������ͷ��
                for (int row = 1; row < dataTable.Rows.Count; row++)
                {
                    var rowData = dataTable.Rows[row];
                    // �� Excel ������ӳ�䵽 FoodKindItemData
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
                        // ����ʵ����˳�����
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



    // �������ݵ� JSON �ļ�
    public void SaveDataToJson(FoodKindItemData data, string filePath)
    {
        List<SerializableItemData> serializableList = new List<SerializableItemData>();
        // ���������л���������
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


        // ת��Ϊ JSON �ַ���
        //string json = JsonUtility.ToJson(new Wrapper { items = serializableList }, true);
        // д���ļ�
        //File.WriteAllText(jsonFilePath, json);
        Debug.Log($"�����ѱ��浽: {jsonFilePath}");
    }


    //// �� JSON �������ݵ��ڴ�
    //public void LoadAllFromJson()
    //{
    //    itemDictionary.Clear();

    //    if (!File.Exists(jsonFilePath))
    //    {
    //        Debug.LogError($"�ļ�������: {jsonFilePath}");
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



    //    Debug.Log($"�Ѽ��� {itemDictionary.Count} ������");

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

            //// ���� Sprite������ͼ���� Resources/Icons �£�
            //if (!string.IsNullOrEmpty(sData.iconName))
            //{
            //    item.iconName = sData.iconName;
            //}
            //foodKindList.Add(item);
        }
        //itemDictionary.Add(id, foodKindList);

    }

    #endregion

    //�� ID ��ѯ����
    public List<FoodKindItemData> GetItemById(string id)
    {
        if (itemDictionary.TryGetValue(id, out List<FoodKindItemData> data))
        {
            return data;
        }
        Debug.Log($"δ�ҵ� ID Ϊ {id} ������");
        return null;
    }

    [System.Serializable]
    private class SerializableItemData
    {
        public string id;
        public string code; //���
        public string iconName; // ��������
        public string Edible;
        public string unit;// ������λ
        public string heat;//����
        public string field7;
        public string protein;//������
        public string fat;//֬��
        public string carbohydrate;//̼ˮ������
        //public int isReachStandard;//������ 1Ϊ�� 2Ϊ�� 3Ϊ��


    }
    // JSON ������Ҫ��װ��
    [System.Serializable]
    private class Wrapper
    {
        public List<SerializableItemData> grain;//��
        public List<SerializableItemData> bean;//��
        public List<SerializableItemData> vegetable;//�߲�
        public List<SerializableItemData> oil;//��
        public List<SerializableItemData> meat;//��
        public List<SerializableItemData> water;//ˮ


    }


}


