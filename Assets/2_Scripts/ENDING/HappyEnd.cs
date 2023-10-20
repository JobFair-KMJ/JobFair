using UnityEngine.SceneManagement;

public class HappyEnd : ChatManager
{
    protected override void End(int k)
    {
        // 호감도 넣기..
        SceneManager.LoadScene("HAPPY");
    }
}