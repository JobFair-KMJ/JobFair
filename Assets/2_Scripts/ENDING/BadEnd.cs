using UnityEngine.SceneManagement;

public class BadEnd : ChatManager
{
    protected override void End(int k)
    {
        // ȣ���� �ֱ�..
        SceneManager.LoadScene("BAD");
    }
}