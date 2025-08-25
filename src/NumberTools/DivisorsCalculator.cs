// Copyright (c) 2025
// MIT License

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace NumberTools;

/// <summary>
/// Utilitário para cálculo de divisores e divisores primos.
/// Sem uso de bibliotecas externas de fatoração.
/// </summary>
public static class DivisorsCalculator
{
    /// <summary>
    /// Calcula todos os divisores de n (n >= 1), ordenados ascendente.
    /// </summary>
    public static IReadOnlyList<long> GetDivisors(long n)
    {
        if (n < 1) throw new ArgumentOutOfRangeException(nameof(n), "n deve ser >= 1");

        var list = new List<long>(capacity: 64);
        long i = 1;
        for (; i * i < n; i++)
        {
            if (n % i == 0)
            {
                list.Add(i);
                long other = n / i;
                if (other != i) list.Add(other);
            }
        }
        if (i * i == n) list.Add(i); // quadrado perfeito

        list.Sort();
        return list;
    }

    /// <summary>
    /// Calcula os divisores primos de n.
    /// Por padrão inclui 1 (para obedecer o enunciado).
    /// </summary>
    public static IReadOnlyList<long> GetPrimeDivisors(long n, bool includeOneInPrimeDivisors = true)
    {
        var divisors = GetDivisors(n);
        var result = new List<long>();
        foreach (var d in divisors)
        {
            if (d == 1)
            {
                if (includeOneInPrimeDivisors) result.Add(1);
                continue;
            }
            if (IsPrime(d)) result.Add(d);
        }
        return result;
    }

    /// <summary>
    /// Teste de primalidade rápido (6k ± 1). O(√n).
    /// </summary>
    public static bool IsPrime(long n)
    {
        if (n < 2) return false;
        if (n % 2 == 0) return n == 2;
        if (n % 3 == 0) return n == 3;

        // Checa apenas 6k ± 1
        long i = 5;
        while (i * i <= n)
        {
            if (n % i == 0 || n % (i + 2) == 0) return false;
            i += 6;
        }
        return true;
    }
}
