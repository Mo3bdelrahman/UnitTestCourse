using Day1.OrderSystem.Models;

namespace Day1.OrderSystem.Tests.TestData;

/// <summary>
/// Provides [MemberData] rows for discount tests.
/// MemberData allows strongly-typed objects, unlike InlineData which is limited to constants.
/// </summary>
public static class DiscountMemberData
{
    public static IEnumerable<object[]> Cases =>
    [
        // customer,                              total,   expectedDiscount
        [new Customer("Alice", CustomerTier.Regular), 200m,  0m   ],
        [new Customer("Bob",   CustomerTier.Silver),  200m,  20m  ],
        [new Customer("Carol", CustomerTier.Gold),    200m,  30m  ],
        [new Customer("Dave",  CustomerTier.Gold),    100m,  15m  ],
        [new Customer("Eve",   CustomerTier.Silver),  50m,   5m   ],
    ];
}
