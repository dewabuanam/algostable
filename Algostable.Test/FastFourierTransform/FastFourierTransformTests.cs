using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Xunit;

namespace Algostable.FastFourierTransform.Tests
{
    public class FastFourierTransformTests
    {
        private readonly FastFourierTransform _fft;

        public FastFourierTransformTests()
        {
            _fft = new FastFourierTransform();
        }

        // Helper method to compare two complex arrays with epsilon tolerance
        private bool AreComplexArraysEqual(Complex[] arr1, Complex[] arr2, double epsilon)
        {
            if (arr1.Length != arr2.Length)
                return false;

            for (int i = 0; i < arr1.Length; i++)
            {
                if (Complex.Abs(arr1[i] - arr2[i]) > epsilon)
                    return false;
            }

            return true;
        }

        [Fact]
        public void TestFastFourierTransform()
        {
            // Sample input data
            var inputData = new Complex[]
            {
                new Complex(1, 0),
                new Complex(2, 0),
                new Complex(3, 0),
                new Complex(4, 0)
            };

            // Expected output after FFT
            var expectedOutput = new Complex[]
            {
                new Complex(10, 0),
                new Complex(-2, 2),
                new Complex(-2, 0),
                new Complex(-2, -2)
            };

            // Perform the FFT
            var result = _fft.Convert(inputData).ToArray();

            // Compare the result with the expected output
            double epsilon = 1e-6;
            Assert.True(AreComplexArraysEqual(result, expectedOutput, epsilon));
        }

        [Fact]
        public void TestFastFourierTransformWithSinglePoint()
        {
            // Single point input data
            var inputData = new Complex[] { new Complex(1, 0) };

            // The FFT of a single point should be the point itself
            var result = _fft.Convert(inputData).ToArray();

            Assert.Equal(inputData.Length, result.Length);
            Assert.Equal(inputData[0], result[0]);
        }

        [Fact]
        public void TestFastFourierTransformWithEmptyInput()
        {
            // Empty input data
            var inputData = Enumerable.Empty<Complex>();

            // The FFT of an empty input should be an empty output
            var result = _fft.Convert(inputData).ToArray();

            Assert.Empty(result);
        }

        [Fact]
        public void TestFastFourierTransformWithPowerOfTwoInput()
        {
            // Input data with a length that is a power of two
            var inputData = new Complex[]
            {
                new Complex(1, 0),
                new Complex(2, 0),
                new Complex(3, 0),
                new Complex(4, 0),
                new Complex(5, 0),
                new Complex(6, 0),
                new Complex(7, 0),
                new Complex(8, 0)
            };

            // Perform the FFT
            var result = _fft.Convert(inputData).ToArray();

            // Check that the result has the same length as the input
            Assert.Equal(inputData.Length, result.Length);
        }
    }
}
