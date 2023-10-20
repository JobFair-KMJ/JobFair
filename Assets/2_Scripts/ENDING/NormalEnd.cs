using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NormalEnd : ChatManager
{
    protected override void End(int k)
    {
        // 호감도 넣기..
        SceneManager.LoadScene("NORMAL");
    }
}