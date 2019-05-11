﻿using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MicrophoneInput : MonoBehaviour {
    // Convert the microphone input into the audiosource

	public float minThreshold = 0f;
	public float frequency = 0.0f;
	public int audioSampleRate = 44100;
	private string microphone;
	public FFTWindow fftWindow;

	private int samples = 512; 
	private AudioSource audioSource;

	void Start() {
		//get components you'll need
		audioSource = GetComponent<AudioSource> ();
		// get all available microphones
		Debug.Log (Microphone.devices + " length " + Microphone.devices.Length + Microphone.devices[0]);
		foreach (string device in Microphone.devices) {
			if (microphone == null) {
				//set default mic to first mic found.
				Debug.Log("device: " + device);
				microphone = device;
			}
		}
		Debug.Log ("so.. " + microphone);
		minThreshold = 0.7f;

		//initialize input with default mic
		 UpdateMicrophone ();
	}

	void UpdateMicrophone(){
		audioSource.Stop(); 
		//Start recording to audioclip from the mic
		audioSource.clip = Microphone.Start(microphone, true, 10, audioSampleRate);
		audioSource.loop = true; 
		// Mute the sound with an Audio Mixer group becuase we don't want the player to hear it
		Debug.Log(Microphone.IsRecording(microphone).ToString());

		if (Microphone.IsRecording (microphone)) { //check that the mic is recording, otherwise you'll get stuck in an infinite loop waiting for it to start
			while (!(Microphone.GetPosition (microphone) > 0)) {
			} // Wait until the recording has started. 
			Debug.Log ("recording started with " + microphone);
			// Start playing the audio source
			audioSource.Play (); 
		} else {
			//microphone doesn't work for some reason
			Debug.Log (microphone + " doesn't work!");
		}
	}
}