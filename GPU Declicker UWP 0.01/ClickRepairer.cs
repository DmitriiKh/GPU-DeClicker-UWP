﻿namespace GPU_Declicker_UWP_0._01
{
    public static class ClickRepairer
    {
        /// <summary>
        ///     Replace output sample at position with prediction and
        ///     sets prediction error sample to zero
        /// </summary>
        /// <param name="audioData"></param>
        /// <param name="position"></param>
        /// <param name="lenght"></param>
        public static float Repair(AudioData audioData, int position, int lenght)
        {
            for (var index = position; index < position + lenght; index++)
            {
                audioData.SetPredictionErr(index, 0.001F);
                audioData.SetOutputSample(
                    index,
                    CalcBurgPred(audioData, index)
                );
            }

            for (var index = position + lenght;
                index < position + lenght + 5;
                index++)
                audioData.SetPredictionErr(
                    index,
                    CalcBurgPred(audioData, index) -
                    audioData.GetOutputSample(index));

            var historyLengthSamples =
                audioData.AudioProcessingSettings.HistoryLengthSamples;

            HelperCalculator.CalculateErrorAverageCPU(
                audioData,
                position - historyLengthSamples,
                position + lenght + historyLengthSamples,
                historyLengthSamples);

            return HelperCalculator.CalculateDetectionLevel(audioData, position);
        }

        /// <summary>
        ///     Returns prediction for a sample at position
        /// </summary>
        public static float CalcBurgPred(
            AudioData audioData,
            int position)
        {
            var historyLengthSamples =
                audioData.AudioProcessingSettings.HistoryLengthSamples;

            // use output audio as an input because it already contains
            // fixed samples before sample at position
            var audioShort = new float[historyLengthSamples + 1];
            for (var index = 0; index < historyLengthSamples + 1; index++)
                audioShort[index] = audioData.GetOutputSample(
                    position - historyLengthSamples + index);

            // array for results
            var forwardPredictionsShort =
                new float[historyLengthSamples + 1];

            // we need this array for calling CalculateBurgPredictionThread
            var backwardPredictionsShort =
                new float[historyLengthSamples + 1];

            BurgPredictionCalculator.Calculate(
                audioShort,
                forwardPredictionsShort,
                backwardPredictionsShort,
                historyLengthSamples,
                audioData.AudioProcessingSettings.CoefficientsNumber,
                historyLengthSamples);

            // return prediction for sample at position
            return forwardPredictionsShort[historyLengthSamples];
        }
    }
}