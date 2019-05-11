using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PitchDetector;

public class AudioAnalyze : MonoBehaviour
{
    public AudioSource Audio;
    public float RmsValue;
    public float DbValue;
    public float PitchValue;
 
    private const int QSamples = 1024;
    private const float RefValue = 0.1f;
    private const float Threshold = 0.02f;

    float[] samples;
    private float[] spectrum;
    private float _fSample;
 
    void Start()
    {
        Audio = GetComponent<AudioSource>();
        samples = new float[QSamples];
        spectrum = new float[QSamples];
        _fSample = AudioSettings.outputSampleRate;
    }

    private void Update()
    {
        var pitch = AnalyzeSound();
        print("pitch value " + pitch);
        var midi = RAPTPitchDetectorExtensions.HerzToFloatMidi(pitch);
        Debug.Log("midi " + midi);
    }

    public float AnalyzeSound()
    {
        Audio.GetOutputData(samples, 0); // fill array with samples
        int i;
        float sum = 0;
        for (i = 0; i < QSamples; i++)
        {
            sum += samples[i] * samples[i]; // sum squared samples
        }
        RmsValue = Mathf.Sqrt(sum / QSamples); // rms = square root of average
        DbValue = 20 * Mathf.Log10(RmsValue / RefValue); // calculate dB
        if (DbValue < -160) DbValue = -160; // clamp it to -160dB min
                                             // get sound spectrum
        Audio.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);
        float maxV = 0;
        var maxN = 0;
        for (i = 0; i < QSamples; i++)
        { // find max 
            if (!(spectrum[i] > maxV) || !(spectrum[i] > Threshold))
                continue;
 
            maxV = spectrum[i];
            maxN = i; // maxN is the index of max
        }
        float freqN = maxN; // pass the index to a float variable
        if (maxN > 0 && maxN < QSamples - 1)
        { // interpolate index using neighbours
            var dL = spectrum[maxN - 1] / spectrum[maxN];
            var dR = spectrum[maxN + 1] / spectrum[maxN];
            freqN += 0.5f * (dR * dR - dL * dL);
        }
        PitchValue = freqN * (_fSample / 2) / QSamples; // convert index to frequency
        return PitchValue;
    }
}