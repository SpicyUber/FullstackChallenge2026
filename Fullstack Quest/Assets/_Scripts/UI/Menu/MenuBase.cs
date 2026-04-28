using System;
using System.Collections;
using UnityEngine;

public abstract class MenuBase : MonoBehaviour, IGenericDataMenu
{
    [Header("Pop Parameters")]
    [SerializeField] protected float _scaleMultiplier = 1.1f;
    [SerializeField] protected float _duration = 0.15f;

    [Header("Drop Parameters")]
    [SerializeField] protected float _dropDuration = 0.25f;
    [SerializeField] protected float _dropDistance = 300f;
    [SerializeField] protected float _dropRotation = -30f;

    private Coroutine dropRoutine;

    private Vector3 _localScale = Vector3.zero;

    private Vector3 _startingPosition, _startingScale;
    private Quaternion _startingRotation;

    private Coroutine currentRoutine;

    public abstract Type DataType { get; }

    private void OnEnable()
    {
        var rectTransform = GetComponent<RectTransform>();
        _startingPosition = rectTransform.localPosition;

        _startingScale = rectTransform.localScale;
        _startingRotation = rectTransform.localRotation;

        Pop(rectTransform);
    }

    private void Pop(RectTransform target)
    {
        if(currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(PopRoutine(target));


    }

    private IEnumerator PopRoutine(RectTransform target)
    {
        if(_localScale == Vector3.zero) _localScale = target.localScale;
        Vector3 targetScale = _localScale * _scaleMultiplier;

        float half = _duration / 2f;
        float t = 0f;


        while(t < half)
        {
            t += Time.unscaledDeltaTime;
            float progress = t / half;

            target.localScale = Vector3.Lerp(_localScale, targetScale, EaseOut(progress));
            yield return null;
        }

        t = 0f;

        while(t < half)
        {
            t += Time.unscaledDeltaTime;
            float progress = t / half;

            target.localScale = Vector3.Lerp(targetScale, _localScale, EaseIn(progress));
            yield return null;
        }

        target.localScale = _localScale;
        currentRoutine = null;
    }

    private float EaseOut(float x)
    {
        return 1f - Mathf.Pow(1f - x, 3f);
    }

    private float EaseIn(float x)
    {
        return x * x * x;
    }

    public float Drop()
    {
        if(dropRoutine != null)
            StopCoroutine(dropRoutine);


        dropRoutine = StartCoroutine(DropRoutine(GetComponent<RectTransform>()));

        return _dropDuration;
    }

    private IEnumerator DropRoutine(RectTransform target)
    {
        Vector3 startPos = target.anchoredPosition;
        Vector3 endPos = startPos + new Vector3(_dropDistance, 0f, 0f);

        Quaternion startRot = target.localRotation;
        Quaternion endRot = startRot * Quaternion.Euler(0f, 0f, _dropRotation);

        float t = 0f;

        while(t < _dropDuration)
        {
            t += Time.unscaledDeltaTime;
            float progress = t / _dropDuration;

            float eased = EaseIn(progress);

            target.anchoredPosition = Vector3.Lerp(startPos, endPos, eased);
            target.localRotation = Quaternion.Lerp(startRot, endRot, eased);

            yield return null;
        }

        target.anchoredPosition = startPos;
        target.localRotation = startRot;

        dropRoutine = null;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        var rectTransform = GetComponent<RectTransform>();

        rectTransform.localPosition = _startingPosition;
        rectTransform.localScale = _startingScale;
        rectTransform.localRotation = _startingRotation;
    }

    public abstract void Load(object data);
}

