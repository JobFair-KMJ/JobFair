using UnityEngine.SceneManagement;

public class HappyEnd : ChatManager
{
    protected override void End(int k)
    {
        // ȣ���� �ֱ�..
        SceneManager.LoadScene("HAPPY");
    }
}