using Algostable.PeakDetection;

namespace Algostable.PeakDetection;

public class PeakDetection :IPeakDetection
{
    public IEnumerable<double> Convert(IEnumerable<double> magnitudes, double peakThreshold)
    {
        var peaks = new List<double>();
        var magnitudesArray = magnitudes.ToArray();
        if (magnitudesArray.Length <= 1)
            return peaks;

        // The first and last points cannot be peaks, so start and end at index 1 to n-2.
        for (var i = 0; i < magnitudesArray.Count(); i++)
        {
            var currentMagnitude = magnitudesArray[i];

            if (i == 0)
            {
                if (currentMagnitude > magnitudesArray[i + 1] &&
                    currentMagnitude > peakThreshold)
                {
                    peaks.Add(magnitudesArray[i]);
                }
                continue;
            }
            if (i == magnitudesArray.Length-1)
            {
                if (currentMagnitude > magnitudesArray[i - 1] &&
                    currentMagnitude > peakThreshold)
                {
                    peaks.Add(magnitudesArray[i]);
                }
                continue;
            }

            // Check if the current point is a peak by comparing with neighbors.
            if (currentMagnitude > magnitudesArray[i - 1] &&
                currentMagnitude > magnitudesArray[i + 1] &&
                currentMagnitude > peakThreshold)
            {
                peaks.Add(magnitudesArray[i]);
            }
        }

        return peaks;
    }
}