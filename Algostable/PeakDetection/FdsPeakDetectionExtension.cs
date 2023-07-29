using Algostable.Common;

namespace Algostable.PeakDetection;

public static class FdsPeakDetectionExtension
{
    public static IEnumerable<double> PeakDetection(this FrequencyDomainSignal frequencyDomainSignal, double threshold = 0.5)
    {
        var peakDetection = new PeakDetection();
        return peakDetection.Convert(frequencyDomainSignal.Points.Select(x => x.Magnitude), threshold);
    }
}