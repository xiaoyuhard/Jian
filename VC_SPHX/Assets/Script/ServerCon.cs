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
/// ʳ��ѡ��󣬷������ݵ������� Ȼ����յ�ѡ��󷵻ص�����
/// </summary>
public class ServerCon : MonoSingletonBase<ServerCon>
{
    // Ŀ���������ַ
    private string serverUrl = "http://your-api.com/data";

    private void Awake()
    {
        //StartCoroutine(LoadFoodPageQueryCoroutine("����", 21));

    }

    /// <summary>
    /// ���������������
    /// </summary>
    /// <param name="message"></param>
    /// <param name="full"></param>
    public void ConverToJsonPost(string message, string full)
    {

        string jsonArray = message;

        // ���� POST ����
        StartCoroutine(SendData(jsonArray, full));
    }


    IEnumerator SendData(string jsonBody, string full)
    {
        // ��ȡ������ַ
        //string baseUrl = ReadServerAddress();
        string baseUrl = "http://172.28.67.73:9090";
        //if (string.IsNullOrEmpty(baseUrl))
        //{
        //    Debug.LogError("��������ַ���ô���");
        //    yield break;
        //}
        string fullUrl = $"{CombineUrl(baseUrl, full)}";
        Debug.Log("ѡ����ɺ󷢸��������ĵ�ַ" + fullUrl + "  ���е����� " + jsonBody);
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
            ProcessServerResponse(responseJson, full);
        }
        else
        {
            Debug.LogError($"����ʧ�ܣ�{request.error}");
            UIManager.Instance.OpenUICaoZuo(UINameType.UI_ServerTip);

        }
    }
    private void ProcessServerResponse(string responseJson, string full)
    {
        // ʾ������������Ӧ
        Debug.Log($"��������Ӧ��{responseJson}");

        // ʵ�ʿ����з����л���Ӧ���ݣ����践�� JSON �ṹ��
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
                Debug.LogError("��ַ���� û�н��յ���Ӧ��ַ������   ��ַ:" + full);
                break;
        }

    }
    ThreeMeals user = new ThreeMeals();

    /// <summary>
    /// ���������ȡ����
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
            Debug.LogError("�����ļ�������: " + path);
        }
        catch (System.Exception e)
        {
            Debug.LogError("��ȡ����ʧ��: " + e.Message);
        }

        return null; // �򷵻�Ĭ�ϵ�ַ
    }



    public void LoadRecipe(string full, string page, string sendPath)
    {

        // ���� POST ����
        StartCoroutine(GetFoodRecipe(full, page, sendPath));
    }
    private IEnumerator GetFoodRecipe(string full, string page, string sendPath)
    {
        // ƴ��������ַ
        string fullUrl = $"{CombineUrl("http://172.28.67.73:9090", full)}" + page;
        using (UnityWebRequest request = UnityWebRequest.Get(fullUrl))
        {
            // ��������ͷ�������Ҫ��
            //request.SetRequestHeader("Content-Type", "application/json");
            request.timeout = 3; // ���ó�ʱʱ��
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                //try
                {
                    // ����JSON����
                    string jsonResponse = request.downloadHandler.text;
                    Debug.Log($"��������Ӧ��{jsonResponse}");
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
                            Debug.LogError("��ַ���� û�н��յ���Ӧ��ַ������   ��ַ:" + full);
                            break;
                    }

                }
                //catch (Exception e)
                {
                    //onError?.Invoke($"JSON����ʧ��: {e.Message}");
                    //Debug.LogError($"JSON����ʧ��: {e.Message}");
                }
            }
            else
            {
                //onError?.Invoke($"��������ʧ��: {request.error}");
                Debug.LogError($"��������ʧ��: {request.error}");
                UIManager.Instance.OpenUICaoZuo(UINameType.UI_ServerTip);


            }
        }
    }

}

// ��װ�ࣨ���ڴ�������/�б����л���
[System.Serializable]
class Wrapper<T>
{
    public T items;
}