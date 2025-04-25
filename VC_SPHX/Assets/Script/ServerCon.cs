using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using RTS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;


/// <summary>
/// ʳ��ѡ��󣬷������ݵ������� Ȼ����յ�ѡ��󷵻ص�����
/// </summary>
public class ServerCon : MonoSingletonBase<ServerCon>
{
    // Ŀ���������ַ
    private string serverUrl = "http://your-api.com/data";

    private void Awake()
    {
        StartCoroutine(LoadFoodPageQueryCoroutine("����", 21));

    }

    public void ConverToJson(FoodSendConverDay foods)
    {

        string jsonArray = SerializeData(foods);

        // ���� POST ����
        StartCoroutine(SendData(jsonArray));
    }
    private string SerializeData(FoodSendConverDay data)
    {
        // ����һ��ʹ�� Unity ���� JsonUtility����Ҫ��װ�ࣩ
        Wrapper<FoodSendConverDay> wrapper = new Wrapper<FoodSendConverDay> { items = data };
        //return JsonUtility.ToJson(wrapper);

        // ��������ʹ�� Newtonsoft.Json��ֱ�����л��б�
        return JsonConvert.SerializeObject(data);
    }


    IEnumerator SendData(string jsonBody)
    {
        // ��ȡ������ַ
        string baseUrl = ReadServerAddress();
        if (string.IsNullOrEmpty(baseUrl))
        {
            Debug.LogError("��������ַ���ô���");
            yield break;
        }
        string fullUrl = $"{CombineUrl(baseUrl, "/analyse/intake")}";

        UnityWebRequest request = new UnityWebRequest(fullUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonBody);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("���ݷ��ͳɹ���");
            string responseJson = request.downloadHandler.text;
            ProcessServerResponse(responseJson);
        }
        else
        {
            Debug.LogError($"����ʧ�ܣ�{request.error}");
        }
    }
    private void ProcessServerResponse(string responseJson)
    {
        // ʾ������������Ӧ
        Debug.Log($"��������Ӧ��{responseJson}");

        // ʵ�ʿ����з����л���Ӧ���ݣ����践�� JSON �ṹ��
        // ServerResponse response = JsonUtility.FromJson<ServerResponse>(responseJson);

        FoodRecriveConverDay response = JsonConvert.DeserializeObject<FoodRecriveConverDay>(responseJson);
        ChooseFoodAllInformCon.Instance.ReceiveServerInform(response);
    }


    private string CombineUrl(string baseUrl, string path)
    {
        return $"{baseUrl.TrimEnd('/')}/{path.TrimStart('/')}";
    }


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
        string fullUrl = $"{CombineUrl(baseUrl, "/food/getEnergy")}";
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



    // ��װ�ࣨ���ڴ�������/�б����л���
    [System.Serializable]
    private class Wrapper<T>
    {
        public T items;
    }
}
