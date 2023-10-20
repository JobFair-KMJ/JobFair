using UnityEngine.SceneManagement;

public class BadEnd : ChatManager
{
    protected override void End(int k)
    {
        // 호감도 넣기..
        SceneManager.LoadScene("BAD");
    }
}