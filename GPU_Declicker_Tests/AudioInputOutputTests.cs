﻿
using System;
using System.Threading.Tasks;
using Windows.Storage;
using GPU_Declicker_UWP_0._01;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GPU_Declicker_Tests
{
    [TestClass]
    public class AudioInputOutputTests
    {
        [TestMethod]
        public async Task AudioInputOutputSaveLoadTest()
        {
            Progress<double> taskProgress = new Progress<double>();
            Progress<string> taskStatus = new Progress<string>();

            AudioInputOutput audioInputOutput =
                new AudioInputOutput();

            await audioInputOutput.Init(taskProgress);

            int audioLength = 44100;
            float[] input_audio = new float[audioLength];

            for (int i = 0; i < input_audio.Length; i++)
            {
                input_audio[i] = (float)Math.Sin(2 * Math.PI * i / (audioLength / 5));
            }

            AudioDataMono audioData =
                new AudioDataMono(input_audio);

            for (int index = 0; index < audioData.LengthSamples(); index++)
            {
                audioData.SetOutputSample(
                    index,
                    audioData.GetInputSample(index));
            }

            audioInputOutput.SetAudioData(audioData);

            StorageFolder testFolder = KnownFolders.MusicLibrary;
            StorageFile audioOutputFile = await testFolder.GetFileAsync("test.wav");

            if (audioOutputFile != null)
            {
                await audioInputOutput.SaveAudioToFile(audioOutputFile, taskProgress, taskStatus);
            }
        }
    }
}