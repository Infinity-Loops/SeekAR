using AssetKits.ParticleImage;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TokensPanelController : MonoBehaviour
{
    public TokenPanelTemplate tokenTemplate;
    public CoinDB tokens;
    public Transform content;
    public List<float> positions;
    public ScrollRect scrollView;
    private int currentView;
    private int prevCurrentView;
    private List<TokenPanelInfo> panels = new List<TokenPanelInfo>();
    public CoinShower shower;
    public BackgroundChanger backgroundChanger;
    public int tokenCountsShower = 10;
    private bool isSame;
    [Header("Particles")]
    public ParticleSystem debris;
    public ParticleSystem glow;
    public ParticleSystem sparks;
    public ParticleSystem highlights;
    void HandleTokens()
    {
        var info = tokenTemplate.template.AddComponent<TokenPanelInfo>();
        info.template = tokenTemplate.template;
        info.title = tokenTemplate.title;
        info.xp = tokenTemplate.xp;
        info.location = tokenTemplate.location;
        info.icon = tokenTemplate.icon;
        info.money = tokenTemplate.money;

        foreach (var token in tokens.coins)
        {
            tokenTemplate.title.text = token.name;

            tokenTemplate.xp.text = $"{DataSystem.gameData.playerData.playerXP} XP";
            tokenTemplate.location.text = $"{1} NEARBY";
            tokenTemplate.icon.sprite = token.menuIcon;
            var balance = DataSystem.gameData.playerData.playerBalance.Find(x => x.currencyID == token.name);
            if (balance != null)
            {
                tokenTemplate.money.text = balance.currencyBalance.ToString("C2", CultureInfo.CreateSpecificCulture("en-US"));
            }
            else
            {
                tokenTemplate.money.text = 0.ToString("C2", CultureInfo.CreateSpecificCulture("en-US"));
            }

            var instance = Instantiate(tokenTemplate.template, content);
            panels.Add(instance.GetComponent<TokenPanelInfo>());
            instance.gameObject.SetActive(true);
        }

        positions = new List<float>();

        for (int i = 0; i < tokens.coins.Count; i++)
        {
            positions.Add((float)i / (float)tokens.coins.Count);
        }
    }

    void HandleGetCurrentTokenView()
    {
        int previousView = currentView;
        var position = (scrollView.normalizedPosition.x);

        for (int i = 0; i < positions.Count; i++)
        {
            if (i != positions.Count - 1)
            {
                if (positions[i] <= position && position < positions[Mathf.Clamp(i + 1, 0, positions.Count - 1)])
                {
                    currentView = i;
                }
            }
            else
            {
                if (position >= positions[i])
                {
                    currentView = i;
                }
            }
        }

        if (previousView != currentView)
        {
            prevCurrentView = previousView;
            OnViewChanged();
            isSame = false;
        }
        else
        {
            if (!isSame)
            {
                isSame = true;
                (panels[currentView].icon.rectTransform.parent as RectTransform).DOSizeDelta(Vector2.one * 1280, 0.25f);
                backgroundChanger.ProjectColors(tokens.coins[currentView].menuColor2, tokens.coins[currentView].menuColor1);
            }

        }
    }

    void OnViewChanged()
    {
        (panels[prevCurrentView].icon.rectTransform.parent as RectTransform).DOSizeDelta(Vector2.one * 640, 0.25f);
        (panels[currentView].icon.rectTransform.parent as RectTransform).DOSizeDelta(Vector2.one * 1280, 0.25f);
        //tokenTemplate.glow.Play();
        shower.model = tokens.coins[currentView].model;
        shower.Play(tokenCountsShower);
        backgroundChanger.ProjectColors(tokens.coins[currentView].menuColor2, tokens.coins[currentView].menuColor1);

        var color2 = tokens.coins[currentView].menuColor2;
        var color1 = tokens.coins[currentView].menuColor1;
        ProjectParticleColor(debris, color1, color2);
        ProjectParticleColor(glow, color1, color2);
        ProjectParticleColor(sparks, color1, color2);
        ProjectParticleColor(highlights, color1, color2);

    }

    public void ProjectParticleColor(ParticleSystem system, Color max, Color min)
    {
        var main = system.main;

        var startcolor = main.startColor;
        max.a = 0.5f;
        min.a = 0.5f;

        startcolor.colorMax = max;
        startcolor.colorMin = min;

        main.startColor = startcolor;

    }


    private void Awake()
    {
        HandleTokens();
    }

    private void Update()
    {
        HandleGetCurrentTokenView();
    }
}

[System.Serializable]
public class TokenPanelTemplate
{
    public GameObject template;
    public TMP_Text title;
    public TMP_Text xp;
    public TMP_Text location;
    public Image icon;
    public TMP_Text money;
}

public class TokenPanelInfo : MonoBehaviour
{
    public GameObject template;
    public TMP_Text title;
    public TMP_Text xp;
    public TMP_Text location;
    public Image icon;
    public TMP_Text money;
}