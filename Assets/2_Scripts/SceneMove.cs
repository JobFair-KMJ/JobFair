using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMove : MonoBehaviour
{
    public float time;
    IEnumerator Load(float time)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene("StartGame");
    }

    private void Awake()
    {
        StartCoroutine(Load(time));
    }
}
