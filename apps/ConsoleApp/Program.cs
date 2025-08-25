using NumberTools;
using System.Globalization;

Console.OutputEncoding = System.Text.Encoding.UTF8;

long n;
if (args.Length > 0)
{
    if (!long.TryParse(args[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out n) || n < 1)
    {
        Console.WriteLine("Entrada inválida. Informe um inteiro >= 1.");
        return;
    }
}
else
{
    Console.Write("Digite um número: ");
    var input = Console.ReadLine();
    if (!long.TryParse(input, NumberStyles.Integer, CultureInfo.InvariantCulture, out n) || n < 1)
    {
        Console.WriteLine("Entrada inválida. Informe um inteiro >= 1.");
        return;
    }
}

var divisores = DivisorsCalculator.GetDivisors(n);
var primos    = DivisorsCalculator.GetPrimeDivisors(n, includeOneInPrimeDivisors: true);

// Impressão no formato do enunciado
Console.WriteLine($"Número de Entrada: {n}");
Console.WriteLine($"Números divisores: {string.Join(' ', divisores)}");
Console.WriteLine($"Divisores Primos: {string.Join(' ', primos)}");
