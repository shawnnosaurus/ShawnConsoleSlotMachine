using Microsoft.Extensions.Options;
using Moq;
using SlotMachine.Config;
using SlotMachine.DTO;
using SlotMachine.Services;

namespace SlotMachine.Tests.Services
{
    public class MathServiceTests
    {
        private readonly Mock<IOptions<SlotMachineConfig>> optionsMock = new();
        private readonly SlotMachineConfig mockConfig = new()
        {
            RowCount = 3,
            Bands = new List<List<int>>() {
                    new List<int> { 1, 6, 6, 0, 0, 4, 0, 3, 4, 2, 1, 2, 7, 3, 4, 1, 7, 4, 6, 1 },
                    new List<int> { 0, 5, 6, 5, 4, 4, 7, 4, 4, 3, 6, 1, 4, 6, 0, 4, 5, 7, 6, 5, 2, 2, 5, 6, 2 },
                    new List<int> { 4, 1, 6, 7, 2, 1, 5, 1, 1, 4, 2, 4, 0, 5, 2, 1, 3, 0, 5, 7, 5, 2, 3, 3, 7, 0, 6, 5, 0, 5 },
                    new List<int> { 1, 5, 2, 5, 7, 7, 2, 5, 7, 0, 4, 0, 5, 2, 5, 6, 1, 4, 2, 5, 7, 3, 0, 4, 6 },
                    new List<int> { 6, 7, 1, 2, 3, 0, 2, 1, 1, 3, 3, 1, 5, 3, 0, 5, 0, 5, 3, 7 }
                },
            Paytable = new Paytable()
            {
                OfAKind = new List<int> { 3, 4, 5 },
                Payouts = new List<List<int>>() {
                        new List<int> { 1, 2, 3 },
                        new List<int> { 1, 2, 3 },
                        new List<int> { 1, 2, 5 },
                        new List<int> { 2, 5, 10 },
                        new List<int> { 5, 10, 15 },
                        new List<int> { 5, 10, 15 },
                        new List<int> { 5, 10, 20 },
                        new List<int> { 10, 20, 50 }
                    }
            },
            Symbols = new List<string> { "sym1", "sym2", "sym3", "sym4", "sym5", "sym6", "sym7", "sym8" }
        };

        public static readonly TheoryData<KeyValuePair<List<int>, Winnings>> ExpectedWinnings = new()
        {
            new KeyValuePair<List<int>, Winnings>(new List<int> {
                1, 1, 1, 4, 0,
                6, 4, 6, 0, 7,
                6, 6, 7, 5, 0
            }, new Winnings {
                Total = 11,
                WayWins = new List<WayWin> {
                    new WayWin {
                        SymbolIndex = 1,
                        ScreenIndexes = new List<int>() { 0, 1, 2 },
                        Value = 1
                    },
                    new WayWin {
                        SymbolIndex = 6,
                        ScreenIndexes = new List<int>() { 5, 11, 7 },
                        Value = 5
                    },
                    new WayWin {
                        SymbolIndex = 6,
                        ScreenIndexes = new List<int>() { 10, 11, 7 },
                        Value = 5
                    }
                }
            }),
            new KeyValuePair<List<int>, Winnings>(new List<int> {
                1, 1, 1, 4, 0,
                6, 4, 6, 0, 7,
                6, 6, 6, 5, 0
            }, new Winnings {
                Total = 21,
                WayWins = new List<WayWin> {
                    new WayWin {
                        SymbolIndex = 1,
                        ScreenIndexes = new List<int>() { 0, 1, 2 },
                        Value = 1
                    },
                    new WayWin {
                        SymbolIndex = 6,
                        ScreenIndexes = new List<int>() { 5, 11, 7 },
                        Value = 5
                    },
                    new WayWin {
                        SymbolIndex = 6,
                        ScreenIndexes = new List<int>() { 10, 11, 7 },
                        Value = 5
                    },
                    new WayWin {
                        SymbolIndex = 6,
                        ScreenIndexes = new List<int>() { 5, 11, 12 },
                        Value = 5
                    },
                    new WayWin {
                        SymbolIndex = 6,
                        ScreenIndexes = new List<int>() { 10, 11, 12 },
                        Value = 5
                    }
                }
            })
        };

        [Theory]
        [MemberData(nameof(ExpectedWinnings))]
        public void Should_Have_Expected_Winnings(KeyValuePair<List<int>, Winnings> ExpectedWinnings)
        {
            // Arange
            optionsMock.Setup(o => o.Value).Returns(mockConfig);
            var sut = new MathService(optionsMock.Object);

            // Act
            var results = sut.CalculateWinnings(ExpectedWinnings.Key);

            // Assert
            Assert.Equal(ExpectedWinnings.Value.WayWins.Count, results.WayWins.Count);
            Assert.Same(ExpectedWinnings.Value.WayWins, results.WayWins);
        }
    }
}
