using System.Numerics;

namespace Algostable.FastFourierTransform;

public interface IFastFourierTransform
{
    public IEnumerable<Complex> Convert(IEnumerable<Complex> input);
}