using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PitchDetector;

public class AudioVisualizer : MonoBehaviour {

    public Transform[] audioSpectrumObjects;
	[Range(1, 300)] public float heightMultiplier;
	private int numberOfSamples = 512;
	public FFTWindow fftWindow;
	public float lerpTime = 1;

    /*
     * The intensity of the frequencies found between 0 and 22050 will be
     * grouped into 512 elements. So each element will contain a range of about 43.06 Hz.
     * The average human voice spans from about 60 hz to 9k Hz
    */
    // We assign a range to each object that gets animated
    void Update() {
		// initialize our float array
		float[] spectrum = new float[numberOfSamples];
		// populate array with fequency spectrum data
		GetComponent<AudioSource>().GetSpectrumData(spectrum, 0, fftWindow);

		// loop over spectrumObjects and modify according to fequency spectrum data
		// this loop matches the Array element to an object on a One-to-One basis.
		int sampleID = 0;
		int pow = 0;
		for(int i = 0; i < (int)audioSpectrumObjects.Length; i += 2)
		{
			float average = 0;
			int sampleCount = (int)Mathf.Pow (2, pow);
			if (i % 2 != 0) pow++;

			for (int j = 0; j < sampleCount; j++) {
				// for compensating value drops while frequency growth,
				// do * (sampleID + 1)
				average += spectrum [sampleID];
				// Debug.Log ("id " + sampleID);
				sampleID++;
			}
			// apply height multiplier to intensity
			float intensity = average * heightMultiplier;
			// calculate object's scale
			float lerpY1 = Mathf.Lerp(audioSpectrumObjects[i].localScale.y,intensity,lerpTime);
			Vector3 newScale1 = new Vector3(audioSpectrumObjects[i].localScale.x, lerpY1, audioSpectrumObjects[i].localScale.z);

			// calculate object's scale
			 float lerpY2 = Mathf.Lerp(audioSpectrumObjects[i+1].localScale.y,intensity,lerpTime);
			 Vector3 newScale2 = new Vector3(audioSpectrumObjects[i+1].localScale.x, lerpY2, audioSpectrumObjects[i+1].localScale.z);

            // appply new scale to object
            audioSpectrumObjects[i].localScale = newScale1;
            audioSpectrumObjects[i+1].localScale = newScale2;
		}
	}
}