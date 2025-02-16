using System.Collections;
using ZawieszkaCore.Map;

namespace ZawieszkaCoreTests.Map;

public class MapMovementTests
{
    [Theory]
    [InlineData(0, 2)]
    [InlineData(1, 2)]
    [InlineData(2, 2)]
    [InlineData(0, 3)]
    public void GoingToStartingPosNotAllowed(int startingPos, int movementPoints)
    {
        var world = WorldFactory.SimpleTestWorld();
        var moves = world.AllowedMoves(startingPos, movementPoints);
        foreach (var tile in moves)
        {
            Assert.NotEqual(startingPos, tile);
        }
    }
    
    [Theory]
    [InlineData(0, 6, 3)]
    [InlineData(3, 6, 0)]
    public void GoingBackNotAllowed(int startingPos, int movementPoints, int destination)
    {
        var world = WorldFactory.LineTestWorld();
        var moves = world.AllowedMoves(startingPos, movementPoints).ToList();
        
        Assert.NotEmpty(moves);
        foreach (var tile in moves)
        {
            Assert.Equal(destination, tile);
        }
        
    }
    
    [Theory]
    [InlineData(0, 1, 1)]
    [InlineData(0, 2, 2)]
    [InlineData(0, 3, 2)]
    [InlineData(0, 4, 3)]
    [InlineData(0, 5, 3)]
    [InlineData(0, 6, 3)]
    public void GoingWhenNotEnoughPointsAllowed(int startingPos, int movementPoints, int destination)
    {
        var world = WorldFactory.LineTestWorld();
        var moves = world.AllowedMoves(startingPos, movementPoints).ToList();
        Assert.NotEmpty(moves);
        foreach (var tile in moves)
        {
            Assert.Equal(destination, tile);
        }
        
    }
    
    [Theory]
    [ClassData(typeof(GoingThroughWorldIsWorkingData))]
    public void GoingThroughWorldIsWorking(int startingPos, int movementPoints, List<int> destinations)
    {
        var world = WorldFactory.BiggerTestWorld();
        var moves = world.AllowedMoves(startingPos, movementPoints).Order().ToList();
        Assert.Equal(destinations.Order().ToList(), moves);
    }
}

public class GoingThroughWorldIsWorkingData : IEnumerable<object[]>
{
    private readonly List<object[]> _data = [
        new object[] {0, 1, new List<int>{1, 5}},
        new object[] {0, 2, new List<int>{1, 4}},
        new object[] {0, 3, new List<int>{2, 3, 6, 8}},
        new object[] {0, 4, new List<int>{3, 2, 7, 8}},
        new object[] {0, 5, new List<int>{4, 1, 7, 8}},
        new object[] {0, 6, new List<int>{1, 5, 7, 8, 6}},
        new object[] {0, 7, new List<int>{1, 5, 7, 8}},
    ];
    public IEnumerator<object[]> GetEnumerator()
    {
        return _data.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}