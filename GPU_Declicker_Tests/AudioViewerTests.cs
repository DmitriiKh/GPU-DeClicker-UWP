﻿using System;
using Windows.Foundation;
using GPU_Declicker_UWP_0._01;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.AppContainer;

namespace GPU_Declicker_Tests
{
    [TestClass]
    public class AudioViewerTests
    {
        [UITestMethod]
        public void Fill_HighFreqSignal_DrawsMaxAndMins()
        {
            AudioViewer audioViewer = new AudioViewer
            {
                WaveFormWidth = 100,
                WaveFormHeight = 100
            };

            // length big enough to have 100 audio samples per 
            // one sample on WaveForm
            int audioLength = (int)audioViewer.WaveFormWidth * 100;

            float[] input_audio = new float[audioLength];

            for (int i = 0; i < input_audio.Length; i++)
            {
                input_audio[i] = (float)Math.Sin(2 * Math.PI * i / 
                    (audioLength / 
                    (audioViewer.WaveFormWidth * 5)// 5 waves per one sample 
                    // on WaveForm
                    )); 
            }

            AudioDataMono audioData =
                new AudioDataMono(input_audio);

            audioViewer.Fill(audioData);

            foreach (Point p in audioViewer.LeftChannelWaveFormPoints)
            {
                // each point should be at max (top) 
                // or at min (bottom of audioViewer)
                Assert.IsTrue(p.Y == 0 || p.Y == audioViewer.WaveFormHeight);
            }
        }
    }
}
