using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Algostable.PeakDetection.Tests
{
    public class PeakDetectionTests
    {
        private readonly PeakDetection _peakDetection;

        public PeakDetectionTests()
        {
            _peakDetection = new PeakDetection();
        }

        [Fact]
        public void TestPeakDetectionWithPeaks()
        {
            // Sample input data with peaks
            var inputData = new double[] { 0.5, 0.8, 1.2, 0.9, 1.5, 0.7, 1.0 };

            // Threshold for peak detection
            double peakThreshold = 0.6;

            // Expected output (peaks) after peak detection
            var expectedPeaks = new double[] { 1.2, 1.5, 1.0 };

            // Perform the peak detection
            var result = _peakDetection.Convert(inputData, peakThreshold).ToArray();

            // Compare the detected peaks with the expected peaks
            Assert.Equal(expectedPeaks.Length, result.Length);
            for (int i = 0; i < result.Length; i++)
            {
                // Due to floating-point imprecision, we need to use an epsilon value for comparison
                double epsilon = 1e-6;

                Assert.InRange(result[i], expectedPeaks[i] - epsilon, expectedPeaks[i] + epsilon);
            }
        }

        [Fact]
        public void TestPeakDetectionWithoutPeaks()
        {
            // Sample input data without peaks
            var inputData = new double[] { 0.1, 0.2, 0.3, 0.2, 0.1 };

            // Threshold for peak detection
            double peakThreshold = 0.4;

            // Perform the peak detection
            var result = _peakDetection.Convert(inputData, peakThreshold).ToArray();

            // Ensure no peaks are detected
            Assert.Empty(result);
        }

        [Fact]
        public void TestPeakDetectionWithEmptyInput()
        {
            // Empty input data
            var inputData = Enumerable.Empty<double>();

            // Threshold for peak detection
            double peakThreshold = 0.2;

            // Perform the peak detection
            var result = _peakDetection.Convert(inputData, peakThreshold).ToArray();

            // Ensure no peaks are detected from an empty input
            Assert.Empty(result);
        }

        [Fact]
        public void TestPeakDetectionWithSinglePoint()
        {
            // Single point input data
            var inputData = new double[] { 0.5 };

            // Threshold for peak detection
            double peakThreshold = 0.2;

            // Perform the peak detection
            var result = _peakDetection.Convert(inputData, peakThreshold).ToArray();

            // Ensure a single point is not considered a peak
            Assert.Empty(result);
        }
    }
}
