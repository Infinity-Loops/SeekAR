using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginAuth : MonoBehaviour
{
    public GameObject loading;
    public void Load()
    {
        DataSystem.gameData.isBetaAuthScreenPassed = true;
        DataSystem.SaveData();
        SceneManager.LoadSceneAsync("GameScene");
    }
}
