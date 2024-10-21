using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class WalletWordButton : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    public WalletCrackDb walletCrackDb;
    private List<string> wordList = new List<string>();
    private int currentIndex;
    private void Awake()
    {
        HandleSetupDropdown();
    }

    void HandlePopulate()
    {
        foreach (var session in walletCrackDb.sessions)
        {
            wordList.AddRange(session.words);
        }

        wordList.OrderBy(x=> Random.Range(0, wordList.Count));

        foreach (var wprd in wordList)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
            option.text = wprd;
            dropdown.options.Add(option);
        }
    }

    void HandleSetupDropdown()
    {
        HandlePopulate();
        dropdown.onValueChanged.AddListener(HandleOptionChange);
        dropdown.value = Random.Range(0, wordList.Count);
    }

    void HandleOptionChange(int val)
    {
        currentIndex = val;
    }

    public string GetWord()
    {
        return wordList[currentIndex];
    }
}
