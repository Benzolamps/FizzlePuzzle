﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using FizzlePuzzle.Core;
using FizzlePuzzle.Extension;
using FizzlePuzzle.Utility;
using NAudio.Flac;
using NVorbis;
using UnityEngine;
using NAudio.Wave;

namespace FizzlePuzzle.Scene
{
    internal class FizzleSceneSetting
    {
        private static readonly Dictionary<string, AudioClip> audioClipDictionary = new Dictionary<string, AudioClip>();
        private readonly Light lightPrefab;

        internal FizzleSceneSetting(Light lightPrefab)
        {
            this.lightPrefab = lightPrefab;
        }

        internal void Apply(Transform parent, LightSetting lightSetting, List<string> audioClips)
        {
            FizzleScene.Camera.GetComponent<Skybox>().material = lightSetting.Skybox;
            RenderSettings.ambientLight = lightSetting.AmbientLightColor.Replace(a: byte.MaxValue);
            RenderSettings.ambientIntensity = lightSetting.AmbientLightIntensity;
            Light light = FizzleBehaviour.Spawn(lightPrefab, parent, "light");
            light.color = lightSetting.DirectionalLightColor.Replace(a: byte.MaxValue);
            light.intensity = lightSetting.DirectionalLightIntensity;
            FizzleScene.StartOneCoroutine(PlayClip(parent.gameObject.GetComponent<AudioSource>(), audioClips));
        }

        private static List<AudioClip> GetAudioClips(IEnumerable<string> audioClips)
        {
            List<AudioClip> audioClipList = new List<AudioClip>();
            foreach (var index in audioClips.Select(CommonTools.ConvertPath))
            {
                if (audioClipDictionary.ContainsKey(index))
                {
                    audioClipList.Add(audioClipDictionary[index]);
                }
                else if (File.Exists(index))
                {
                    try
                    {
                        AudioClip audioClip2 = GetAudioClip(index);
                        audioClipList.Add(audioClip2);
                        audioClipDictionary[index] = audioClip2;
                    }
                    catch (Exception e)
                    {
                        FizzleDebug.LogException(e);
                        audioClipDictionary[index] = null;
                    }
                }
            }

            return audioClipList;
        }

        [SuppressMessage("ReSharper", "InvertIf")]
        private static AudioClip GetAudioClip(string path)
        {
            if (path.ToLower().EndsWith(".ogg"))
            {
                VorbisReader vorbis = new VorbisReader(new MemoryStream(File.ReadAllBytes(path)), true);
                return AudioClip.Create("audio clip", (int) (vorbis.SampleRate * vorbis.TotalTime.TotalSeconds), vorbis.Channels, vorbis.SampleRate, false, data =>
                {
                    float[] buffer = new float[data.Length];
                    vorbis.ReadSamples(buffer, 0, data.Length);
                    for (int index = 0; index < data.Length; ++index)
                    {
                        data[index] = buffer[index];
                    }
                }, position => vorbis.DecodedTime = new TimeSpan(position));
            }

            if (path.ToLower().EndsWith(".flac"))
            {
                FlacReader flac = new FlacReader(new MemoryStream(File.ReadAllBytes(path)));
                WAV wav = new WAV(AudioMemStream(flac).ToArray());
                AudioClip audioClip = AudioClip.Create("audio clip", wav.SampleCount, 1, wav.Frequency, false);
                audioClip.SetData(wav.LeftChannel, 0);
                return audioClip;
            }
            
            if (path.ToLower().EndsWith(".wav"))
            {
                WAV wav = new WAV(File.ReadAllBytes(path));
                AudioClip audioClip = AudioClip.Create("audio clip", wav.SampleCount, 1, wav.Frequency, false);
                audioClip.SetData(wav.LeftChannel, 0);
                return audioClip;
            }

            if (path.ToLower().EndsWith(".mp3"))
            {
                MemoryStream mp3Stream = new MemoryStream(File.ReadAllBytes(path));
                Mp3FileReader mp3Audio = new Mp3FileReader(mp3Stream);
                WaveStream waveStream = WaveFormatConversionStream.CreatePcmStream(mp3Audio);
                WAV wav = new WAV(AudioMemStream(waveStream).ToArray());
                AudioClip audioClip = AudioClip.Create("audio clip", wav.SampleCount, 1, wav.Frequency, false);
                audioClip.SetData(wav.LeftChannel, 0);
                return audioClip;
            }
            
            throw new FizzleException("不支持的音乐格式: " + path);
        }

        private static MemoryStream AudioMemStream(WaveStream waveStream)
        {
            MemoryStream outputStream = new MemoryStream();
            using (WaveFileWriter waveFileWriter = new WaveFileWriter(outputStream, waveStream.WaveFormat))
            {
                byte[] bytes = new byte[waveStream.Length];
                waveStream.Position = 0;
                waveStream.Read(bytes, 0, Convert.ToInt32(waveStream.Length));
                waveFileWriter.Write(bytes, 0, bytes.Length);
                waveFileWriter.Flush();
            }

            return outputStream;
        }


        private static IEnumerator PlayClip(AudioSource audioSource, IEnumerable<string> audioClips)
        {
            List<AudioClip> clips = GetAudioClips(audioClips);
            while (true)
            {
                do
                {
                    yield return new WaitForFixedUpdate();
                    audioSource.pitch = !FizzleScene.TimeCtrl.Rewinding ? 1.0F : !FizzleScene.TimeCtrl.TimeOut ? FizzleScene.TimeCtrl.CurrentRewindSpeed : 0.0F;
                } 
                while (audioSource.isPlaying || FizzleScene.TimeCtrl.TimeOut && FizzleScene.TimeCtrl.Rewinding);
                CommonTools.PlayRandomSound(audioSource, clips);
            }
        }
    }
}
