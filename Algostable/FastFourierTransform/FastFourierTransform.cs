using System.Numerics;

namespace Algostable.FastFourierTransform;

public class FastFourierTransform : IFastFourierTransform
{
    public IEnumerable<Complex> Convert(IEnumerable<Complex> input)
    {
        var inputArray = input.ToArray();
        if (!inputArray.Any())
            return inputArray;
        var n = inputArray.Length;

        // Base case: If the input size is 1, return the input itself
        if (n == 1)
            return new[] { inputArray[0] };

        // Split the input into even and odd parts
        var even = new Complex[n / 2];
        var odd = new Complex[n / 2];
        for (int i = 0; i < n / 2; i++)
        {
            even[i] = inputArray[2 * i];
            odd[i] = inputArray[2 * i + 1];
        }

        // Recursive calls to compute the FFT of even and odd parts
        var fftEven = Convert(even).ToArray();
        var fftOdd = Convert(odd).ToArray();

        // Combine the results
        var fftResult = new Complex[n];
        for (var k = 0; k < n / 2; k++)
        {
            var t = Complex.FromPolarCoordinates(1, -2 * Math.PI * k / n) * fftOdd[k];
            fftResult[k] = fftEven[k] + t;
            fftResult[k + n / 2] = fftEven[k] - t;
        }

        return fftResult;
    }
}