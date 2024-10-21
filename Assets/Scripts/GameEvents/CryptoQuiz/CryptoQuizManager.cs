using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CryptoQuizManager : MonoBehaviour
{
    [Header("Start Page")]
    public TMP_Text startTitle;
    public Image startIcon;
    public GameObject startPage;
    [Header("Quiz Page")]
    public GameObject quizPage;
    public TMP_Text quizQuestion;
    public AlternativeButtonQuiz templateQuizButton;
    public Transform quizButtonContent;
    public CryptoQuizDB quizDB;
    public NftDB nfts;
    public GameObject loading;
    [Header("Finish Pages")]
    public GameObject wonPage;
    public GameObject wonModel;
    public GameObject lostPage;
    public GameObject lostModel;
    private Canvas canvas;
    private CryptoQuizType currentQuiz;
    private int currentQuizPageIndex;
    private int maxQuizPageIndex;
    private List<AlternativeButtonQuiz> alternatives = new List<AlternativeButtonQuiz>();
    private int quizSuccesses;
    private bool isCloseEnough;
    private GameObject lostModelInstance;
    private GameObject wonModelInstance;
    public void HandleStartQuiz()
    {
        startPage.SetActive(false);
        quizPage.SetActive(true);
        HandleQuizLogic();
    }

    public void HandleSelectRandomQuiz()
    {
        currentQuiz = quizDB.quiz[Random.Range(0, quizDB.quiz.Count)];
    }

    private void Awake()
    {
        canvas = GetComponent<Canvas>();

        HandleSelectRandomQuiz();

        startIcon.sprite = currentQuiz.coinIcon;
        startTitle.text = $"{currentQuiz.coinName} Quiz";
    }

    private void HandleQuizLogic()
    {
        maxQuizPageIndex = currentQuiz.pages.Count;
        currentQuizPageIndex = -1;
        HandlePrepareQuiz();
    }

    private void HandleEndQuiz()
    {
        var pagesCount = currentQuiz.pages.Count;
        if (quizSuccesses >= pagesCount)
        {
            StartCoroutine(HandleWonSequence());
        }
        else
        {
            StartCoroutine(HandleLostSequence());

        }
    }

    IEnumerator HandleWonSequence()
    {
        canvas.enabled = false;
        Vector3 location = ARCamera.arCamera.transform.position + ARCamera.arCamera.transform.forward * 2f;
        Quaternion rotation = ARCamera.arCamera.transform.rotation * Quaternion.Euler(0, -180, 0);
        wonModelInstance = Instantiate(wonModel, location, rotation);
        yield return new WaitForSeconds(3.5f);
        canvas.enabled = true;
        this.quizPage.gameObject.SetActive(false);
        wonPage.SetActive(true);
    }

    IEnumerator HandleLostSequence()
    {
        canvas.enabled = false;
        Vector3 location = ARCamera.arCamera.transform.position + ARCamera.arCamera.transform.forward * 2f;
        Quaternion rotation = ARCamera.arCamera.transform.rotation * Quaternion.Euler(0, -180, 0);
        lostModelInstance = Instantiate(lostModel, location, rotation);
        yield return new WaitForSeconds(3.5f);
        canvas.enabled = true;
        this.quizPage.gameObject.SetActive(false);
        lostPage.SetActive(true);
    }

    public void HandlePrepareQuiz()
    {

        currentQuizPageIndex++;

        if (currentQuizPageIndex >= maxQuizPageIndex)
        {
            currentQuizPageIndex--;
            HandleEndQuiz();
            return;
        }

        if (alternatives.Count > 0)
        {
            foreach (AlternativeButtonQuiz alternative in alternatives)
            {
                Destroy(alternative.gameObject);
            }

            alternatives.Clear();
        }

        var quizPage = currentQuiz.pages[currentQuizPageIndex];

        quizQuestion.text = quizPage.question;

        for (int i = 0; i < quizPage.alternatives.Count; i++)
        {
            var quizAlternative = quizPage.alternatives[i];
            AlternativeButtonQuiz alternative = Instantiate(templateQuizButton, quizButtonContent);
            alternative.alternativeText.text = quizAlternative;
            int currentIndex = i;
            alternative.alternativeButton.onClick.AddListener(() => { HandleAnswer(currentIndex); });
            alternative.gameObject.SetActive(true);
            alternatives.Add(alternative);
        }
    }

    void HandleAnswer(int index)
    {
        var quizPage = currentQuiz.pages[currentQuizPageIndex];
        Debug.Log(index);
        if (quizPage.correctAlternative == index)
        {
            alternatives[index].alternativeButton.targetGraphic.color = Color.green;
            quizSuccesses++;
        }
        else
        {
            alternatives[index].alternativeButton.targetGraphic.color = Color.red;
        }

        WaitForNextAnswer();
    }

    void WaitForNextAnswer()
    {
        StartCoroutine(HandleNextAnswer());
    }

    IEnumerator HandleNextAnswer()
    {
        loading.SetActive(true);
        yield return new WaitForSeconds(3f);
        HandlePrepareQuiz();
        loading.SetActive(false);
    }

    public void GetReward()
    {
        transform.DOScale(0, 0.25f).OnComplete(() =>
        {
            Nft randomNFT = nfts.NftList[Random.Range(0, nfts.NftList.Count)];
            Quaternion rotation = ARCamera.arCamera.transform.rotation * Quaternion.Euler(0, -180, 0);
            Instantiate(randomNFT.collectablePrefab, transform.position, rotation);

            Destroy(gameObject);
        });
    }

    public void Close()
    {
        transform.DOScale(0, 0.25f).OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }

    void HandleRotation()
    {
        float distance = Vector3.Distance(ARCamera.arCamera.transform.position, transform.position);
        if (distance <= 3)
        {
            if (!isCloseEnough)
            {
                isCloseEnough = true;
            }
        }
        else
        {
            isCloseEnough = false;
            transform.rotation = Quaternion.Euler(0, ARCamera.arCamera.transform.eulerAngles.y, 0);
        }
    }

    private void Update()
    {
        HandleRotation();
    }
}
