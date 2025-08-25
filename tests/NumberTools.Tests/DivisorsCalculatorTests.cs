using NumberTools;
using Xunit;

namespace NumberTools.Tests;

public class DivisorsCalculatorTests
{
    [Theory]
    [InlineData(1, new long[] { 1 }, new long[] { 1 })]
    [InlineData(2, new long[] { 1, 2 }, new long[] { 1, 2 })]
    [InlineData(3, new long[] { 1, 3 }, new long[] { 1, 3 })]
    [InlineData(4, new long[] { 1, 2, 4 }, new long[] { 1, 2 })]
    [InlineData(12, new long[] { 1, 2, 3, 4, 6, 12 }, new long[] { 1, 2, 3 })]
    [InlineData(45, new long[] { 1, 3, 5, 9, 15, 45 }, new long[] { 1, 3, 5 })]
    [InlineData(49, new long[] { 1, 7, 49 }, new long[] { 1, 7 })]
    [InlineData(97, new long[] { 1, 97 }, new long[] { 1, 97 })]
    public void Divisors_And_Primes(long n, long[] expectedDivs, long[] expectedPrimes)
    {
        var divs = DivisorsCalculator.GetDivisors(n);
        Assert.Equal(expectedDivs, divs);

        var primes = DivisorsCalculator.GetPrimeDivisors(n, includeOneInPrimeDivisors: true);
        Assert.Equal(expectedPrimes, primes);
    }

    [Fact]
    public void Invalid_Input_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => DivisorsCalculator.GetDivisors(0));
        Assert.Throws<ArgumentOutOfRangeException>(() => DivisorsCalculator.GetDivisors(-1));
    }

    [Theory]
    [InlineData(2, true)]
    [InlineData(3, true)]
    [InlineData(4, false)]
    [InlineData(17, true)]
    [InlineData(21, false)]
    [InlineData(1, false)]
    public void IsPrime_Basic(long n, bool expected)
    {
        Assert.Equal(expected, DivisorsCalculator.IsPrime(n));
    }
}
