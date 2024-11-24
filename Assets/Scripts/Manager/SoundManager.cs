using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource soundLevelUnlock;
    public AudioSource soundDamage;

    [Space(10)]
    public AudioSource musicLevel;
    public AudioSource musicFon;

    public static SoundManager InstanceSound { get; private set; }

    private void Awake()
    {
        if (InstanceSound != null && InstanceSound != this)
        {
            Destroy(gameObject);
        }
        else
        {
            InstanceSound = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
