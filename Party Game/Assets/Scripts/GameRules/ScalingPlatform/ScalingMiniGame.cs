using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using TMPro;
using UnityEngine;

public class ScalingMiniGame : MonoBehaviour
{
    [Header("Platform Initial Settings")]
    [SerializeField] private int TimeLimit;
    [SerializeField] private float SmoothTime;
    [SerializeField] private float ScaleXZ;

    [Header("UI Display")]
    [SerializeField] private TextMeshProUGUI Message;

    [Header("Platform Constraints")]
    [SerializeField] private float XZScaleConstraint;

    private float _target;
    private float _velcoity;
    private bool _scaleDown = false;
    
    private Transform _transform;
    private Coroutine lastRoutine = null;
    
    private void Awake()
    {
        _transform = GetComponent<Transform>();
        _target = ScaleXZ;
    }

    private void Update()
    {
        if (_scaleDown && !IsScaledFully())
        {
            float newScale = Mathf.SmoothDamp(transform.localScale.x, _target, ref _velcoity, SmoothTime);
            _transform.localScale = new Vector3(newScale, 1, newScale);

            if (newScale <= _target)
                _scaleDown = false;
        }
    }

    public void StartMiniGame()
    {
        lastRoutine = StartCoroutine(Countdown(TimeLimit));
    }

    public void RestMiniGame()
    {
        StopCoroutine(lastRoutine);
        ResetScale();
    }

    private void ResetScale()
    {
        _scaleDown = false;
        _target = ScaleXZ;
        _transform.localScale = new Vector3(ScaleXZ, 1 , ScaleXZ);
    }
    

    IEnumerator Countdown(int seconds)
    {
        bool isFinished = false;
        
        while (!isFinished)
        {
            int time = seconds;

            while (time > 0)
            {
                yield return new WaitForSeconds(1);
                time--;

                var timespan = TimeSpan.FromSeconds(time);
                GameManager.instance.UiManager.InGameUI.ChangeTimeText(timespan.ToString(@"mm\:ss"));
            }

            isFinished = IsScaledFully();
            _target -= 0.5f;
            _scaleDown = true;
        }
        
        Debug.Log("It Stoppped");
    }
    
    private bool IsScaledFully()
    {
        float offset = 0.1f;
        
        return _transform.localScale.x <= XZScaleConstraint+offset;
    }
}
