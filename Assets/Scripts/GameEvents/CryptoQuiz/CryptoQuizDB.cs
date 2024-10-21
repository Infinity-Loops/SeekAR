using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CryptoQuizDB : ScriptableObject
{
    public List<CryptoQuizType> quiz = new List<CryptoQuizType>();
}
[Serializable]
public class CryptoQuizType
{
    public string coinName;
    public Sprite coinIcon;
    public List<QuizAnswerPage> pages = new List<QuizAnswerPage>();
}
[Serializable]
public class QuizAnswerPage
{
    public string question;
    public List<string> alternatives = new List<string>();
    public int correctAlternative;
}