using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;



public class MainMenu : MonoBehaviour
{
    [SerializeField] private Image blackBack;
    [SerializeField] private float time = 1.0f;

    static private Image BlackBack;
    static private float Time;

    static Sequence sequenceFadeInOut;
    public class GlobalValue
    {
        public enum Transition
        {
            Fade,
        }
    }

    private void Awake()
    {
        BlackBack = blackBack;
        Time = time;

        BlackBack.enabled = false;
    }

    private void Start()
    {
        sequenceFadeInOut = DOTween.Sequence()
            .SetAutoKill(false)
            .OnRewind(() =>
            {
                BlackBack.enabled = true;
            })
            .Append(BlackBack.DOFade(1.0f, Time))
            .Append(BlackBack.DOFade(0.0f, Time))
            .OnComplete(() =>
            {
                BlackBack.enabled = false;
            });
    }

    static public void Play(GlobalValue.Transition transition)
    {
        switch (transition)
        {
            case GlobalValue.Transition.Fade: FadeInOut(); break;
        }
    }

    static public void FadeInOut()
    {
        sequenceFadeInOut.Restart();
    }
    public void QuitGame()
    {
        Application.Quit();
    }

}