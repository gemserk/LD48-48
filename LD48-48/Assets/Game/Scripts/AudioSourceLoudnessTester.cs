using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Game.Scripts
{
    public class AudioSourceLoudnessTester : MonoBehaviour
    {
        public AudioSource audioSource;
        public float updateStep = 0.1f;
        public int sampleDataLength = 1024;

        private float currentUpdateTime = 0f;

        public float clipLoudness;
        private float[] clipSampleData;

        public GameObject cube;
        public float sizeFactor = 1;

        public float minSize = 0;
        public float maxSize = 500;

        public List<Light> lights;
        [NonSerialized]
        public List<List<GameObject>> dynamicLights;

        private Dictionary<Light, float> lightIntensities = new Dictionary<Light, float>();
        private Dictionary<GameObject, LightBeatController> lightComponents = new Dictionary<GameObject, LightBeatController>();
        public WallsSpawner spawner;

        public MineCartHud hud;

        // Use this for initialization
        private void Awake()
        {
            clipSampleData = new float[sampleDataLength];
            if (spawner != null)
            {
                dynamicLights = spawner.GetActiveLightsLists();
                dynamicLights.Add(lights.Select(light1 => light1.gameObject).ToList());
            }
        }

        // Update is called once per frame
        private void Update()
        {
            currentUpdateTime += Time.deltaTime;
            if (currentUpdateTime >= updateStep)
            {
                currentUpdateTime = 0f;
                audioSource.clip.GetData(clipSampleData, audioSource.timeSamples); //I read 1024 samples, which is about 80 ms on a 44khz stereo clip, beginning at the current sample position of the clip.
                clipLoudness = 0f;
                foreach (var sample in clipSampleData)
                {
                    clipLoudness += Mathf.Abs(sample);
                }
                clipLoudness /= sampleDataLength; //clipLoudness is what you are looking for

                clipLoudness *= sizeFactor;
                clipLoudness = Mathf.Clamp(clipLoudness, minSize, maxSize);
                if (cube != null)
                {
                    cube.transform.localScale = new Vector3(clipLoudness, clipLoudness, clipLoudness);
                }


                foreach (var lightList in dynamicLights)
                {
                    foreach (var lightGO in lightList)
                    {
                        LightBeatController light = null;
                        if(!lightComponents.TryGetValue(lightGO, out light))
                        {
                            light = lightGO.GetComponentInChildren<LightBeatController>();
                            if (light == null)
                            {
                                light = lightGO.AddComponent<LightBeatController>();
                            }
                            lightComponents[lightGO] = light;
                        }

                        light.Change(clipLoudness);
                    }
                }

                if (hud != null)
                {
                    hud.musicIntensityMultiplier = clipLoudness;
                }
            }
        }
    }
}