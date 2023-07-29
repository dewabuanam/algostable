namespace Algostable.PeakDetection;

public interface IPeakDetection
{
    public IEnumerable<double> Convert(IEnumerable<double> magnitudes, double peakThreshold);
}