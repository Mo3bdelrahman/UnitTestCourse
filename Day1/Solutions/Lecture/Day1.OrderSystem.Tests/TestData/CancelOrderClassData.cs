using Day1.OrderSystem.Models;
using System.Collections;

namespace Day1.OrderSystem.Tests.TestData;

/// <summary>
/// Provides [ClassData] rows for CancelOrder tests.
/// ClassData is an IEnumerable class — useful when test data needs constructor logic.
/// </summary>
public class CancelOrderClassData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        // status,                canCancel
        yield return [OrderStatus.Pending,   true ];
        yield return [OrderStatus.Confirmed, true ];
        yield return [OrderStatus.Shipped,   false];  // ← cannot cancel shipped orders
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
