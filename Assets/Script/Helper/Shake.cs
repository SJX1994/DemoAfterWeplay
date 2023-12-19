using UnityEngine;
using DG.Tweening;

public class Shake : MonoBehaviour
{
    Tween shakeTween;
    Vector3 originalPosition;
    Vector3 originalScale;
    void Start()
    {
        originalPosition = transform.position;
        originalScale = transform.localScale;
    }
    public void ShakeObjectPosition()
    {
        // 设置振动的参数
        float duration = 0.5f;     // 振动的持续时间
        float strength = 0.6f;   // 振动的强度
        int vibrato = 10;        // 振动的频率
        // 开始振动
        transform.position = originalPosition;
        shakeTween?.Kill();
        shakeTween = transform.DOShakePosition(duration, strength, vibrato);
        shakeTween.OnComplete (() => {
            transform.position = originalPosition;
        });
    }
    public void ShakeObjectScale()
    {
        // 设置振动的参数
        float duration = 0.2f;     // 振动的持续时间
        float strength = 0.25f;   // 振动的强度
        int vibrato = 20;        // 振动的频率
        // 开始振动
        // originalPosition = transform.position;
        shakeTween?.Kill();
        shakeTween = transform.DOShakeScale(duration, strength, vibrato);
        shakeTween.OnComplete (() => {
             transform.localScale = originalScale;
        });
    }
    public void StopShake()
    {
        shakeTween?.Kill();
        transform.position = originalPosition;
    }
}