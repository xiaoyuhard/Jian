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
/// 食物选完后，发送数据到服务器 然后接收到选完后返回的总量
/// </summary>
public class ServerCon : MonoSingletonBase<ServerCon>
{
    // 目标服务器地址
    private string serverUrl = "http://your-api.com/data";

    private void Awake()
    {
        StartCoroutine(LoadFoodPageQueryCoroutine("其他", 21));

    }

    public void ConverToJson(FoodSendConverDay foods)
    {

        string jsonArray = SerializeData(foods);

        // 发送 POST 请求
        StartCoroutine(SendData(jsonArray));
    }
    private string SerializeData(FoodSendConverDay data)
    {
        // 方法一：使用 Unity 内置 JsonUtility（需要包装类）
        Wrapper<FoodSendConverDay> wrapper = new Wrapper<FoodSendConverDay> { items = data };
        //return JsonUtility.ToJson(wrapper);

        // 方法二：使用 Newtonsoft.Json（直接序列化列表）
        return JsonConvert.SerializeObject(data);
    }


    IEnumerator SendData(string jsonBody)
    {
        // 获取基础地址
        string baseUrl = ReadServerAddress();
        if (string.IsNullOrEmpty(baseUrl))
        {
            Debug.LogError("服务器地址配置错误");
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
            Debug.Log("数据发送成功！");
            string responseJson = request.downloadHandler.text;
            ProcessServerResponse(responseJson);
        }
        else
        {
            Debug.LogError($"发送失败：{request.error}");
        }
    }
    private void ProcessServerResponse(string responseJson)
    {
        // 示例：解析简单响应
        Debug.Log($"服务器响应：{responseJson}");

        // 实际开发中反序列化响应数据（假设返回 JSON 结构）
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
            Debug.LogError("配置文件不存在: " + path);
        }
        catch (System.Exception e)
        {
            Debug.LogError("读取配置失败: " + e.Message);
        }

        return null; // 或返回默认地址
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
        string fullUrl = $"{CombineUrl(baseUrl, "/food/getEnergy")}";
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



    // 包装类（用于处理数组/列表序列化）
    [System.Serializable]
    private class Wrapper<T>
    {
        public T items;
    }
}
