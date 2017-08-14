using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using System;

[RequireComponent(typeof(SpriteRenderer))] [ExecuteInEditMode]
public class ImageAnimator : MonoBehaviour 
{

    public float FramesPerSecond = 10;
    public new AnimationFile animation;
    public bool AutoPlay;

    public bool Playing { get {return _playing;}}

    public Dictionary<string, Sprite[]> Animations = new Dictionary<string, Sprite[]>();

    public Action EndCallback;

    private Sprite[] _currentAnimation;
    private int _currentFrame = 0;

    private SpriteRenderer _image;

    private bool _playing;

    private bool _explicitlyStoped = false;
    
    private bool _initialized = false;

    private void _init()
    {
        if (_initialized) return;
        _image = GetComponent<SpriteRenderer>();
        _currentAnimation = animation.Sprites;
        _currentFrame = 0;
        _initialized = true;
    }
    
    public void Start() {
        _init();
    }

    public void OnEnable () 
    {
        _init();
        if (AutoPlay && !_explicitlyStoped &&  Application.isPlaying)
            StartAnimation();
    }

    public void OnDisable()
    {
        _playing = false;
    }

    public void StartAnimation()
    {
        if (_playing) return;
        _playing = true;
        _explicitlyStoped = false;
        StartCoroutine(_updater());
    }

    public void StopAnimation()
    {
        _playing = false;
        _explicitlyStoped = false;
    }

    public void ResumeAnimation()
    {
        if (_playing) return;
        _playing = true;
        _explicitlyStoped = false;
        StartCoroutine(_updater());
    }

    public void RestartAnimation()
    {
        _currentFrame = 0;
        _explicitlyStoped = false;
    }

    public void SwitchAnimation(string animation)
    {
        if (Animations.ContainsKey(animation))
            _currentAnimation = Animations[animation];
        else Debug.LogError("No animation \'" + animation + "\' in this ImageAnimator");
    }

    private IEnumerator _updater()
    {
        if (!_playing) yield break;
        yield return new WaitForSeconds(animation.TimeMultiplier[_currentFrame]/FramesPerSecond);
        _currentFrame++;
        if (_currentFrame >= _currentAnimation.Length)
        {
            _currentFrame = 0;
            EndCallback?.Invoke();
        }
        _image.sprite = _currentAnimation[_currentFrame];
        StartCoroutine(_updater());
    }
}