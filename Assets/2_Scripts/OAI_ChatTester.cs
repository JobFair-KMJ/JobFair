using UnityEngine;
using UnityEngine.UI;

public class OAI_ChatTester : MonoBehaviour
{
    private string postText;
    string ReqestText = "";
    public OAI_Chat[] OAI_Chat;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < OAI_Chat.Length; i++)
        {
            OAI_Chat[i].CompletedRepostEvent = delegate (string _string) { ReqestText = _string; };
        }
    }

    public void Reqest(Text text)
    {
        ReqestText = text.text;
        Test();
    }

    private async void Test()
    {
        //OAI_Chat.ReqestStringData(b);
        //Debug.Log(b);
        for (int i = 0; i < OAI_Chat.Length; i++)
        {
            postText = (await OAI_Chat[i].AsyncReqesStringtData(ReqestText, _sendMessageDebugLog: true));
            string addlike = postText.Substring(0, 1);
            Debug.Log(i + ": " + addlike);

            if (addlike == "+")
            {
                PlayerStatus.friendshiplevel[i] += 10;
            }
            if (addlike == "-")
            {
                PlayerStatus.friendshiplevel[i] -= 10;
            }
        }

        
    }
}