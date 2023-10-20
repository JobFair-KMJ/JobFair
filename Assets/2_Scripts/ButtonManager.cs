using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public void StartGame()
    {
        InputField inputfield = GameObject.Find("InputField").GetComponent<InputField>();

        if(inputfield.text.Length != 0)
        {
            PlayerStatus.name = inputfield.text;
            for (int i = 0; i < PlayerStatus.friendshiplevel.Length; i++)
            {
                PlayerStatus.friendshiplevel[i] = 20;
            }

            SceneManager.LoadScene("PlayingGame");
        }
        
    }

    public void StartInputName()
    {
        SceneManager.LoadScene("SetName");
    }
    public void Movescene(string scenename)
    {
        SceneManager.LoadScene(scenename);
    }

    public void ShowStart() { }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
    }
}
