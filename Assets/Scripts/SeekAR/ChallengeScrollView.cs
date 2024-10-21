using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.ProceduralImage;

public class ChallengeScrollView : MonoBehaviour
{
    public List<Transform> objectives = new List<Transform>();
    public List<ProceduralImage> lines = new List<ProceduralImage>();
    public GameObject detailText;

    private void OnEnable()
    {
        StartCoroutine(Animate());
    }

    IEnumerator Animate()
    {
        for (int i = 0; i < objectives.Count; i++)
        {
            var objective = objectives[i];
            objective.transform.localScale = Vector3.zero;
        }

        for (int i = 0; i < lines.Count; i++)
        {
            var line = lines[i];
            line.fillAmount = 0;
        }

        for (int i = 0; i < objectives.Count; i++)
        {
            var objective = objectives[i];
            objective.transform.DOScale(Vector3.one, 0.25f);

            yield return new WaitForSeconds(0.3f);
        }


        for (int i = 0; i < lines.Count; i++)
        {
            var line = lines[i];
            line.DOFillAmount(1, 0.4f);
            yield return new WaitForSeconds(0.55f);
        }

        detailText.transform.DORotate(new Vector3(0, 0, -15f), 0.15f).OnComplete(() =>
        {
            detailText.transform.DORotate(new Vector3(0, 0, 15f), 0.15f).OnComplete(() =>
            {
                detailText.transform.DORotate(new Vector3(0, 0, 0), 0.15f);
            });
        });
    }
}
