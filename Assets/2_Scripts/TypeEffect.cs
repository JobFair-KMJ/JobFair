using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypeEffect : MonoBehaviour
{
    public GameObject EndCursor;

    // ��ȭ ���ڿ� ����� ����
    string targetMsg;
    // ����ӵ�
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

        // Invoke�� �ð��� �ݺ� ���
        Invoke("Effecting", interval);  // 1���ڰ� ������ ������
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