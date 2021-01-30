using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance = null;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else 
        {
            Destroy(gameObject);
        }
    }

    //==================================================================
    //General Audio Functions
    //==================================================================


    //instantiate a new AudioSource with optional gain, pitch, and spatial blend (2D by default)
    //destory AudioSource when finished playing
    public void PlayNewSound(AudioClip sound, GameObject sourceObject,
                            AudioMixerGroup bus = null, float gainInDecibels = 0f,
                            float pitchChangeInCents = 0f, float spatialBlend = 0f,
                            float delayInSeconds = 0f)
    {
        if (sound == null)
        {
            Debug.LogWarning("No AudioClip available to play");
            return;
        }

        AudioSource src = sourceObject.AddComponent<AudioSource>();
        src.clip = sound;
        src.spatialBlend = spatialBlend;
        src.outputAudioMixerGroup = bus;

        src.volume = DecibelToLinear(gainInDecibels);
        src.pitch = SemitonesToSpeed(pitchChangeInCents);


        src.PlayDelayed(delayInSeconds);




        Destroy(src, src.clip.length + delayInSeconds + 0.1f);
    }



    //instantiate a new AudioSource with random gain and pitch,
    //plus optional spatial blend. Destroy AudioSource when finished playing
    public void PlayNewSoundWithRandomParameters(AudioClip sound, GameObject sourceObject,
                                            float minPitchInSemitones, float maxPitchinSemitones,
                                            float minGainInDecibels = 0f, float maxGainInDecibels = 0f,
                                            float spatialBlend = 0f, AudioMixerGroup bus = null,
                                            float minDelay = 0f, float maxDelay = 0f)
    {
        if (sound == null)
        {
            Debug.LogWarning("No AudioClip available to play");
            return;
        }

        AudioSource src = sourceObject.AddComponent<AudioSource>();
        src.clip = sound;
        src.outputAudioMixerGroup = bus;

        float randGain = Random.Range(minGainInDecibels, maxGainInDecibels);
        float randPitch = Random.Range(minPitchInSemitones, maxPitchinSemitones);
        float randDelay = Random.Range(minDelay, maxDelay);

        src.volume = DecibelToLinear(randGain); src.pitch = SemitonesToSpeed(randPitch);
        src.spatialBlend = spatialBlend;

        src.PlayDelayed(randDelay);
        Destroy(src, src.clip.length + randDelay + 0.1f);
    }



    //Change the volume of an audiosource over a given period of time
    //useful for fade-ins/fade-outs
    public void ChangeVolumeOverTime(AudioSource source, float newVolumeInDecibels,
                                    float timeInSeconds, float delayInSeconds = 0f)
    {
        StopCoroutine("CR_ChangeVolumeOverTime");
        StartCoroutine(CR_ChangeVolumeOverTime(source, newVolumeInDecibels, timeInSeconds, delayInSeconds));
    }


    IEnumerator CR_ChangeVolumeOverTime(AudioSource source, float newVolumeInDecibels,
                                        float timeInSeconds, float delayInSeconds = 0f)
    {
        if (delayInSeconds > 0)
            yield return new WaitForSeconds(delayInSeconds);


        float currentVolume = source.volume;
        float targetVolume = DecibelToLinear(newVolumeInDecibels);
        float elapsedTime = 0.0f;

        while (elapsedTime < timeInSeconds)
        {
            source.volume = Mathf.Lerp(currentVolume, targetVolume, elapsedTime / timeInSeconds);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }




    //==================================================================
    //Audio Math Methods
    //==================================================================

    //Linear-to-dB gain conversion
    float LinearToDecibel(float linear)
    {
        float dB;

        if (linear != 0f)
            dB = 20f * Mathf.Log10(linear);
        else
            dB = -144f;


        return dB;
    }


    //dB-to-linear gain conversion
    float DecibelToLinear(float dB)
    {
        float linear = Mathf.Pow(10f, dB / 20f);

        return linear;
    }


    //convert pitch change in cents to speed (Unity native pitch-change)
    float SemitonesToSpeed(float cents)
    {
        return Mathf.Pow(2f, cents / 12f);
    }
}
