using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PitchDetector;


public class VisualSpawn : MonoBehaviour {

    public static VisualSpawn instance;

    // each list contains the visuals that correspond to their color bin
    public GameObject[] visualList1;
    public GameObject[] visualList2;
    public GameObject[] visualList3;
    public GameObject[] visualList4;
    public GameObject[] visualList5;
    public GameObject[] visualList6;
    public GameObject[] visualList7;
    public GameObject[] visualList8;
    public GameObject[] visualList9;
    public GameObject[] visualList10;

    // total 18 spawn positions
    public GameObject spawnPosition1;
    public GameObject spawnPosition2;
    public GameObject spawnPosition3;
    public GameObject spawnPosition4;
    public GameObject spawnPosition5;
    public GameObject spawnPosition6;
    public GameObject spawnPosition7;
    public GameObject spawnPosition8;
    public GameObject spawnPosition9;
    public GameObject spawnPosition10;
    public GameObject spawnPosition11;
    public GameObject spawnPosition12;
    public GameObject spawnPosition13;
    public GameObject spawnPosition14;
    public GameObject spawnPosition15;
    public GameObject spawnPosition16;
    public GameObject spawnPosition17;
    public GameObject spawnPosition18;



    public Dictionary<int, SpawnArea> dictionary = new Dictionary<int, SpawnArea>();

    private int spawnNum = 2;
    private readonly int maxVisuals = 15;
    private int numVisuals = 0;

    public float RmsValue;
    public float DbValue;

    private const int QSamples = 1024;
    private const float RefValue = 0.1f;
    private const float Threshold = 0.02f;

    float[] samples;
    private float[] spectrum;
    private float _fSample;

    public AudioVisualizer visualizer;

    // Class that holds a bool (whether the spawn area is occupied) and its visual
    public class SpawnArea
    {
        public SpawnArea(bool occupied, GameObject spawnPosition)
        {
            this.occupied = occupied;
            //this.name = name;
            this.spawnPosition = spawnPosition;
        }
        public bool occupied;
        //public string name;
        public GameObject spawnPosition;
    };

    // set event handlers
    void Start () {
        instance = this;
        samples = new float[QSamples];
        spectrum = new float[QSamples];
        _fSample = AudioSettings.outputSampleRate;

        dictionary.Add(1, new SpawnArea(false, spawnPosition1));
        dictionary.Add(2, new SpawnArea(false, spawnPosition2));
        dictionary.Add(3, new SpawnArea(false, spawnPosition3));
        dictionary.Add(4, new SpawnArea(false, spawnPosition4));
        dictionary.Add(5, new SpawnArea(false, spawnPosition5));
        dictionary.Add(6, new SpawnArea(false, spawnPosition6));
        dictionary.Add(7, new SpawnArea(false, spawnPosition7));
        dictionary.Add(8, new SpawnArea(false, spawnPosition8));
        dictionary.Add(9, new SpawnArea(false, spawnPosition9));
        dictionary.Add(10, new SpawnArea(false, spawnPosition10));
        dictionary.Add(11, new SpawnArea(false, spawnPosition11));
        dictionary.Add(12, new SpawnArea(false, spawnPosition12));
        dictionary.Add(13, new SpawnArea(false, spawnPosition13));
        dictionary.Add(14, new SpawnArea(false, spawnPosition14));
        dictionary.Add(15, new SpawnArea(false, spawnPosition15));
        dictionary.Add(16, new SpawnArea(false, spawnPosition16));
        dictionary.Add(17, new SpawnArea(false, spawnPosition17));
        dictionary.Add(18, new SpawnArea(false, spawnPosition18));
	}

    private void Update()
    {
        if (numVisuals < maxVisuals)
        {
            var pitch = AnalyzeSound();
            print("ptich " + pitch);
            var midi = RAPTPitchDetectorExtensions.HerzToFloatMidi(pitch);
            Debug.Log("midi " + midi);
            if (!float.IsInfinity(midi))
            {
                // instantiate 3D object corresponding to the pitch
                mapColor(midi);
            }
        }
    }


    // visuals will be assigned based on the range where the input midi value falls into
    public void mapColor(float midi)
    {
        // Total 10 color bins
        // 1: < 24
        // 2: 24 - 34
        // 3: 34 - 44
        // 4: 44 - 54
        // 5: 54 - 64
        // 6: 64 - 74
        // 7: 74 - 84
        // 8: 84 - 94
        // 9: 94 - 104
        // 10: < 104 
        
        if (midi <= 24)
        {
            Spawn (visualList1);
        }
        else if (midi <= 34)
        {
            Spawn (visualList2);
        }
        else if (midi <= 44)
        {
            Spawn (visualList3);
        }
        else if (midi <= 54)
        {
            Spawn (visualList4);
        }
        else if (midi <= 64)
        {
            Spawn (visualList5);
        }
        else if (midi <= 74)
        {
            Spawn (visualList6);
        }
        else if (midi <= 84)
        {
            Spawn (visualList7);
        }
        else if (midi <= 94)
        {
            Spawn (visualList8);
        }
        else if (midi <= 104)      
        {
            Spawn (visualList9);
        }
        else
        {
            Spawn (visualList10);
        }
    }

    public void Spawn(GameObject[] visualInputs)
    {
        List<int> randomList = GenerateRandomList();
        Debug.Log(randomList);
        for (int i = 0; i < randomList.Count; i++)
        {
            var index = randomList[i];
            var visualIndex = UnityEngine.Random.Range(0, 4);
            Vector3 spawnPosition = dictionary[index].spawnPosition.transform.position;
            // change the spawn position to occupied = true
            dictionary[index] = new SpawnArea(true, dictionary[index].spawnPosition);
            // instantiate the visual at the given spawnPosition
            GameObject visual = Instantiate(visualInputs[visualIndex], spawnPosition, Quaternion.identity);
            numVisuals++;
            visual.name = index.ToString();
            // the spawned visual will be the child of the current game object
            visual.transform.parent = gameObject.transform;
        }
    }

    // Returns the list of 2 random, unique indexes that are not occupied
    public List<int> GenerateRandomList()
    {
        List<int> result = new List<int>();
        List<int> unoccupied = new List<int>();
        // add indexes that are not occupied
        for (int i = 1; i <= dictionary.Count; i++)
        {
            if (!dictionary[i].occupied) unoccupied.Add(i);
        }
        // by default, spawnNum = 2
        // if the number of unoccupied spots are less than spawnNum, 
        // just add all the unoccupied spots to the result list
        // otherwise add 3 unique random numbers to the result list
        if (unoccupied.Count < spawnNum)
        {
            for (int j = 0; j < unoccupied.Count; j++)
            {
                result.Add(unoccupied[j]);
            }
        } 
        else
        {
            for (int j = 0; j < spawnNum; j++)
            {
                int ranNum = unoccupied[UnityEngine.Random.Range(0, unoccupied.Count-1)];
                result.Add(ranNum);
                unoccupied.Remove(ranNum);
            }
        }
        return result;
    }

    // change the state(occupied) of the spawnPosition to false since it is spot is now available as
    // the previous visual has been destroyed
    public void BackToOriginal(string name)
    {
        var index = int.Parse(name);
        dictionary[index] = new SpawnArea(false, dictionary[index].spawnPosition);
        numVisuals--;
    }

    // Anaylze the sound
    public float AnalyzeSound()
    {
        AudioSource Audio = visualizer.GetComponent<AudioSource>();
        // fill array with samples
        Audio.GetOutputData(samples, 0); 
        int i;
        float sum = 0;
        for (i = 0; i < QSamples; i++)
        {
            // sum squared samples
            sum += samples[i] * samples[i]; 
        }
        // rms = square root of average
        RmsValue = Mathf.Sqrt(sum / QSamples);
        // calculate dB
        DbValue = 20 * Mathf.Log10(RmsValue / RefValue);
        // clamp it to -160dB min
        if (DbValue < -160) DbValue = -160;
        // get sound spectrum
        Audio.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);
        float maxV = 0;
        var maxN = 0;
        // find max 
        for (i = 0; i < QSamples; i++)
        { 
            if (!(spectrum[i] > maxV) || !(spectrum[i] > Threshold))
                continue;
            maxV = spectrum[i];
            maxN = i; // maxN is the index of max
        }
        // pass the index to a float variable
        float freqN = maxN;
        // interpolate index using neighbours
        if (maxN > 0 && maxN < QSamples - 1)
        { 
            var dL = spectrum[maxN - 1] / spectrum[maxN];
            var dR = spectrum[maxN + 1] / spectrum[maxN];
            freqN += 0.5f * (dR * dR - dL * dL);
        }
        // convert index to frequency
        float PitchValue = freqN * (_fSample / 2) / QSamples; 
        return PitchValue;
    }

}
