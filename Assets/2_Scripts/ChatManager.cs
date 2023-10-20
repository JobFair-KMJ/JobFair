using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChatManager : MonoBehaviour
{
    #region Variable
    // 데이터!
    public TextAsset data;
    protected AllData datas;
    //김민준, 강예원, 한예설
    public List<string> cName = new List<string>() { "김민준", "강예원", "한예설" };

    // UI!
    public Text TextName;
    public TypeEffect TextSentence;
    public Image Portrait;

    // 인물
    public List<Sprite> Profiles1;
    public List<Sprite> Profiles2;
    public List<Sprite> Profiles3;
    public List<Sprite> Profiles4;

    public Image InMoL1;
    public Image InMoL2;
    public Image InMoL3;
    public Image InMoL4;

    public Image BG;
    public List<Sprite> BGS;
    // 입력!
    public KeyCode NextInput;

    // 인덱스 관리!
    protected int NextIndex = 0;

    // 애니메이션
    public Animator CharacterPanel;
    public Animator BranchPanel1;
    public Animator BranchPanel2;

    protected bool onBranch1;
    protected bool onBranch2;
    protected bool checkBranch;

    public Text tx1;
    public Text tx2;

    // 배경 리스트
    protected string[] backgroundlist = new string[] { "(HIGHLIGHT)CHESTAMAZING", "CAFFE", "CLASS", "DAYSCHOOL", "HEERAHOME", "MANHOME", "NIGHTCLASS", "NIGHTSTREET", "PARK", "RESTAURANT", "UNIVERSITY", "WOMANHOME" };

    Dictionary<string, Dictionary<string, Sprite>> profiles;
    protected float timer = 0;
    protected int mode = 0;

    #endregion

    #region Utility
    private IEnumerator setWaitT()
    {
        print("setWaitT");
        CharacterPanel.SetBool("isShow", true);
        yield return new WaitForSeconds(0.7f);

        BranchPanel1.SetBool("isBranch", true);
        BranchPanel2.SetBool("isBranch", true);
    }

    private IEnumerator setWaitF()
    {
        print("setWaitF");
        yield return new WaitForSeconds(0.7f);
        BranchPanel1.SetBool("isBranch", false);
        BranchPanel2.SetBool("isBranch", false);
        yield return new WaitForSeconds(0.5f);

        CharacterPanel.SetBool("isShow", false);
    }

    
    protected IEnumerator Timer()
    {
        while (true)
        {
            //print(timer);
            timer += Time.deltaTime;
            yield return null;
        }
    }

    protected void Setprofiles()
    {
        profiles = new Dictionary<string, Dictionary<string, Sprite>>
        {
            {
                "윤가온1", new Dictionary<string, Sprite>
                {
                    { "IDLE", Profiles1[0] },
                    { "SCARY", Profiles1[1] },
                    { "SHY", Profiles1[2] },
                    { "SHYTALK", Profiles1[3] },
                    { "STRAIGHTFACE", Profiles1[4] },
                    { "TALK", Profiles1[5] }
                }
            },
            {
                "고희라", new Dictionary<string, Sprite>
                {
                    { "ANGRYTALK", Profiles2[0] },
                    { "HAPPYTALK", Profiles2[1] },
                    { "IDLE", Profiles2[2] },
                    { "MEGAANGRY", Profiles2[3] },
                    { "MEGAHAPPY", Profiles2[4] },
                    { "MEGAHAPPYTALK", Profiles2[5] },
                }
            },
            {
                "윤가온2", new Dictionary<string, Sprite>
                {
                    { "ANGRY", Profiles3[0] },
                    { "ANGRYTALK", Profiles3[1] },
                    { "HAPPYTALK", Profiles3[2] },
                    { "IDLE", Profiles3[3] },
                    { "SADTALK", Profiles3[4] },
                    { "SCARY", Profiles3[5] },
                    { "SHYTALK", Profiles3[6] }
                }
            }
        };
    }

    protected void Changebackground()
    {
        for (int i = 0; i < backgroundlist.Length; i++)
        {
            if (datas.story[NextIndex].SubInfo == backgroundlist[i].ToString())
            {
                BG.color = new Color(1, 1, 1, 1);
                BG.sprite = BGS[i];
                return;
            }
        }
        BG.color = new Color(0, 0, 0, 255 / 255f);
        return;
    }

    protected virtual void End(int k)
    {
        Debug.Log("Welcome to EEEEEEEEEENNNNNNNDDDDDD");
        // 호감도 넣기..
        
        if(k == 0)
        {
            if(datas.story[NextIndex + 1].SubContents == "HOME")
            {
                SceneManager.LoadScene("StartGame");
            }
            if (datas.story[NextIndex + 1].SubContents == "HAPPY1")
            {
                SceneManager.LoadScene("Happy1_EndScene");
            }
            if (datas.story[NextIndex + 1].SubContents == "HAPPY2")
            {
                SceneManager.LoadScene("Happy2_EndScene");
            }
            if (datas.story[NextIndex + 1].SubContents == "HAREM")
            {
                SceneManager.LoadScene("Harem_EndScene");
            }
            if (datas.story[NextIndex + 1].SubContents == "YANDERE")
            {
                SceneManager.LoadScene("Yandere_EndScene");
            }
        }
        else if(k == 1)
        {
            if (PlayerStatus.friendshiplevel[0] >= 60 && PlayerStatus.friendshiplevel[2] >= 60)
            {
                SceneManager.LoadScene("Harem");
            }
            else if (PlayerStatus.friendshiplevel[0] >= 60)
            {
                SceneManager.LoadScene("Happy1");
            }
            else if (PlayerStatus.friendshiplevel[0] >= 30)
            {
                SceneManager.LoadScene("Normal1");
            }
            else if (PlayerStatus.friendshiplevel[0] < 30)
            {
                SceneManager.LoadScene("Sad1");
            }
        }
        if (k == 2)
        {
            if (PlayerStatus.friendshiplevel[2] >= 60)
            {
                if(PlayerStatus.friendshiplevel[0] >= 60 && PlayerStatus.friendshiplevel[1] <= -30)
                {
                    SceneManager.LoadScene("Yandere");
                }
                SceneManager.LoadScene("Happy2");
            }
            else if (PlayerStatus.friendshiplevel[0] >= 60 && PlayerStatus.friendshiplevel[2] >= 60)
            {
                SceneManager.LoadScene("Harem");
            }
            else if (PlayerStatus.friendshiplevel[2] >= 30)
            {
                SceneManager.LoadScene("Normal2");
            }
            else if (PlayerStatus.friendshiplevel[2] < 30)
            {
                SceneManager.LoadScene("Bad2");
            }
        }

        
    }

    #endregion

    protected void Awake()
    {
        datas = JsonUtility.FromJson<AllData>(data.text);

        print(datas.story[NextIndex].Info);
        TextName.text = datas.story[NextIndex].Info;
        TextSentence.SetMsg(datas.story[NextIndex].Contents);
        Portrait.sprite = Profiles4[0];

        Setprofiles();
        StartCoroutine(Timer());
    }

    protected void Update()
    {
        Next();
    }

    protected void Next()
    {
        // 인물
        if (datas.story[NextIndex].Info == "나래이션")
        {
            InMoL1.color = Color.clear;
            InMoL2.color = Color.clear;
            InMoL3.color = Color.clear;
        }
        
        if (datas.story[NextIndex].Info == cName[0])
        {
            InMoL1.color = new Color(255 / 255f, 255 / 255f, 255 / 255f, 255 / 255f);
            InMoL2.color = new Color(166 / 255f, 166 / 255f, 166 / 255f, 255 / 255f);
            InMoL3.color = new Color(166 / 255f, 166 / 255f, 166 / 255f, 255 / 255f);
        }
        else if (datas.story[NextIndex].Info == cName[1])
        {
            InMoL1.color = new Color(166 / 255f, 166 / 255f, 166 / 255f, 255 / 255f);
            InMoL2.color = new Color(255 / 255f, 255 / 255f, 255 / 255f, 255 / 255f);
            InMoL3.color = new Color(166 / 255f, 166 / 255f, 166 / 255f, 255 / 255f);
        }
        else if (datas.story[NextIndex].Info == cName[2])
        {
            InMoL1.color = new Color(166 / 255f, 166 / 255f, 166 / 255f, 255 / 255f);
            InMoL2.color = new Color(166 / 255f, 166 / 255f, 166 / 255f, 255 / 255f);
            InMoL3.color = new Color(255 / 255f, 255 / 255f, 255 / 255f, 255 / 255f);
        }
        else
        {
            Portrait.sprite = Profiles4[0];
        }

        if (datas.story[NextIndex].SubInfo.Contains("(HIGHLIGHT)"))
        {
            InMoL1.color = new Color(255 / 255f, 255 / 255f, 255 / 255f, 0 / 255f);
            InMoL2.color = new Color(166 / 255f, 166 / 255f, 166 / 255f, 0 / 255f);
            InMoL3.color = new Color(166 / 255f, 166 / 255f, 166 / 255f, 0 / 255f);
        }

        // 배경
        Changebackground();

        if(datas.story[NextIndex].Code == "NARRATION")
        {
            InMoL1.sprite = Profiles1[0];
            InMoL2.sprite = Profiles2[2];
            InMoL3.sprite = Profiles3[3];
        }

       
        // 현재 데이터에 따라서 sprite 값을 설정합니다.
        string info = datas.story[NextIndex].Info;
        string subContents = datas.story[NextIndex].SubContents;

        if (profiles.ContainsKey(info) && profiles[info].ContainsKey(subContents))
        {
            if (info == "윤가온1")
            {
                InMoL1.sprite = profiles[info][subContents];
                Portrait.sprite = profiles[info][subContents];
            }
            else if (info == "고희라")
            {
                InMoL2.sprite = profiles[info][subContents];
                Portrait.sprite = profiles[info][subContents];
            }
            else if (info == "윤가온2")
            {
                InMoL3.sprite = profiles[info][subContents];
                Portrait.sprite = profiles[info][subContents];
            }
            
        }

        if (datas.story[NextIndex + 1].Code != "END" && (Input.GetKeyUp(NextInput) || checkBranch)) // 다음 스토리가 종료가 아니면서, 다음으로 가는 입력 또는 브랜치를 선택을 하였을때. 선택 모드 중에는 이하의 함수는 전부 무시된다.
        {
            while (datas.story[NextIndex + 1].Flag != mode)  // 분기점에 따라 mode가 변하며, 만약에 다른 분기점 대화창을 만난다면 전부 넘겨버린다
            {
                if(datas.story[NextIndex + 1].Flag == 0)
                {
                    mode = 0;
                    break;
                }
                print(NextIndex);
                NextIndex++;
            }
            print(mode);

            //print(NextIndex + " " + onBranch1 + " " + onBranch2);
            if (datas.story[NextIndex + 1].Code == "SELECT") // SELECT 는 분기점 선택 모드이다, 선택을 하여야 다음으로 넘어갈 수 있다.
            {
                if (TextSentence.isAnim)
                {
                    TextSentence.SetMsg("");
                    return;
                }
                tx1.text = datas.story[NextIndex + 1].Contents;
                tx2.text = datas.story[NextIndex + 2].Contents;
                StartCoroutine(setWaitT());

                if (onBranch1 || onBranch2)
                {
                    StartCoroutine(setWaitF());
                }

                checkBranch = false;

                if (onBranch1)
                {
                    while (datas.story[NextIndex].Flag != 1)
                    {
                        NextIndex++;
                    }
                    mode = 1;

                    onBranch1 = false;
                }
                else if (onBranch2)
                {
                    while (datas.story[NextIndex].Flag != 2)
                    {
                        NextIndex++;
                    }
                    mode = 2;

                    onBranch2 = false;
                }
                else
                    return;

                checkBranch = false;
            }
            else // 분기점 모드가 아니면 넘어간다.
            {
                NextIndex++;

                //print(NextIndex);
                //print("접근함.");

            }

            if (TextSentence.isAnim)
            {
                TextSentence.SetMsg("");
                NextIndex--;
                return;
            }
            else
            {
                if (NextIndex >= datas.story.Length)
                {
                    //print("끝");
                    return;
                }

                PrintText();
            }
        }
        else if (datas.story[NextIndex + 1].Code == "END")
        {
            End(0);
        }
    }

    protected void PrintText()
    {
        TextName.text = datas.story[NextIndex].Info;
        if (TextName.text == "{name}")
        {
            TextName.text = PlayerStatus.name;
        }
        TextSentence.SetMsg(datas.story[NextIndex].Contents);
    }

    public void OnBranch1()
    {
        if (datas.story[NextIndex + 1].Code == "SELECT")
        {
            print("접근");
            checkBranch = true;
            onBranch1 = true;
            if (datas.story[NextIndex + 1].SubInfo == "END")
            {
                End(1);
            }
        }
    }

    public void OnBranch2()
    {
        if (datas.story[NextIndex + 1].Code == "SELECT")
        {
            print("접근");
            checkBranch = true;
            onBranch2 = true;
            if (datas.story[NextIndex + 1].SubInfo == "END")
            {
                End(2);
            }
        }
    }
}

[Serializable]
public class AllData
{
    public ScenarioData[] story;
}

[Serializable]
public class ScenarioData
{
    public string Code;
    public int Flag;
    public string Info;
    public string SubInfo;
    public string SubContents;
    public string Contents;
}