using Excel;
using RTS;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

//������ʾ����
public class DataManager : MonoSingletonBase<DataManager>
{
    // �洢���� ItemData ���б�
    public List<EquipmentItemData> itemList = new List<EquipmentItemData>();
    //private Dictionary<string, EquipmentItemData> itemDictionary = new Dictionary<string, EquipmentItemData>();
    public Dictionary<string, List<EquipmentItemData>> itemDictionary = new Dictionary<string, List<EquipmentItemData>>();

    public string jsonName;
    private string jsonFilePath => Path.Combine(Application.streamingAssetsPath, jsonName + ".json");

    // �洢���� ItemData ���б�
    //private Dictionary<string, EquipmentItemData> itemDictionary = new Dictionary<string, EquipmentItemData>();
    //private Dictionary<DataCategory, List<FoodKindItemData>> itemDictionary = new Dictionary<DataCategory, List<FoodKindItemData>>();

    void Awake()
    {
        InitializeManager();
        LoadAllExcelData();
    }

    // ��ʽ��ʼ�����������༭�����ã�
    public void InitializeManager()
    {
        if (Instance == null)
        {
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
            //Instance = null;
        }
    }
#endif

    public void LoadAllExcelData()
    {
        itemDictionary.Clear();

        string streamingAssetsPath = Application.streamingAssetsPath;
        string tishi_path = "/ExperimentalHint/ʵ����ʾ.xlsx";
        tishi_path = streamingAssetsPath + tishi_path;
        string pathMoxing = "/ExperimentalHint/ģ��չʾ.xlsx";
        pathMoxing = streamingAssetsPath + pathMoxing;

        if (File.Exists(tishi_path))
        {
            itemDictionary.Add("��������ʾ", ParseSheet(tishi_path, "��������ʾ"));
            itemDictionary.Add("������ʾ", ParseSheet(tishi_path, "������ʾ"));
            itemDictionary.Add("�ؽ�����ʾ", ParseSheet(tishi_path, "�ؽ�����ʾ"));
        }

        if (File.Exists(pathMoxing))
        {
            itemDictionary.Add("Anjisuan", ParseSheet(pathMoxing, "������"));
            itemDictionary.Add("Xiangqi", ParseSheet(pathMoxing, "����"));
            itemDictionary.Add("Zhongjinshu", ParseSheet(pathMoxing, "�ؽ���"));
            itemDictionary.Add("Shachongji", ParseSheet(pathMoxing, "ɱ���"));
            itemDictionary.Add("Huanyuantang", ParseSheet(pathMoxing, "��ԭ��"));
            itemDictionary.Add("Zhifang", ParseSheet(pathMoxing, "֬��"));
            itemDictionary.Add("Danbaizhi", ParseSheet(pathMoxing, "������"));
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
                        // ������ͷ������ʵ�����������
                        reader.Read(); // ������һ�У�������

                        while (reader.Read())
                        {
                            // ������ֶ�ӳ���ֶ�
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
            // ���� Reader��ָ����ȡ .xlsx �ļ���
            using (var reader = ExcelReaderFactory.CreateOpenXmlReader(stream))
            {
                // ��ȡ����Ϊ DataSet
                var result = reader.AsDataSet();
                var dataTable = result.Tables[0]; // ���������ڵ�һ��������

                // �����У�������ͷ��
                for (int row = 1; row < dataTable.Rows.Count; row++)
                {
                    var rowData = dataTable.Rows[row];
                    // �� Excel ������ӳ�䵽 FoodKindItemData
                    EquipmentItemData item = new EquipmentItemData
                    {
                        id = rowData[0].ToString(),
                        parent = rowData[1].ToString(),
                        introduce = rowData[2].ToString(),
                        iconName = rowData[3].ToString()
                        // ����ʵ����˳�����
                    };
                    dataList.Add(item);
                }
            }

            itemDictionary[tableName] = dataList;

        }
    }




    #region
    // �������ݵ� JSON �ļ�
    public void SaveDataToJson(EquipmentItemData data, string filePath)
    {
        List<SerializableItemData> serializableList = new List<SerializableItemData>();
        // ���������л���������
        foreach (EquipmentItemData item in itemList)
        {
            serializableList.Add(new SerializableItemData
            {
                id = data.id,
                parent = data.parent,
                introduce = data.introduce,
                iconName = item.iconName // ���� Sprite ����
            });
        }


        // ת��Ϊ JSON �ַ���
        string json = JsonUtility.ToJson(new Wrapper { items = serializableList }, true);
        // д���ļ�
        File.WriteAllText(jsonFilePath, json);
        Debug.Log($"�����ѱ��浽: {jsonFilePath}");
    }


    // �� JSON �������ݵ��ڴ�
    public void LoadAllFromJson()
    {
        itemDictionary.Clear();

        if (!File.Exists(jsonFilePath))
        {
            Debug.LogError($"�ļ�������: {jsonFilePath}");
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


        Debug.Log($"�Ѽ��� {itemDictionary.Count} ������");
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

            // ���� Sprite������ͼ���� Resources/Icons �£�
            if (!string.IsNullOrEmpty(sData.iconName))
            {
                item.iconName = sData.iconName;
            }
            equipmentList.Add(item);
        }
        itemDictionary.Add(id, equipmentList);

    }

    #endregion

    // �� ID ��ѯ����
    public List<EquipmentItemData> GetItemById(string id)
    {
        if (itemDictionary.TryGetValue(id, out List<EquipmentItemData> data))
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
        public string parent;
        public string introduce;
        public string iconName; // �洢 Sprite ·��������
    }
    // JSON ������Ҫ��װ��
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
