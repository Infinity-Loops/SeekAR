using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro2 : MonoBehaviour
{
    public List<Transform> objects = new List<Transform>();
    public float moveSpeed;
    public Transform rotateCenter;
    public RectTransform logo;
    public WelcomeScreen welcomeScreen;
    public CanvasGroup canvas;
    private bool stop;
    IEnumerator Start()
    {
        foreach (Transform t in objects)
        {
            var cg = t.GetComponent<CanvasGroup>();
            cg.DOFade(1f, 0.25f);
            yield return new WaitForSeconds(0.25f);
        }

        yield return new WaitForSeconds(1f);
        stop = true;

        rotateCenter.DORotate(Vector3.zero, 1.25f);

        foreach (var item in objects)
        {
            item.DOLocalRotate(Vector3.zero, 0.15f);
        }

        foreach (var item in objects)
        {
            yield return new WaitForSeconds(0.25f);
            (item as RectTransform).DOAnchorPos(logo.anchoredPosition, 0.5f);
            item.DOScale(Vector3.zero, 0.5f);
            yield return new WaitForSeconds(0.1f);
        }

        canvas.DOFade(0f, 0.5f);
        yield return new WaitForSeconds(0.5f);

       StartCoroutine(welcomeScreen.InitialSequence());
    }
    private void Update()
    {
        if (stop)
        {
            return;
        }

        foreach (var item in objects)
        {
            item.rotation = Quaternion.identity;
        }

        rotateCenter.rotation *= (Quaternion.Euler(0, 0, moveSpeed * Time.deltaTime));
    }
}
