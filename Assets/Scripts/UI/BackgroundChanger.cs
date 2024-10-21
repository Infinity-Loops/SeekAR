using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundChanger : MonoBehaviour
{
    public MeshRenderer bottomQuad;
    public MeshRenderer topQuad;
    public void ProjectColors(Color bottom, Color top)
    {
        bottomQuad.material.DOColor(bottom, 0.5f);
        topQuad.material.DOColor(top, 0.5f);
    }
}
