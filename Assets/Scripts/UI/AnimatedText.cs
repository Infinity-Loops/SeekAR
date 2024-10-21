using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Windows;

public class AnimatedText : MonoBehaviour
{
    public string wordToAnimate;
    public float stageDuration;
    private List<TextAnimationStage> animations = new List<TextAnimationStage>();
    private TMP_Text text;
    bool running;
    int animationIndex;
    private void OnEnable()
    {
        text = GetComponent<TMP_Text>();
        running = true;
        HandleGenerateCombinations();
        StartCoroutine(Animate());
    }

    private void OnDisable()
    {
        running = false;
    }

    private void OnDestroy()
    {
        running = false;
    }

    public void HandleGenerateCombinations()
    {
        List<string> combinations = InternalHandleGenerateCombinations(wordToAnimate);

        foreach (string combination in combinations)
        {
            var stage = new TextAnimationStage();
            stage.text = combination;
            stage.duration = stageDuration;
            animations.Add(stage);
        }
    }

    List<string> InternalHandleGenerateCombinations(string input)
    {
        List<string> results = new List<string>();
        char[] chars = input.ToCharArray();

        // Loop through each letter in the input string
        for (int i = 0; i < input.Length; i++)
        {
            if (char.IsLetter(chars[i]))
            {
                char[] combination = input.ToCharArray(); // Create a copy of the original string

                // Set all letters to lowercase
                for (int j = 0; j < combination.Length; j++)
                {
                    combination[j] = char.ToLower(combination[j]);
                }

                // Set the current letter to uppercase
                combination[i] = char.ToUpper(combination[i]);

                // Add the new combination to the results list
                results.Add(new string(combination));
            }
        }

        return results;
    }

    IEnumerator Animate()
    {
        while (running)
        {
            animationIndex++;

            if (animationIndex > animations.Count - 1)
            {
                animationIndex = 0;
            }

            var anim = animations[animationIndex];

            text.text = anim.text;
            yield return new WaitForSeconds(anim.duration);
            yield return null;
        }
    }
}

[System.Serializable]
public class TextAnimationStage
{
    public string text;
    public float duration;
}