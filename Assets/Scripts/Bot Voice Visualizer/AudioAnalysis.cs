using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

//TODO: Remove all "FormerlySerializedAs" tags once scene has been successfully saved

//big ups Rhythm Rain team

/// <summary>
/// Analyzes audio from AudioSource and finds average amplitude of 8 frequency bands
/// </summary>
public class AudioAnalysis : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Sound source to analyze")]
    [FormerlySerializedAs("_audioSource")]
    private AudioSource audioSource;

    private float[] samples = new float[512];

    /// <summary>
    /// raw data split into bands
    /// </summary>
    private float[] freqBand = new float[8];

    /// <summary>
    /// data on bands with smoothing applied
    /// </summary>
    private float[] bandBuffer = new float[8];

    /// <summary>
    /// rate bands should fall back towards zero
    /// </summary>
    private float[] bufferDecrease = new float[8];

    /// <summary>
    /// loudest each band has ever been, used to normalize output
    /// </summary>
    private float[] freqBandHighest = new float[8];

    /// <summary>
    /// this is a normalized version of freqBand
    /// </summary>
    private float[] audioBand = new float[8];
    public float[] AudioBand
    {
        get { return audioBand; }
        private set { audioBand = value; }
    }

    /// <summary>
    /// this is a normalized version of freqBandBuffer
    /// </summary>
    private float[] audioBandBuffer = new float[8];
    public float[] AudioBandBuffer
    {
        get { return audioBandBuffer; }
        private set { audioBandBuffer = value; }
    }

    /// <summary>
    /// overall loudness of sound
    /// </summary>
    private float amplitude = 0;
    public float Amplitude
    {
        get { return amplitude; }
        private set { amplitude = value; }
    }

    /// <summary>
    /// loudness with smoothing applied
    /// </summary>
    private float amplitudeBuffer = 0;
    public float AmplitudeBuffer
    {
        get { return amplitudeBuffer; }
        private set { amplitudeBuffer = value; }
    }

    /// <summary>
    /// used for normalizing amplitude
    /// </summary>
    private float amplitudeHighest = 1;

    public Color BassColor { get; private set; }
    public Color MidColor { get; private set; }
    public Color HighColor { get; private set; }

    private void Awake()
    {
        InitAudioProfile();
    }
    // Update is called once per frame
    private void Update()
    {
        GetSpectrumAudioSource();
        MakeFrequencyBands();
        BandBuffer();
        CreateAudioBands();
        GetAmplitude();
    }

    private void InitAudioProfile()
    {
        for (int x = 0; x < 8; x++)
        {
            // 5 is pretty much arbitrary. We just don't want to accidentally divide by zero later
            freqBandHighest[x] = 5;
        }
    }

    /// <summary>
    /// find the average amplitude of all bands and store it in Amplitude and AmplitudeBuffer
    /// </summary>
    private void GetAmplitude()
    {
        float currentAmplitude = 0;
        float currentAmplitudeBuffer = 0;

        for (int i = 0; i < 8; i++)
        {
            currentAmplitude += audioBand[i];
            currentAmplitudeBuffer += audioBandBuffer[i];
        }

        if (currentAmplitude > amplitudeHighest)
        {
            amplitudeHighest = currentAmplitude;
        }

        Amplitude = currentAmplitude / amplitudeHighest;
        AmplitudeBuffer = currentAmplitudeBuffer / amplitudeHighest;
    }

    /// <summary>
    /// normalize the results of analysis and store them in AudioBand and AudioBandBuffer
    /// </summary>
    private void CreateAudioBands()
    {
        for (int i = 0; i < 8; i++)
        {
            if (freqBand[i] > freqBandHighest[i])
            {
                freqBandHighest[i] = freqBand[i];
                Debug.Log(freqBandHighest[i]);
            }
            AudioBand[i] = (freqBand[i] / freqBandHighest[i]);
            AudioBandBuffer[i] = (bandBuffer[i] / freqBandHighest[i]);

        }
    }

    /// <summary>
    /// tell unity to do fourier analysis on the sound and give us some frequency data to work with
    /// </summary>
    private void GetSpectrumAudioSource()
    {
        audioSource.GetSpectrumData(samples, 0, FFTWindow.Blackman);
    }

    /// <summary>
    /// 
    /// </summary>
    private void MakeFrequencyBands()
    {
        /* which samples correspond to what frequency ranges?
         * 
         * 22050 / 512 = 43 hertz per sample
         * 
         * 20 - 60 hertz
         * 60 - 250 hertz
         * 250 - 500 hertz
         * 500 - 2000 hertz
         * 2000 - 4000 hertz
         * 4000 - 6000 hertz
         * 6000 - 20000 hertz
         * 
         * 0 - 2 = 86hertz
         * 1 - 4 = 172 hertz  - 87-258
         * 2 - 8 = 344 hertz  - 259-602
         * 3 - 16 = 688 hertz - 603-1290
         * 4 - 32 = 1376 hertz - 1291-2666
         * 5 - 64 = 2752 hertz - 2667-5418
         * 6 - 128 = 5504 hertz - 5419-10922
         * 7 - 256 = 11008 hertz - 10923-21930
         * 
         * 510 samples
         * 
         */

        int count = 0;

        for (int i = 0; i < 8; i++)
        {
            float average = 0;
            int sampleCount = (int)Mathf.Pow(2, i) * 2;

            if (i == 7)
            {
                //may as well use the last two samples as well
                sampleCount += 2;
            }

            for (int j = 0; j < sampleCount; j++)
            {
                average += samples[count] * (count + 1);
                count++;
            }

            average /= count;

            freqBand[i] = average * 10;
        }

    }

    /// <summary>
    /// if the current amplitude in each band is less than before, start falling at an exponential rate.
    /// looks better than lerping as we don't know in advance how much time this process should take
    /// </summary>
    private void BandBuffer()
    {
        for (int i = 0; i < 8; i++)
        {

            if (freqBand[i] > bandBuffer[i])
            {
                bandBuffer[i] = freqBand[i];
                bufferDecrease[i] = 0.005f;
            }

            if (freqBand[i] < bandBuffer[i])
            {
                bandBuffer[i] -= bufferDecrease[i];
                bufferDecrease[i] *= 1.2f;
            }
        }
    }
}
