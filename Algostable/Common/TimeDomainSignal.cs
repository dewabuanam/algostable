namespace Algostable.Common;

/// <summary>
/// Object signal(x,y) : x=time(s) and y=amplitude
/// </summary>
public class TimeDomainSignal
{
    public IEnumerable<TimeDomainSignalPoint> Points { get; private set; }
    public double Amplitude { get; }
    public double Frequency { get; }
    public int SamplingRate { get; }
    public double Duration { get; }

    public TimeDomainSignal(IEnumerable<TimeDomainSignalPoint> points)
    {
        var signalPoints = points.ToList();
        Points = signalPoints;
        Amplitude = signalPoints.Max(x => x.Amplitude);
        Frequency = 0;
        Duration = signalPoints.Max(x => x.Time);
        SamplingRate = Convert.ToInt32(signalPoints.Count() / Duration);
    }

    /// <summary>
    /// Signal by calculate amplitude, frequency, sampling rate and duration
    /// </summary>
    public TimeDomainSignal(double amplitude, double frequency, int samplingRate, double duration)
    {
        Amplitude = amplitude;
        Frequency = frequency;
        SamplingRate = samplingRate;
        Duration = duration;

        var points = new List<TimeDomainSignalPoint>();

        var samples = duration * samplingRate;

        for (var i = 0; i <= samples; i++)
        {
            var time = i * 1.0 / samplingRate;

            points.Add(new TimeDomainSignalPoint(time, 0));
        }

        Points = points;
    }

    /// <summary>
    /// Create sines signal
    /// </summary>
    /// <returns>Sines signal</returns>
    public TimeDomainSignal Sin()
    {
        var points = new List<TimeDomainSignalPoint>();
        var dt = 1.0 / SamplingRate;

        for (double t = 0; t < Duration; t += dt)
        {
            var amp = Math.Sin(t * Frequency * 2 * Math.PI) * Amplitude;

            points.Add(new TimeDomainSignalPoint(t, amp));
        }

        return new TimeDomainSignal(points);
    }

    /// <summary>
    /// Create cosines signal
    /// </summary>
    /// <returns>Cosines signal</returns>
    public TimeDomainSignal Cos()
    {
        var points = new List<TimeDomainSignalPoint>();
        var dt = 1.0 / SamplingRate;

        for (double t = 0; t < Duration; t += dt)
        {
            var amp = Math.Cos(t * Frequency * 2 * Math.PI) * Amplitude;

            points.Add(new TimeDomainSignalPoint(t, amp));
        }

        return new TimeDomainSignal(points);
    }

    public static TimeDomainSignal operator -(TimeDomainSignal timeDomainSignal)
    {
        return new TimeDomainSignal(
            timeDomainSignal.Points.Select(x => new TimeDomainSignalPoint(x.Time, -x.Amplitude)));
    }

    public static TimeDomainSignal operator +(TimeDomainSignal signal1, TimeDomainSignal signal2)
    {
        var processedSignalPoints = new List<TimeDomainSignalPoint>();

        var intersectTime = signal1.Points.Select(x => x.Time).Intersect(signal2.Points.Select(x => x.Time)).ToList();
        var signal1TimeOnly = signal1.Points.Select(x => x.Time).Except(intersectTime).ToList();
        var signal2TimeOnly = signal2.Points.Select(x => x.Time).Except(intersectTime).ToList();

        var signal1Points = new List<TimeDomainSignalPoint>(signal1.Points);
        foreach (var point in signal1TimeOnly
                     .Select(time => signal1Points.FirstOrDefault(x => Math.Abs(time - x.Time) == 0))
                     .Where(point => point != null))
        {
            if (point == null) continue;
            processedSignalPoints.Add(new TimeDomainSignalPoint(point.Time, point.Amplitude));
            signal1Points.Remove(point);
        }

        var signal2Points = new List<TimeDomainSignalPoint>(signal2.Points);
        foreach (var point in signal2TimeOnly
                     .Select(time => signal2Points.FirstOrDefault(x => Math.Abs(time - x.Time) == 0))
                     .Where(point => point != null))
        {
            if (point == null) continue;
            processedSignalPoints.Add(new TimeDomainSignalPoint(point.Time, point.Amplitude));
            signal2Points.Remove(point);
        }

        processedSignalPoints.AddRange(from time in intersectTime
            let signal1Data = signal1Points.FirstOrDefault(x => Math.Abs(x.Time - time) == 0)
            where signal1Data != null
            let signal2Data = signal2Points.FirstOrDefault(x => Math.Abs(x.Time - time) == 0)
            where signal2Data != null
            select new TimeDomainSignalPoint(time, signal1Data.Amplitude + signal2Data.Amplitude));

        return new TimeDomainSignal(processedSignalPoints.OrderBy(x=>x.Time));
    }

    public static TimeDomainSignal operator -(TimeDomainSignal signal1, TimeDomainSignal signal2)
    {
        var processedSignalPoints = new List<TimeDomainSignalPoint>();

        var intersectTime = signal1.Points.Select(x => x.Time).Intersect(signal2.Points.Select(x => x.Time)).ToList();
        var signal1TimeOnly = signal1.Points.Select(x => x.Time).Except(intersectTime).ToList();
        var signal2TimeOnly = signal2.Points.Select(x => x.Time).Except(intersectTime).ToList();

        var signal1Points = new List<TimeDomainSignalPoint>(signal1.Points);
        foreach (var point in signal1TimeOnly
                     .Select(time => signal1Points.FirstOrDefault(x => Math.Abs(time - x.Time) == 0))
                     .Where(point => point != null))
        {
            if (point == null) continue;
            processedSignalPoints.Add(new TimeDomainSignalPoint(point.Time, point.Amplitude));
            signal1Points.Remove(point);
        }

        var signal2Points = new List<TimeDomainSignalPoint>(signal2.Points);
        foreach (var point in signal2TimeOnly
                     .Select(time => signal2Points.FirstOrDefault(x => Math.Abs(time - x.Time) == 0))
                     .Where(point => point != null))
        {
            if (point == null) continue;
            processedSignalPoints.Add(new TimeDomainSignalPoint(point.Time, -point.Amplitude));
            signal2Points.Remove(point);
        }

        processedSignalPoints.AddRange(from time in intersectTime
            let signal1Data = signal1Points.FirstOrDefault(x => Math.Abs(x.Time - time) == 0)
            where signal1Data != null
            let signal2Data = signal2Points.FirstOrDefault(x => Math.Abs(x.Time - time) == 0)
            where signal2Data != null
            select new TimeDomainSignalPoint(time, signal1Data.Amplitude - signal2Data.Amplitude));

        return new TimeDomainSignal(processedSignalPoints.OrderBy(x=>x.Time));
    }

    public static TimeDomainSignal operator *(TimeDomainSignal signal1, TimeDomainSignal signal2)
    {
        var processedSignalPoints = new List<TimeDomainSignalPoint>();

        var intersectTime = signal1.Points.Select(x => x.Time).Intersect(signal2.Points.Select(x => x.Time)).ToList();
        var signal1TimeOnly = signal1.Points.Select(x => x.Time).Except(intersectTime).ToList();
        var signal2TimeOnly = signal2.Points.Select(x => x.Time).Except(intersectTime).ToList();

        var signal1Points = new List<TimeDomainSignalPoint>(signal1.Points);
        foreach (var point in signal1TimeOnly
                     .Select(time => signal1Points.FirstOrDefault(x => Math.Abs(time - x.Time) == 0))
                     .Where(point => point != null))
        {
            if (point == null) continue;
            processedSignalPoints.Add(new TimeDomainSignalPoint(point.Time, 0));
            signal1Points.Remove(point);
        }

        var signal2Points = new List<TimeDomainSignalPoint>(signal2.Points);
        foreach (var point in signal2TimeOnly
                     .Select(time => signal2Points.FirstOrDefault(x => Math.Abs(time - x.Time) == 0))
                     .Where(point => point != null))
        {
            if (point == null) continue;
            processedSignalPoints.Add(new TimeDomainSignalPoint(point.Time, 0));
            signal2Points.Remove(point);
        }

        processedSignalPoints.AddRange(from time in intersectTime
            let signal1Data = signal1Points.FirstOrDefault(x => Math.Abs(x.Time - time) == 0)
            where signal1Data != null
            let signal2Data = signal2Points.FirstOrDefault(x => Math.Abs(x.Time - time) == 0)
            where signal2Data != null
            select new TimeDomainSignalPoint(time, signal1Data.Amplitude * signal2Data.Amplitude));

        return new TimeDomainSignal(processedSignalPoints.OrderBy(x=>x.Time));
    }

    public static TimeDomainSignal operator /(TimeDomainSignal signal1, TimeDomainSignal signal2)
    {
        var processedSignalPoints = new List<TimeDomainSignalPoint>();

        var intersectTime = signal1.Points.Select(x => x.Time).Intersect(signal2.Points.Select(x => x.Time)).ToList();
        var signal1TimeOnly = signal1.Points.Select(x => x.Time).Except(intersectTime).ToList();
        var signal2TimeOnly = signal2.Points.Select(x => x.Time).Except(intersectTime).ToList();

        var signal1Points = new List<TimeDomainSignalPoint>(signal1.Points);
        foreach (var point in signal1TimeOnly
                     .Select(time => signal1Points.FirstOrDefault(x => Math.Abs(time - x.Time) == 0))
                     .Where(point => point != null))
        {
            if (point == null) continue;
            processedSignalPoints.Add(new TimeDomainSignalPoint(point.Time, 0));
            signal1Points.Remove(point);
        }

        var signal2Points = new List<TimeDomainSignalPoint>(signal2.Points);
        foreach (var point in signal2TimeOnly
                     .Select(time => signal2Points.FirstOrDefault(x => Math.Abs(time - x.Time) == 0))
                     .Where(point => point != null))
        {
            if (point == null) continue;
            processedSignalPoints.Add(new TimeDomainSignalPoint(point.Time, 0));
            signal2Points.Remove(point);
        }

        processedSignalPoints.AddRange(from time in intersectTime
            let signal1Data = signal1Points.FirstOrDefault(x => Math.Abs(x.Time - time) == 0)
            where signal1Data != null
            let signal2Data = signal2Points.FirstOrDefault(x => Math.Abs(x.Time - time) == 0)
            where signal2Data != null
            select new TimeDomainSignalPoint(time, signal1Data.Amplitude / signal2Data.Amplitude));

        return new TimeDomainSignal(processedSignalPoints.OrderBy(x=>x.Time));
    }
}

public class TimeDomainSignalPoint
{
    public TimeDomainSignalPoint(double time, double amplitude)
    {
        Time = time;
        Amplitude = amplitude;
    }

    public double Time { get; init; }
    public double Amplitude { get; init; }
}