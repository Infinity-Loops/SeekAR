using DG.Tweening;
using Niantic.Lightship.Maps.Core.Coordinates;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Jester : MonoBehaviour, IPointerDownHandler
{
    public GameObject confetti;
    public Transform table;
    public CoinDB gameCoins;
    public GameObject coinPrefab;
    public Animator animator;
    public Transform startCanvas;
    public Transform takeRiskCanvas;
    public Transform jesterMouth;
    public Transform coinJesterMouthEatPos1;
    public Transform coinJesterMouthEatPos2;
    public ParticleSystem startParticle;
    public SkinnedMeshRenderer skinnedMesh;
    public AudioClip startSpeech;
    public AudioClip takeRiskSpeech;
    public AudioClip apeInSpeech;
    public AudioClip wonLaugh;
    public AudioClip lostLaugh;
    public AudioClip swoosh;
    public AudioClip fail;
    public GameObject lostGraphic;
    public List<Transform> coinStacks = new List<Transform>();
    private List<GameCoin> betCoins = new List<GameCoin>();
    private List<GameObject> stackCoinInstances = new List<GameObject>();
    private bool jesterActivated;
    private bool isCloseEnough;

    private float eyeClosedBlendWeight;
    private float blinkDuration = 0.333f;
    private float transitionStartTime;
    private bool isBlinking = false;
    private GameObject instantiatedMapObject;
  

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!jesterActivated)
        {
            jesterActivated = true;
            AudioUtils.PlayClipAtPoint(swoosh, transform.position, 1f, 0f, 1f);
            transform.DORotate(new Vector3(0, ARCamera.arCamera.transform.eulerAngles.y - 180, 0), 1f, RotateMode.FastBeyond360).OnComplete(() =>
            {
                Instantiate(confetti, transform.position, Quaternion.identity);
                InitJester();
            });
        }
    }

    void InitJester()
    {
        table.DOScale(Vector3.one, 0.25f).OnComplete(() =>
        {
            SelectBetCoins();
            startCanvas.transform.localScale = Vector3.zero;
            startCanvas.gameObject.SetActive(true);
            startCanvas.transform.DOScale(0.003f, 0.25f).OnComplete(() =>
            {
                AudioUtils.PlayClipAtPoint(startSpeech, transform.position, 1f, 0f, 1f);
                skinnedMesh.DoShapeKey(5, 100, 0.5f).OnComplete(() =>
                {
                    skinnedMesh.DoShapeKey(5, 0, 0.5f).SetDelay(0.1f);
                });
            });
        });
    }

    public void TryOut()
    {
        startCanvas.transform.DOScale(Vector3.zero, 0.25f).OnComplete(() =>
        {
            startCanvas.gameObject.SetActive(false);
            takeRiskCanvas.transform.localScale = Vector3.zero;
            takeRiskCanvas.gameObject.SetActive(true);
            takeRiskCanvas.transform.DOScale(0.003f, 0.25f).OnComplete(() =>
            {
                AudioUtils.PlayClipAtPoint(takeRiskSpeech, transform.position, 1f, 0f, 1f);
                skinnedMesh.DoShapeKey(5, 100, 0.5f).OnComplete(() =>
                {
                    skinnedMesh.DoShapeKey(5, 0, 0.5f).SetDelay(0.1f);
                });
            });
        });
    }

    public void TakeRisk()
    {
        takeRiskCanvas.transform.DOScale(Vector3.zero, 0.25f).OnComplete(() =>
        {
            AudioUtils.PlayClipAtPoint(apeInSpeech, transform.position, 1f, 0f, 1f);
            skinnedMesh.DoShapeKey(5, 100, 0.5f).OnComplete(() =>
            {
                skinnedMesh.DoShapeKey(5, 0, 0.5f).SetDelay(0.1f);

            });

            animator.CrossFade("Animation", 0.25f);

            startParticle.Play();

            StartCoroutine(StartJesterLogic());
        });
    }

    IEnumerator StartJesterLogic()
    {
        yield return new WaitForSeconds(3f);
        int wonOrLost = Random.Range(0, 2);
        switch (wonOrLost)
        {
            case 0:
                StartCoroutine(HandleLost());
                break;
            case 1:
                StartCoroutine(HandleWon());
                break;
        }

        //StartCoroutine(HandleWon());
    }

    IEnumerator HandleLost()
    {
        yield return new WaitForSeconds(1.5f);
        animator.CrossFade("Animation", 0.25f);
        AudioUtils.PlayClipAtPoint(lostLaugh, transform.position, 1f, 0f, 1f);
        yield return new WaitForSeconds(3f);
        animator.CrossFade("OpenMouth", 0.25f);
        foreach (var coin in stackCoinInstances)
        {
            coin.transform.DOMove(coinJesterMouthEatPos1.position, 0.5f).OnComplete(() =>
            {
                coin.transform.DOMove(coinJesterMouthEatPos2.position, 0.5f);
            });
        }
        yield return new WaitForSeconds(1f);
        animator.CrossFade("CloseMouth", 0.25f);
        yield return new WaitForSeconds(1f);
        lostGraphic.SetActive(true);
        var graphicAnimator = lostGraphic.GetComponent<Animator>();
        graphicAnimator.CrossFade("Pop", 0.25f);
        yield return new WaitForSeconds(2f);
        AudioUtils.PlayClipAtPoint(fail, transform.position, 1f, 0f, 1f);
        yield return new WaitForSeconds(4.5f);
        AudioUtils.PlayClipAtPoint(swoosh, transform.position, 1f, 0f, 1f);
        transform.DOScale(Vector3.zero, 1.5f);
        transform.DORotate(new Vector3(0, 360, 0), 0.5f, RotateMode.FastBeyond360);
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    IEnumerator HandleWon()
    {
        yield return new WaitForSeconds(1.5f);
        animator.CrossFade("Animation", 0.25f);
        AudioUtils.PlayClipAtPoint(wonLaugh, transform.position, 1f, 0f, 1f);
        skinnedMesh.DoShapeKey(5, 100, 0.5f).OnComplete(() =>
        {
            skinnedMesh.DoShapeKey(5, 0, 0.5f).SetDelay(0.1f);
        });
        yield return new WaitForSeconds(0.25f);
        float[] positions = new float[] { -0.25f, 0f, 0.25f };
        var randomCoin = betCoins[Random.Range(0, betCoins.Count)];
        for (int i = 0; i < 3; i++)
        {
            var coinInstance = Instantiate(coinPrefab, coinJesterMouthEatPos2.position, Quaternion.identity);
            var selector = coinInstance.GetComponent<CollectableCoinSelector>();
            selector.dontChooseCoin = true;
            selector.coinToChoose = randomCoin.name;
            coinInstance.transform.localScale = Vector3.one * 0.2f;
            var circle = Random.insideUnitCircle * 2f;
            coinInstance.transform.DOMove(transform.position + new Vector3(positions[i], 0, 0) + transform.forward * 0.5f, 1f).OnComplete(() =>
            {
                //coinInstance.transform.DOScale(Vector3.one, 0.25f);
            });
        }

        yield return new WaitForSeconds(8f);
        AudioUtils.PlayClipAtPoint(swoosh, transform.position, 1f, 0f, 1f);
        transform.DOScale(Vector3.zero, 1.5f);
        transform.DORotate(new Vector3(0, 360, 0), 0.5f, RotateMode.FastBeyond360);
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    public void CancelRisk()
    {
        takeRiskCanvas.transform.DOScale(Vector3.zero, 0.25f).OnComplete(() =>
        {
            takeRiskCanvas.gameObject.SetActive(false);
            startCanvas.gameObject.SetActive(true);
            startCanvas.transform.DOScale(0.003f, 0.25f);
        });
    }

    void HandleRotation()
    {
        if (jesterActivated)
        {
            return;
        }

        float distance = Vector3.Distance(ARCamera.arCamera.transform.position, transform.position);
        if (distance <= 5)
        {
            if (!isCloseEnough)
            {
                isCloseEnough = true;
                transform.DORotate(new Vector3(0, ARCamera.arCamera.transform.eulerAngles.y - 180, 0), 0.25f);
            }
        }
        else
        {
            isCloseEnough = false;
            transform.Rotate(Vector3.up * 100 * Time.deltaTime);
        }
    }

    void SelectBetCoins()
    {
        Debug.Log("Selecting Coins");
        for (int i = 0; i < coinStacks.Count; i++)
        {
            var randomCoin = gameCoins.coins[Random.Range(0, gameCoins.coins.Count)];
            //while (betCoins.Exists(x => x == randomCoin))
            //{
            //    randomCoin = gameCoins.coins[Random.Range(0, gameCoins.coins.Count)];
            //}

            foreach (Transform stackCoin in coinStacks[i])
            {
                var model = Instantiate(randomCoin.model, stackCoin);
                model.transform.localScale = Vector3.zero;
                model.transform.DOScale(Vector3.one, 0.25f);
                stackCoinInstances.Add(model);
            }

            betCoins.Add(randomCoin);
        }
    }

    void HandleInitialState()
    {
        table.localScale = Vector3.zero;
    }

    private void Awake()
    {
        HandleInitialState();
    }

    private void Update()
    {
        HandleRotation();
        HandleBlink();
    }

    private void OnDestroy()
    {
        if (instantiatedMapObject != null)
        {
            Destroy(instantiatedMapObject);
        }
    }

    void HandleBlink()
    {
        if (!isBlinking)
        {
            isBlinking = true;
            transitionStartTime = Time.time;
            StartCoroutine(Blink());
        }
    }

    IEnumerator Blink()
    {
        float elapsedTime = 0f;

        while (elapsedTime < blinkDuration)
        {
            float progress = elapsedTime / blinkDuration;

            eyeClosedBlendWeight = Mathf.Lerp(0f, 100f, progress);

            skinnedMesh.SetBlendShapeWeight(4, eyeClosedBlendWeight);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        skinnedMesh.SetBlendShapeWeight(4, 0f);

        yield return new WaitForSeconds(Random.Range(1f, 4f));

        isBlinking = false;
    }
}