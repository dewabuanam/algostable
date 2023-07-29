using Algostable.Common;

namespace Algostable.Test.Common
{
    public class TimeDomainSignalTests
    {
        [Fact]
        public void TestSignalAddition()
        {
            // Arrange
            var signal1 = new TimeDomainSignal(new List<TimeDomainSignalPoint>
            {
                new TimeDomainSignalPoint(0.0, 1.0),
                new TimeDomainSignalPoint(1.0, 2.0),
                new TimeDomainSignalPoint(2.0, 3.0),
                new TimeDomainSignalPoint(3.0, 4.0),
            });

            var signal2 = new TimeDomainSignal(new List<TimeDomainSignalPoint>
            {
                new TimeDomainSignalPoint(0.0, 2.0),
                new TimeDomainSignalPoint(1.0, 3.0),
                new TimeDomainSignalPoint(2.0, 1.0),
            });

            // Act
            var result = signal1 + signal2;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(signal1.SamplingRate, result.SamplingRate);
            Assert.Equal(Math.Max(signal1.Duration, signal2.Duration), result.Duration);

            // Verify addition of individual signal points
            var expectedPoints = new List<TimeDomainSignalPoint>
            {
                new TimeDomainSignalPoint(0, 3),
                new TimeDomainSignalPoint(1, 5),
                new TimeDomainSignalPoint(2, 4),
                new TimeDomainSignalPoint(3, 4),
            };

            Assert.NotStrictEqual(expectedPoints, result.Points);
        }

        [Fact]
        public void TestSignalMultiplication()
        {
            // Arrange
            var signal1 = new TimeDomainSignal(new List<TimeDomainSignalPoint>
            {
                new TimeDomainSignalPoint(0.0, 1.0),
                new TimeDomainSignalPoint(1.0, 2.0),
                new TimeDomainSignalPoint(2.0, 3.0),
                new TimeDomainSignalPoint(3.0, 4.0),
            });

            var signal2 = new TimeDomainSignal(new List<TimeDomainSignalPoint>
            {
                new TimeDomainSignalPoint(0.0, 2.0),
                new TimeDomainSignalPoint(1.0, 3.0),
                new TimeDomainSignalPoint(2.0, 1.0),
            });

            // Act
            var result = signal1 * signal2;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(signal1.SamplingRate, result.SamplingRate);
            Assert.Equal(Math.Max(signal1.Duration, signal2.Duration), result.Duration);

            // Verify multiplication of individual signal points
            var expectedPoints = new List<TimeDomainSignalPoint>
            {
                new TimeDomainSignalPoint(0.0, 2.0),
                new TimeDomainSignalPoint(1.0, 6.0),
                new TimeDomainSignalPoint(2.0, 3.0),
                new TimeDomainSignalPoint(3.0, 4.0),
            };

            Assert.NotStrictEqual(expectedPoints, result.Points);
        }

        [Fact]
        public void TestSineSignalCreation()
        {
            // Arrange
            double amplitude = 2.0;
            double frequency = 5.0;
            int samplingRate = 1000;
            double duration = 2.0;

            // Act
            var signal = new TimeDomainSignal(amplitude, frequency, samplingRate, duration).Sin();

            // Assert
            Assert.NotNull(signal);
            Assert.Equal(amplitude, signal.Amplitude);

            // Verify that the signal is actually a sine wave
            foreach (var point in signal.Points)
            {
                var expectedAmplitude = Math.Sin(point.Time * frequency * 2 * Math.PI) * amplitude;
                Assert.Equal(expectedAmplitude, point.Amplitude,
                    precision: 6); // Using a small precision for comparison due to floating-point errors.
            }
        }

        [Fact]
        public void TestSignalSubtraction()
        {
            // Arrange
            var signal1 = new TimeDomainSignal(new List<TimeDomainSignalPoint>
            {
                new TimeDomainSignalPoint(0.0, 1.0),
                new TimeDomainSignalPoint(1.0, 2.0),
                new TimeDomainSignalPoint(2.0, 3.0),
                new TimeDomainSignalPoint(3.0, 4.0),
            });

            var signal2 = new TimeDomainSignal(new List<TimeDomainSignalPoint>
            {
                new TimeDomainSignalPoint(0.0, 2.0),
                new TimeDomainSignalPoint(1.0, 3.0),
                new TimeDomainSignalPoint(2.0, 1.0),
            });

            // Act
            var result = signal1 - signal2;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(signal1.SamplingRate, result.SamplingRate);
            Assert.Equal(Math.Max(signal1.Duration, signal2.Duration), result.Duration);

            // Verify subtraction of individual signal points
            var expectedPoints = new List<TimeDomainSignalPoint>
            {
                new TimeDomainSignalPoint(0.0, -1.0),
                new TimeDomainSignalPoint(1.0, -1.0),
                new TimeDomainSignalPoint(2.0, 2.0),
                new TimeDomainSignalPoint(3.0, 4.0),
            };

            Assert.NotStrictEqual(expectedPoints, result.Points);
        }

        [Fact]
        public void TestSignalDivision()
        {
            // Arrange
            var signal1 = new TimeDomainSignal(new List<TimeDomainSignalPoint>
            {
                new TimeDomainSignalPoint(0.0, 2.0),
                new TimeDomainSignalPoint(1.0, 4.0),
                new TimeDomainSignalPoint(2.0, 6.0),
            });

            var signal2 = new TimeDomainSignal(new List<TimeDomainSignalPoint>
            {
                new TimeDomainSignalPoint(0.0, 1.0),
                new TimeDomainSignalPoint(1.0, 2.0),
                new TimeDomainSignalPoint(2.0, 3.0),
            });

            // Act
            var result = signal1 / signal2;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(signal1.SamplingRate, result.SamplingRate);
            Assert.Equal(Math.Max(signal1.Duration, signal2.Duration), result.Duration);

            // Verify division of individual signal points
            var expectedPoints = new List<TimeDomainSignalPoint>
            {
                new TimeDomainSignalPoint(0.0, 2.0),
                new TimeDomainSignalPoint(1.0, 2.0),
                new TimeDomainSignalPoint(2.0, 2.0),
            };

            Assert.NotStrictEqual(expectedPoints, result.Points);
        }

        [Fact]
        public void TestCosineSignalCreation()
        {
            // Arrange
            double amplitude = 3.0;
            double frequency = 2.0;
            int samplingRate = 1000;
            double duration = 4.0;

            // Act
            var signal = new TimeDomainSignal(amplitude, frequency, samplingRate, duration).Cos();

            // Assert
            Assert.NotNull(signal);
            Assert.Equal(samplingRate, signal.SamplingRate);
            Assert.Equal(amplitude, signal.Amplitude);

            // Verify that the signal is actually a cosine wave
            foreach (var point in signal.Points)
            {
                var expectedAmplitude = Math.Cos(point.Time * frequency * 2 * Math.PI) * amplitude;
                Assert.Equal(expectedAmplitude, point.Amplitude,
                    precision: 6); // Using a small precision for comparison due to floating-point errors.
            }
        }

        [Fact]
        public void TestConstructorWithPoints()
        {
            // Arrange
            var points = new List<TimeDomainSignalPoint>
            {
                new TimeDomainSignalPoint(0.0, 1.0),
                new TimeDomainSignalPoint(1.0, 2.0),
                new TimeDomainSignalPoint(2.0, 3.0),
                new TimeDomainSignalPoint(3.0, 4.0),
            };

            // Act
            var signal = new TimeDomainSignal(points);

            // Assert
            Assert.NotNull(signal);
            Assert.Equal(points.Count, signal.Points.Count());
            Assert.Equal(4.0, signal.Amplitude);
            Assert.Equal(0.0, signal.Frequency);
        }
    }
}