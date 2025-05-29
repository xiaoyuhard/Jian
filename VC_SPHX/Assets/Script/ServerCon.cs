using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using RTS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
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
        //StartCoroutine(LoadFoodPageQueryCoroutine("其他", 21));

    }

    /// <summary>
    /// 向服务器发送数据
    /// </summary>
    /// <param name="message"></param>
    /// <param name="full"></param>
    public void ConverToJsonPost(string message, string full)
    {

        string jsonArray = message;

        // 发送 POST 请求
        StartCoroutine(SendData(jsonArray, full));
    }


    IEnumerator SendData(string jsonBody, string full)
    {
        // 获取基础地址
        //string baseUrl = ReadServerAddress();
        string baseUrl = "http://172.28.67.73:9090";
        //if (string.IsNullOrEmpty(baseUrl))
        //{
        //    Debug.LogError("服务器地址配置错误");
        //    yield break;
        //}
        string fullUrl = $"{CombineUrl(baseUrl, full)}";
        Debug.Log("选择完成后发给服务器的地址" + fullUrl + "  所有的数据 " + jsonBody);
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
            ProcessServerResponse(responseJson, full);
        }
        else
        {
            Debug.LogError($"发送失败：{request.error}");
            UIManager.Instance.OpenUICaoZuo(UINameType.UI_ServerTip);

        }
    }
    private void ProcessServerResponse(string responseJson, string full)
    {
        // 示例：解析简单响应
        Debug.Log($"服务器响应：{responseJson}");

        // 实际开发中反序列化响应数据（假设返回 JSON 结构）
        // ServerResponse response = JsonUtility.FromJson<ServerResponse>(responseJson);

        switch (full)
        {
            case "/analyse/intake":
                FoodRecriveConverDay response = JsonConvert.DeserializeObject<FoodRecriveConverDay>(responseJson);
                if (response.code == 200)
                {
                    ChooseFoodAllInformCon.Instance.ReceiveServerInform(response);
                }
                else
                {
                    UIManager.Instance.OpenUICaoZuo(UINameType.UI_ServerTip);
                }
                break;
            case "/analyse/nutrition/plan":
                user = JsonConvert.DeserializeObject<ThreeMeals>(responseJson);
                if (user.code == 200)
                {
                    YiTiJiUI.Instance.ShowBiLiScroll(user);
                    ChooseFoodAllInformCon.Instance.SetThreeMeals(user);
                }
                else
                {
                    YiTiJiUI.Instance.isValid = false;

                    UIManager.Instance.OpenUICaoZuo(UINameType.UI_ServerTip);
                }
                break;
            case "/group/analyse/heatIntake":
                ReceiveHeatIntake receiveHeatIntake = JsonConvert.DeserializeObject<ReceiveHeatIntake>(responseJson);
                if (receiveHeatIntake.code == 200)
                {
                    GroupRegisterUI.Instance.ReceiveRecIntake(receiveHeatIntake.data);

                }
                else
                {
                    UIManager.Instance.OpenUICaoZuo(UINameType.UI_ServerTip);
                }
                break;

            default:
                Debug.LogError("地址错误 没有接收到对应地址的数据   地址:" + full);
                break;
        }

    }
    ThreeMeals user = new ThreeMeals();

    /// <summary>
    /// 向服务器获取数据
    /// </summary>
    /// <param name="baseUrl"></param>
    /// <param name="path"></param>
    /// <returns></returns>
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



    public void LoadRecipe(string full, string page, string sendPath)
    {

        // 发送 POST 请求
        StartCoroutine(GetFoodRecipe(full, page, sendPath));
    }
    private IEnumerator GetFoodRecipe(string full, string page, string sendPath)
    {
        // 拼接完整地址
        string fullUrl = $"{CombineUrl("http://172.28.67.73:9090", full)}" + page;
        using (UnityWebRequest request = UnityWebRequest.Get(fullUrl))
        {
            // 设置请求头（如果需要）
            //request.SetRequestHeader("Content-Type", "application/json");
            request.timeout = 3; // 设置超时时间
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                //try
                {
                    // 解析JSON数据
                    string jsonResponse = request.downloadHandler.text;
                    Debug.Log($"服务器响应：{jsonResponse}");
                    switch (full)
                    {
                        case "/cookbook/getRecipeFood":
                            RecipeAllFood recipeAll = JsonConvert.DeserializeObject<RecipeAllFood>(jsonResponse);
                            FoodChooseUI.Instance.ReceiveRecipeItem(recipeAll.data);
                            break;
                        case "/cookbook/getGroupRecipeFood":
                            switch (sendPath)
                            {
                                case "Choose":
                                    FoodRecipeGroupResponse recipeAllGroup = JsonConvert.DeserializeObject<FoodRecipeGroupResponse>(jsonResponse);
                                    FoodGroupChooseUI.Instance.ReceiveRecipeItem(recipeAllGroup.data);
                                    break;
                                case "Swop":
                                    FoodRecipeGroupResponse recipeAllGroupSwop = JsonConvert.DeserializeObject<FoodRecipeGroupResponse>(jsonResponse);
                                    SwopRecipeGroupUI.Instance.ReceiveRecipeItem(recipeAllGroupSwop.data);
                                    break;
                               
                            }
                       
                            break;
                        case "/group/analyse/getAllPhysique":
                            GetAllPhysique getAllPhysique = JsonConvert.DeserializeObject<GetAllPhysique>(jsonResponse);
                            GroupRegisterUI.Instance.ServerGetAllPhysique(getAllPhysique.data);
                            break;

                        default:
                            Debug.LogError("地址错误 没有接收到对应地址的数据   地址:" + full);
                            break;
                    }

                }
                //catch (Exception e)
                {
                    //onError?.Invoke($"JSON解析失败: {e.Message}");
                    //Debug.LogError($"JSON解析失败: {e.Message}");
                }
            }
            else
            {
                //onError?.Invoke($"网络请求失败: {request.error}");
                Debug.LogError($"网络请求失败: {request.error}");
                UIManager.Instance.OpenUICaoZuo(UINameType.UI_ServerTip);


            }
        }
    }

}

// 包装类（用于处理数组/列表序列化）
[System.Serializable]
class Wrapper<T>
{
    public T items;
}