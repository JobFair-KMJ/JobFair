using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypeEffect : MonoBehaviour
{
    public GameObject EndCursor;

    // 대화 문자열 저장용 변수
    string targetMsg;
    // 재생속도
    public int CharPerSeconds;

    public Text msgText;
    int msgIndex = 0;
    float interval;
    public bool isAnim;

    public bool isAct;
    private void Awake()
    {
        //msgText = GetComponent<Text>();
    }

    public void SetMsg(string msg)
    {
        if (msg.Contains("{name}"))
        {
            msg = msg.Replace("{name}", PlayerStatus.name);
            Debug.Log(msg);
        }

        if (isAnim)
        {
            msgText.text = targetMsg;
            CancelInvoke();
            EffectEnd();
        }
        else
        {
            targetMsg = msg;
            
            EffectStart();
        }
    }

    private void EffectStart()
    {
        msgText.text = "";
        msgIndex = 0;
        EndCursor.SetActive(false);

        interval = 1.0f / CharPerSeconds;

        isAnim = true;
        isAct = false;

        // Invoke로 시간차 반복 재생
        Invoke("Effecting", interval);  // 1글자가 나오는 딜레이
    }

    private void Effecting()
    {
        if(msgText.text == targetMsg)
        {
            EffectEnd();
            return;
        }

        msgText.text += targetMsg[msgIndex];
        msgIndex++;

        Invoke("Effecting", interval);
    }

    private void EffectEnd()
    {
        isAnim = false;
        isAct = true;
        EndCursor.SetActive(true);
    }
}