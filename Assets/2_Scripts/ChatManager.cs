using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChatManager : MonoBehaviour
{
    #region Variable
    // ������!
    public TextAsset data;
    protected AllData datas;
    //�����, ������, �ѿ���
    public List<string> cName = new List<string>() { "�����", "������", "�ѿ���" };

    // UI!
    public Text TextName;
    public TypeEffect TextSentence;
    public Image Portrait;

    // �ι�
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
    // �Է�!
    public KeyCode NextInput;

    // �ε��� ����!
    protected int NextIndex = 0;

    // �ִϸ��̼�
    public Animator CharacterPanel;
    public Animator BranchPanel1;
    public Animator BranchPanel2;

    protected bool onBranch1;
    protected bool onBranch2;
    protected bool checkBranch;

    public Text tx1;
    public Text tx2;

    // ��� ����Ʈ
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
                "������1", new Dictionary<string, Sprite>
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
                "�����", new Dictionary<string, Sprite>
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
                "������2", new Dictionary<string, Sprite>
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
        // ȣ���� �ֱ�..
        
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
        // �ι�
        if (datas.story[NextIndex].Info == "�����̼�")
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

        // ���
        Changebackground();

        if(datas.story[NextIndex].Code == "NARRATION")
        {
            InMoL1.sprite = Profiles1[0];
            InMoL2.sprite = Profiles2[2];
            InMoL3.sprite = Profiles3[3];
        }

       
        // ���� �����Ϳ� ���� sprite ���� �����մϴ�.
        string info = datas.story[NextIndex].Info;
        string subContents = datas.story[NextIndex].SubContents;

        if (profiles.ContainsKey(info) && profiles[info].ContainsKey(subContents))
        {
            if (info == "������1")
            {
                InMoL1.sprite = profiles[info][subContents];
                Portrait.sprite = profiles[info][subContents];
            }
            else if (info == "�����")
            {
                InMoL2.sprite = profiles[info][subContents];
                Portrait.sprite = profiles[info][subContents];
            }
            else if (info == "������2")
            {
                InMoL3.sprite = profiles[info][subContents];
                Portrait.sprite = profiles[info][subContents];
            }
            
        }

        if (datas.story[NextIndex + 1].Code != "END" && (Input.GetKeyUp(NextInput) || checkBranch)) // ���� ���丮�� ���ᰡ �ƴϸ鼭, �������� ���� �Է� �Ǵ� �귣ġ�� ������ �Ͽ�����. ���� ��� �߿��� ������ �Լ��� ���� ���õȴ�.
        {
            while (datas.story[NextIndex + 1].Flag != mode)  // �б����� ���� mode�� ���ϸ�, ���࿡ �ٸ� �б��� ��ȭâ�� �����ٸ� ���� �Ѱܹ�����
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
            if (datas.story[NextIndex + 1].Code == "SELECT") // SELECT �� �б��� ���� ����̴�, ������ �Ͽ��� �������� �Ѿ �� �ִ�.
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
            else // �б��� ��尡 �ƴϸ� �Ѿ��.
            {
                NextIndex++;

                //print(NextIndex);
                //print("������.");

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
                    //print("��");
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
            print("����");
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
            print("����");
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