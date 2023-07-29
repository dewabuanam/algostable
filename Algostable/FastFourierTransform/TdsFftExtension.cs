using System.Numerics;
using Algostable.Common;

namespace Algostable.FastFourierTransform;

public static class TdsFftExtension
{
    public static FrequencyDomainSignal FastFourierTransform(this TimeDomainSignal timeDomainSignal)
    {
        var n = timeDomainSignal.Points.Count();
        // Convert the time domain signal to Complex numbers
        var input = timeDomainSignal.Points.Select(point => new Complex(point.Amplitude, 0)).ToList();

        var fft = new FastFourierTransform();
        var fftResult = fft.Convert(input.ToArray()).ToArray();

        // Extract the frequency and magnitude arrays from the FFT result
        var freqDomainSigPoints = new List<FrequencyDomainSignalPoint>();

        // Calculate the time interval between each pair of adjacent time points
        var inputTime = timeDomainSignal.Points.Select(x => x.Time).ToArray();
        var timeIntervals = new double[n - 1];
        for (var i = 0; i < n - 1; i++)
        {
            timeIntervals[i] = inputTime[i + 1] - inputTime[i];
        }

        for (var k = 0; k < n / 2; k++)
        {
            // Calculate the frequency value using the time intervals
            var totalTimeInterval = 0.0;
            for (var i = 0; i < n - 1; i++)
            {
                totalTimeInterval += timeIntervals[i];
            }

            var dt = totalTimeInterval / (n - 1);
            var frequency = k / (dt * n); // Normalize frequency
            var magnitude = fftResult[k].Magnitude;

            var freqDomainSigPoint = new FrequencyDomainSignalPoint(frequency, magnitude);
            freqDomainSigPoints.Add(freqDomainSigPoint);
        }

        return new FrequencyDomainSignal(freqDomainSigPoints);
    }
}