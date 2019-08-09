using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField]
    public List<AudioClip> clipList = new List<AudioClip>();

    private void Awake()
    {
        instance = this;
    }
}
