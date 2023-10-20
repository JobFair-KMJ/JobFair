using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UnityEngine;

[SerializeField]

public class OAI_Chat : MonoBehaviour
{
    public string OpenAI_Key = "sk-api키";
    public bool Has_DefinedMessageJsonDataFile = false;
    public int max_tokens = 1000;


    private static HttpClient client;
    private static string JsonFilelocation = Application.streamingAssetsPath + "/Json/JsonData.json";
    private float requestInterval = 0.1f; // 요청 간격을 0.1초로 설정

    public delegate void stringEvent(string _string);
    public stringEvent CompletedRepostEvent;

    private const string ApiUrl = "https://api.openai.com/v1/chat/completions";
    private const string AuthorizationHeader = "Bearer";
    private const string UserAgentHeader = "User-Agent";

    /// <summary>
    /// 미리 인스펙터에서 설정해둘 대화형 
    /// </summary>
    [JsonProperty("message")]
    public List<message> DefineMessage;

    DefinedmessageClass definedmessageClass = new DefinedmessageClass();
    public OAPI_DATA Oapi_data { get { return OpenAI_DATA; } set { OpenAI_DATA = value; } }
    private OAPI_DATA OpenAI_DATA = new OAPI_DATA();

    private void Awake()
    {

        if (Has_DefinedMessageJsonDataFile)
        {
            definedmessageClass = JsonConvert.DeserializeObject<DefinedmessageClass>(JsonFilelocation);
            // definedmessageClass = JsonParser.Load<DefinedmessageClass>(JsonFilelocation);
            definedmessageClass.RefineMessageData(ref DefineMessage);
        }
    }
    private void Start()
    {
        InitReqestData();
    }

    public void DataAdd(string text, message.role _role = message.role.assistant)
    {
        Oapi_data.Messages.Add(new message { Role = _role, Message = text });
    }
    public void DataErease(int _number, int _count)
    {
        Oapi_data.Messages.RemoveRange(_number, _count);
    }
    public void ResetData()
    {
        Oapi_data.Resetmessages();
    }
    public void InitReqestData()
    {

        if (DefineMessage?.Count > 0)
        {
            Oapi_data.ConceptSettingMessages = DefineMessage;
        }
        Oapi_data.Messages = new List<message>(Oapi_data.ConceptSettingMessages);
        Oapi_data.max_tokens = max_tokens;
    }
    public string ReqestStringData(string _messageString, bool _dataAutoAdd = true, bool _sendMessageDebugLog = false)
    {
        string ReqestStringData;

        DataAdd(_messageString, message.role.user);
        var responsData = ClieantResponse(Oapi_data, _sendMessageDebugLog);
        ReqestStringData = responsData?.Result.Message[0].datamessage.Content;
        if (_dataAutoAdd)
        {
            DataAdd(ReqestStringData);
        }
        if (CompletedRepostEvent != null)
            CompletedRepostEvent(ReqestStringData);
        return ReqestStringData;
    }

    /// <summary>
    /// 대화 메시지 보내면 자동으로 
    /// </summary>
    /// <param name="messageString">user가 입력한 string 값 </param>
    /// <param name="DataAutoAdd"> 데이터 자동 으로 Messages 의 List에 추가</param>
    /// <returns>받은 데이터</returns>
    public async Task<string> AsyncReqesStringtData(string _messageString, bool _dataAutoAdd = true, bool _sendMessageDebugLog = false)
    {
        string ReqestStringData;

        DataAdd(_messageString, message.role.user);
        var responsData = await ClieantResponse(Oapi_data, _sendMessageDebugLog);
        ReqestStringData = responsData?.Message[0].datamessage.Content;

        if (_dataAutoAdd)
        {
            DataAdd(ReqestStringData);
        }
        if (CompletedRepostEvent != null)
            CompletedRepostEvent(ReqestStringData);
        return ReqestStringData;
    }

    private void CreateHttpClient()
    {
        client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(AuthorizationHeader, OpenAI_Key);
        client.DefaultRequestHeaders.Add(UserAgentHeader, "okgodoit/dotnet_openai_api");
    }

    private async Task<CompletionResult_OAI> ClieantResponse(OAPI_DATA request, bool SendMessageDebugLog)
    {
        if (client == null)
        {
            CreateHttpClient();
        }

        string jsonContent = JsonConvert.SerializeObject(request, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
        var stringContent = new StringContent(jsonContent, UnicodeEncoding.UTF8, "application/json");
        if (SendMessageDebugLog)
        {
            UnityEngine.Debug.Log(jsonContent);
        }
        var response = await client.PostAsync(ApiUrl, stringContent);

        await Task.Delay((int)(requestInterval * 1000)); // 요청 간격만큼 대기

        if (response.IsSuccessStatusCode)
        {
            string resultAsString = await response.Content.ReadAsStringAsync();
            UnityEngine.Debug.Log(resultAsString);
            ResetData();
            var resultData = JsonConvert.DeserializeObject<CompletionResult_OAI>(resultAsString);
            return resultData;
        }
        else
        {
            throw new HttpRequestException("Error calling OpenAi API to get completion.  HTTP status code: " + response.StatusCode.ToString() + ". Request body: " + jsonContent);
        }
    }


}


/// <summary>
/// json 메시지를 보낼 형식 구성
/// </summary>
[Serializable]
public class OAPI_DATA
{

    [JsonProperty("model")]
    public string Model = "gpt-3.5-turbo-0301";
    [JsonProperty("messages")]
    public List<message> Messages { get; set; } = new List<message>();
    //추가 옵션
    //[JsonProperty("temperature"), XmlAttribute("temperature")]
    //public int temperature = 1;
    //[JsonProperty("top_p"), XmlAttribute("top_p")]
    //public int top_p = 1;
    //[JsonProperty("n"), XmlAttribute("n")]
    //public int n = 1;
    //[JsonProperty("stream"), XmlAttribute("stream")]
    //public bool stream = false;
    //[JsonProperty("stop"), XmlAttribute("stop")]
    //public List<string> stop;
    [JsonProperty("max_tokens"), XmlAttribute("max_tokens")]
    public int max_tokens = 100;
    //[JsonProperty("presence_penalty"), XmlAttribute("presence_penalty")]
    //public int presence_penalty = 0;
    //[JsonProperty("frequency_penalty"), XmlAttribute("frequency_penalty")]
    //public int frequency_penalty = 0;
    //[JsonProperty("logit_bias"), XmlAttribute("logit_bias")]
    //public int logit_bias = 1;
    //[JsonProperty("user"), XmlAttribute("user")]
    //public string user = "00";
    [JsonIgnore]
    public List<message> ConceptSettingMessages { get; set; } = new List<message>();
    public void Resetmessages()
    {
        Messages.Clear();
        Messages = new List<message>(ConceptSettingMessages);
    }


}
/// <summary>
/// json Defin Message Setting Data Class
/// 
/// defindMessage -> Role , Message
/// 
/// RefineMessageData -> (defindMessage chainge List<message>) 
/// </summary>
[Serializable]
public class DefinedmessageClass
{
    [JsonProperty("message")]
    public List<DefinJsonMessage> defindMessages = new List<DefinJsonMessage>();

    public void RefineMessageData(ref List<message> messages)
    {
        try
        {
            foreach (var data in defindMessages)
            {
                messages.Add(new message { Role = (message.role)data.Role, Message = data.Message });
            }
        }
        catch (NullReferenceException e)
        {
            throw new Exception("Null Data" + e);
        }
    }
}
[Serializable]
public class DefinJsonMessage
{
    [JsonProperty("Role")]
    public int Role;
    [JsonProperty("Mesage")]
    public string Message;
}

/// <summary>
/// chat gpt json 기본구성
/// 
/// role 3가지 설정과 텍스트로 구성
/// </summary>
[Serializable]
public class message
{
    public enum role
    {
        [EnumMember(Value = "system")]
        system,
        [EnumMember(Value = "user")]
        user,
        [EnumMember(Value = "assistant")]
        assistant
    }

    [JsonProperty("role"), JsonConverter(typeof(StringEnumConverter)), XmlAttribute("role")]
    public role Role;
    [JsonProperty("content"), XmlAttribute("content")]
    public string Message = "";


}

#region jsonData class
/// <summary>
/// response 를 받았을때 데이터 구성 deserialize 를 위해 필요함
/// </summary>
public class CompletionResult_OAI
{


    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("object")]
    public string Object { get; set; }

    [JsonProperty("created")]
    public int Created { get; set; }

    [JsonProperty("model")]
    public string Model { get; set; }

    [JsonProperty("choices")]
    public List<dataNumber> Message;

}

[System.Serializable]
public class dataNumber
{

    [JsonProperty("message")]
    public datamessage datamessage { get; set; }

}
public class datamessage
{

    [JsonProperty("role")]
    public string Role { get; set; }

    [JsonProperty("content")]
    public string Content { get; set; }
}

#endregion


/// <summary>
/// chat gpt의 코드리뷰후 gpt가 추출한 코드
/// </summary>
class OpenAIChatApiClient
{

    private static readonly HttpClient client = new HttpClient();
    private const string ApiUrl = "https://api.openai.com/v1/chat/completions";
    private const string AuthorizationHeader = "Bearer";
    private const string UserAgentHeader = "User-Agent";
    private readonly string apiKey = "sk-키";

    public OpenAIChatApiClient(string _apiKey)
    {
        this.apiKey = _apiKey;
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(AuthorizationHeader, _apiKey);
        client.DefaultRequestHeaders.Add(UserAgentHeader, "okgodoit/dotnet_openai_api");
    }


    public async Task<CompletionResult_OAI> GetCompletionResultAsync((List<message>, string model) _request)
    {
        var jsonContent = JsonConvert.SerializeObject(_request, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        try
        {
            using var response = await client.PostAsync(ApiUrl, content);
            if (response.IsSuccessStatusCode)
            {
                var resultAsString = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<CompletionResult_OAI>(resultAsString);
                return result;
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Error calling OpenAI API to get completion. HTTP status code: {response.StatusCode}. Request body: {jsonContent}. Error content: {errorContent}");
            }
        }
        catch (HttpRequestException ex)
        {
            throw new Exception("Failed to get completion result.", ex);
        }
    }
}