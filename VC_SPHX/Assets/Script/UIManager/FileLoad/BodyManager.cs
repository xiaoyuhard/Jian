using Excel;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;


/// <summary>
/// ����������ݹ����� ���ж�ȡexcel����
/// </summary>
public class BodyManager : MonoBehaviour
{
    public static BodyManager Instance { get; private set; }

    // �洢���� ItemData ���б�
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

    // ��ʽ��ʼ�����������༭�����ã�
    public void InitializeManager()
    {
        if (Instance == null)
        {
            Instance = this;
            LoadAllExcelData();

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

    public void LoadAllExcelData()
    {
        //itemDictionary.Clear();

        // ��ȡ����·��
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

            //string path = "/StreamingAssets/����/�������.xlsx";

            //// �����ڷ���ϵͳ
            //var endocrineData = ParseSheet<EndocrineSystemData>(fullPath + path, "�ڷ���ϵͳ");
            //var breatheData = ParseSheet<EndocrineSystemData>(fullPath + path, "����ϵͳ");
            //var loopData = ParseSheet<EndocrineSystemData>(fullPath + path, "ѭ��ϵͳ");
            //var urinaryData = ParseSheet<EndocrineSystemData>(fullPath + path, "����ϵͳ");
            //var digestionData = ParseSheet<EndocrineSystemData>(fullPath + path, "����ϵͳ");
            //var lymphData = ParseSheet<EndocrineSystemData>(fullPath + path, "�ܰ�ϵͳ");
            //var procreationData = ParseSheet<EndocrineSystemData>(fullPath + path, "��ֳϵͳ");
            //var nerveData = ParseSheet<EndocrineSystemData>(fullPath + path, "��ϵͳ");
            //var epitheliumData = ParseSheet<EndocrineSystemData>(fullPath + path, "��Ƥ��֯");

            //itemEndocrDic.Add("�ڷ���ϵͳ", endocrineData);
            //itemEndocrDic.Add("����ϵͳ", breatheData);
            //itemEndocrDic.Add("ѭ��ϵͳ", loopData);
            //itemEndocrDic.Add("����ϵͳ", urinaryData);
            //itemEndocrDic.Add("����ϵͳ", digestionData);
            //itemEndocrDic.Add("�ܰ�ϵͳ", lymphData);
            //itemEndocrDic.Add("��ֳϵͳ", procreationData);
            //itemEndocrDic.Add("��ϵͳ", nerveData);
            //itemEndocrDic.Add("��Ƥ��֯", epitheliumData);

            //// ��������ϵͳ
            //var skeletalData = ParseSheet<BoneHierarchy>(fullPath + path, "�˶�ϵͳ_����");
            //var feelData = ParseSheet<BoneHierarchy>(fullPath + path, "�о�ϵͳ");
            //var muscleData = ParseSheet<BoneHierarchy>( fullPath + path, "������֯");
            //itemBoneDic.Add("�˶�ϵͳ_����", skeletalData);
            //itemBoneDic.Add("�о�ϵͳ", feelData);
            //itemBoneDic.Add("������֯", muscleData);

            string pathBodyExpl = "/StreamingAssets/����/BodyExpl.xlsx";

            //var man = ParseSheet<BodyItem>(fullPath + pathBodyExpl, "Man");
            //var woman = ParseSheet<BodyItem>(fullPath + pathBodyExpl, "Woman");
            //itemBodyExplic.Add("Man", man);
            //itemBodyExplic.Add("Woman", woman);
        }
    }


    /// <summary>
    /// ͨ��CSV���ط���
    /// </summary>
    /// <param name="filePath">�ļ�·��������Ҫ��չ���������Resources�ļ��У�</param>
    /// <param name="skipHeader">�Ƿ����������У�Ĭ��������</param>
    public static List<T> LoadCSVData<T>(string filePath, bool skipHeader, int count) where T : new()
    {
        List<T> dataList = new List<T>();

        try
        {

            // ����CSV�ı��ļ� ---------------------------------------------------
            TextAsset csvFile = Resources.Load<TextAsset>(filePath);

            if (csvFile == null)
            {
                Debug.LogError($"CSV�ļ�δ�ҵ�: {filePath}");
                return dataList;
            }

            // ʹ��������ʽ��ָ���CSV��ʽ ----------------------------------------
            // ����˵��������������š����з����ֶΣ����磺"aaa,bbb"��
            Regex csvSplit = new Regex("(?:^|,)(\"(?:[^\"]+|\"\")*\"|[^,]*)");
            //Debug.Log(csvFile.text);

            // ���н��� ----------------------------------------------------------
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
                        // �������к�ע���У���#��ͷ��
                        if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#")) continue;
                        // ����������
                        if (isFirstLine && skipHeader)
                        {
                            isFirstLine = false;
                            continue;
                        }

                        // ʹ������ƥ�����ֶ� ----------------------------------------
                        List<string> fields = new List<string>();
                        MatchCollection matches = csvSplit.Matches(line);

                        foreach (Match match in matches)
                        {
                            string field = match.Value;

                            // ����ͷ���ŵ����
                            if (field.StartsWith(","))
                                field = field.Substring(1);

                            //// ȥ���ֶ����˵����ţ�������ڣ�
                            //if (field.StartsWith("\"") && field.EndsWith("\""))
                            //    field = field.Substring(1, field.Length - 2);
                            // �ϲ������裺ֱ����ȡ�������� + ȥ�����˶���/����
                            //string field = match.Groups[2].Value                     // �����򲶻�����ȡ��������
                            //    .TrimStart(',')                                      // ȥ����ͷ�Ķ���
                            //    .Trim('"');                                          // ȥ�����˵�����

                            fields.Add(field);
                        }

                        // ���ֶ�ӳ�䵽������ ------------------------------------------
                        T dataItem = new T();
                        System.Type type = typeof(T);

                        // ͨ�������Զ�ӳ���ֶΣ�Ҫ���ֶ�˳����CSV��˳��һ�£�
                        System.Reflection.FieldInfo[] fieldsInfo = type.GetFields();

                        for (int i = 0; i < Mathf.Min(fields.Count, fieldsInfo.Length); i++)
                        {
                            try
                            {
                                // ����ת������
                                object value = ConvertValue(fields[i], fieldsInfo[i].FieldType);
                                fieldsInfo[i].SetValue(dataItem, value);
                            }
                            catch (System.Exception e)
                            {
                                Debug.LogError($"�ֶν������� ��: {line}\n����: {e.Message}");
                            }
                        }

                        dataList.Add(dataItem);
                    }
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"CSV����ʧ��: {e.Message}");
        }

        return dataList;
    }
    /// <summary>
    /// ����ת����������
    /// </summary>
    private static object ConvertValue(string valueString, System.Type targetType)
    {
        if (string.IsNullOrEmpty(valueString)) return null;

        try
        {
            // ������������
            if (targetType == typeof(string))
                return valueString;

            if (targetType == typeof(int))
                return int.Parse(valueString);

            if (targetType == typeof(float))
                return float.Parse(valueString);

            if (targetType == typeof(bool))
                return valueString.ToLower() == "true";

            // ��Ӹ�������ת������...
        }
        catch
        {
            Debug.LogError($"�޷�ת��ֵ: {valueString} ������ {targetType.Name}");
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
                        // ������ͷ������ʵ�����������
                        reader.Read(); // ������һ�У�������

                        while (reader.Read())
                        {
                            T data = new T();
                            // ������ֶ�ӳ���ֶ�
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
                    //// �� Excel ������ӳ�䵽 FoodKindItemData
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
                    //    // ����ʵ����˳�����
                    //};
                    //dataList.Add(item);
                }
            }

            //itemDictionary[tableName] = dataList;

        }
    }


    //�� ID ��ѯ����
    public List<EndocrineSystemData> GetEndocrItemById(string id)
    {
        if (itemEndocrDic.TryGetValue(id, out List<EndocrineSystemData> data))
        {
            return data;
        }
        Debug.Log($"δ�ҵ� ID Ϊ {id} ������");
        return null;
    }
    //�� ID ��ѯ����
    public List<BoneHierarchy> GetBoneItemById(string id)
    {
        if (itemBoneDic.TryGetValue(id, out List<BoneHierarchy> data))
        {
            return data;
        }
        Debug.Log($"δ�ҵ� ID Ϊ {id} ������");
        return null;
    }

    //�� ID ��ѯ����
    public List<BodyItem> GetBodyExplItemById(string id)
    {
        if (itemBodyExplic.TryGetValue(id, out List<BodyItem> data))
        {
            return data;
        }
        Debug.Log($"δ�ҵ� ID Ϊ {id} ������");
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

