namespace Algostable.Common;

public class FrequencyDomainSignal
{
    public FrequencyDomainSignal(IEnumerable<FrequencyDomainSignalPoint> points)
    {
        Points = points;
    }

    public IEnumerable<FrequencyDomainSignalPoint> Points { get; private set; }
}

public class FrequencyDomainSignalPoint
{
    public FrequencyDomainSignalPoint(double frequency, double magnitude)
    {
        Frequency = frequency;
        Magnitude = magnitude;
    }

    public double Frequency { get; init; }
    public double Magnitude { get; init; }
}