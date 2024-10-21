using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using DG.Tweening;
using UnityEngine;

public static class SkinnedUtils
{
    public static TweenerCore<float, float, FloatOptions> DoShapeKey(this SkinnedMeshRenderer target,int index, float endValue, float duration)
    {
        TweenerCore<float, float, FloatOptions> tweenerCore = DOTween.To(() => target.GetBlendShapeWeight(index), delegate (float x)
        {
            target.SetBlendShapeWeight(index, endValue);
        }, endValue, duration);
        tweenerCore.SetTarget(target);
        return tweenerCore;
    }

}
