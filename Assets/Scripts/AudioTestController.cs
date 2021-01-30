using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTestController : MonoBehaviour
{
    public AudioClip clip_A;
    public AudioClip clip_B;
    public AudioClip clip_C;

    public string playSound_A_Key = "1";
    public string playSound_B_Key = "2";
    public string playSound_C_Key = "3";

    void Update()
    {
        if (Input.GetKeyDown(playSound_A_Key))
        {
            AudioManager.instance.PlayNewSound(clip_A, gameObject);
        }
    }
}
