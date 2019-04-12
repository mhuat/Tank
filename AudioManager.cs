using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instace;
    [SerializeField]
    public List<AudioClip> clipList = new List<AudioClip>();

    private void Awake()
    {
        instace = this;
    }
}
