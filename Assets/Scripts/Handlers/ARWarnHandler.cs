using DG.Tweening;
using UnityEngine;

public class ARWarnHandler : MonoBehaviour
{

    private void Start()
    {
        if (DataSystem.gameData.dontRememberARWarn)
        {
            gameObject.SetActive(false);
        }
    }

    public void Remember()
    {
        DataSystem.gameData.dontRememberARWarn = false;
        DataSystem.SaveData();

        transform.DOScale(Vector3.zero, 0.25f).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }

    public void DontRemember()
    {
        transform.DOScale(Vector3.zero, 0.25f).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });

        DataSystem.gameData.dontRememberARWarn = true;
        DataSystem.SaveData();
    }
}
