using ElevatorSystem.Models;
using System.Reflection;

namespace ElevatorSystem.Tests;

public static class TestHelpers
{
    public static int GetNextTestDestination(this Elevator elevator)
    {
        var field = typeof(Elevator).GetField("_destinations", BindingFlags.NonPublic | BindingFlags.Instance);
        var set = (SortedSet<int>)field.GetValue(elevator);
        return set.First();
    }

    public static void InvokeTestSetDirection(this Elevator elevator)
    {
        var method = typeof(Elevator).GetMethod("SetDirection", BindingFlags.NonPublic | BindingFlags.Instance);
        method.Invoke(elevator, null);
    }

    public static IEnumerable<int> GetTestDestinations(this Elevator elevator)
    {
        var field = typeof(Elevator).GetField("_destinations", BindingFlags.NonPublic | BindingFlags.Instance);
        return ((SortedSet<int>)field.GetValue(elevator)).ToList();
    }

    public static void SetTestCurrentFloor(this Elevator elevator, int floor)
    {
        var field = typeof(Elevator).GetField("<CurrentFloor>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance);
        field.SetValue(elevator, floor);
    }
}
