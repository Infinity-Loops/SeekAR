using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizEvent : SpawnableEvent
{
    public GameObject quizObject;
    public override GameObject spawnableObject()
    {
        return quizObject;
    }
}
