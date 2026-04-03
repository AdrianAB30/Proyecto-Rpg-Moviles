using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "UIAnimationSO", menuName = "Scriptable Objects/NewUIAnimationSO", order = 1)]

public class UIAnimationSO : ScriptableObject
{
    public enum AnimDirection
    {
        Top,
        Bottom,
        Left,
        Right
    }

    [Header("Configuración de Tiempo")]
    public float duration = 0.5f;
    public Ease easeIn = Ease.OutBack;
    public Ease easeOut = Ease.InBack;
    public float stayDuration = 0.5f;

    [Header("Dirección de Entrada")]
    public AnimDirection enterFrom;


    public void PlayEnter(RectTransform target, TweenCallback onComplete = null)
    {
        target.gameObject.SetActive(true);
        target.DOKill();

        target.anchoredPosition = GetOffScreenPosition(enterFrom);
        target.DOAnchorPos(Vector2.zero, duration).SetEase(easeIn).OnComplete(onComplete);
    }

    public void PlayExit(RectTransform target, TweenCallback onComplete = null)
    {
        target.DOKill();
        Vector2 exitPos = GetOffScreenPosition(enterFrom);

        target.DOAnchorPos(exitPos, duration).SetEase(easeOut).OnComplete(() =>
        {
            target.gameObject.SetActive(false);
            onComplete?.Invoke();
        });
    }
    private Vector2 GetOffScreenPosition(AnimDirection dir)
    {
        switch (dir)
        {
            case AnimDirection.Top: return new Vector2(0, Screen.height);
            case AnimDirection.Bottom: return new Vector2(0, -Screen.height);
            case AnimDirection.Left: return new Vector2(-Screen.width, 0);
            case AnimDirection.Right: return new Vector2(Screen.width, 0);
            default: return Vector2.zero;
        }
    }
}
