using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Analyzes pitch of Microphone input. After analysis, onPitchDetected event is invoked.
namespace PitchDetector {

    [System.Serializable]
    public class PitchEvent : UnityEvent<List<float>, int, float> {
    }

    public class MicrophonePitchDetector : MonoBehaviour {

        public PitchEvent onPitchDetected;
        public int micSampleRate = 16000;

        private RAPTPitchDetector pitchDetector;
        private bool recording;

		public float minThreshold = 0f;
		public float frequency = 0.0f;
		public int audioSampleRate = 44100;
		private string microphone;
		public FFTWindow fftWindow;
	
		private int samples = 8192; 
		private AudioSource audioSource;
	
        private RAPTPitchDetector Detector {
            get {
                if (pitchDetector == null) {
                    pitchDetector = new RAPTPitchDetector((float)micSampleRate, 50f, 800f);
                }
                return pitchDetector;
            }
        }

		void Start() {
			//get components you'll need
			audioSource = GetComponent<AudioSource> ();
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
				Debug.Log ("threshold" + minThreshold);

				// Start playing the audio source
				audioSource.Play (); 
			} else {
				//microphone doesn't work for some reason

				Debug.Log (microphone + " doesn't work!");
			}

			Debug.Log ("start");
			recording = true;
			StartCoroutine(RecordingCoroutine(audioSource.clip));
		}


        private IEnumerator RecordingCoroutine(AudioClip rec) {
            recording = true;
			Debug.Log ("hi");
            // AudioClip rec = Microphone.Start(null, true, 1, micSampleRate);
			// rec = Microphone.Start(microphone, true, 1, micSampleRate);
            float[] readBuffer = new float[rec.samples];
            int recPos = 0;
            int prevRecPos = 0;
            Func<bool> enoughSamples = () => { 
                int count = (readBuffer.Length + Microphone.GetPosition(null) - prevRecPos) % readBuffer.Length;
                return count > Detector.RequiredSampleCount((float)micSampleRate);
            };
            while (recording) {
                prevRecPos = recPos;
                yield return new WaitUntil(enoughSamples);
                recPos = Microphone.GetPosition(null);
                rec.GetData(readBuffer, 0);
                int sampleCount = (readBuffer.Length + recPos - prevRecPos) % readBuffer.Length;
                float db = 0f;
                List<float> pitchValues = Detector.getPitch(readBuffer, prevRecPos, ref recPos, ref db, (float)micSampleRate, true, !recording);
                sampleCount = (readBuffer.Length + recPos - prevRecPos) % readBuffer.Length;
				if (sampleCount > 0) {
                    onPitchDetected.Invoke(pitchValues, sampleCount, db);
                }
            }
            Microphone.End(null);
        }

		public float GetAveragedVolume()
		{ 
			float[] data = new float[512];
			float a = 0;
			audioSource.GetOutputData(data,0);
			foreach(float s in data)
			{
				a += Mathf.Abs(s);
			}
			return a/512;
		}
	
		public float GetFundamentalFrequency()
		{
			float fundamentalFrequency = 0.0f;
			float[] data = new float[samples];
			audioSource.GetSpectrumData(data,0,fftWindow);
			float s = 0.0f;
			int i = 0;
			for (int j = 1; j < samples; j++)
			{
				if(data[j] > minThreshold) // volumn must meet minimum threshold
				{
					if ( s < data[j] )
					{
						s = data[j];
						i = j;
					}
				}
			}
			fundamentalFrequency = i * audioSampleRate / samples;
			frequency = fundamentalFrequency;
			Debug.Log("frequency" + frequency);
			return fundamentalFrequency;
		}

    }
}