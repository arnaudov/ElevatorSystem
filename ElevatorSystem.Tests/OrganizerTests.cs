using ElevatorSystem.Models;
using static ElevatorSystem.Models.Enums;

namespace ElevatorSystem.Tests;

public class OrganizerTests
{
    [Fact]
    public void Organizer_ShouldAssignClosestElevator()
    {
        var elevators = new List<Elevator>
        {
            new Elevator(1),
            new Elevator(2),
            new Elevator(3),
        };

        elevators[0].AddDestination(3);
        elevators[1].AddDestination(5);
        elevators[2].AddDestination(10);

        var organizer = new Organizer(elevators);
        organizer.HandleRequest(new ElevatorRequest(4, Direction.Up));

        var assignedElevator = elevators.OrderBy(e => Math.Abs(e.CurrentFloor - 4)).First();

        Assert.Contains(4, assignedElevator.GetTestDestinations());
    }

    [Fact]
    public void Organizer_GetElevatorStatuses_ReturnsStatuses()
    {
        var elevators = new List<Elevator>
        {
            new Elevator(1),
            new Elevator(2),
        };

        elevators[0].AddDestination(2);
        elevators[1].AddDestination(5);

        var organizer = new Organizer(elevators);
        var statuses = organizer.GetElevatorStatuses().ToList();

        Assert.Equal(elevators.Count, statuses.Count);
        Assert.All(statuses, status => Assert.False(string.IsNullOrWhiteSpace(status)));
    }

    [Fact]
    public void Organizer_PrioritizesIdleElevator()
    {
        var elevators = new List<Elevator>
        {
            new Elevator(1), 
            new Elevator(2),
        };
        elevators[1].AddDestination(5);

        var organizer = new Organizer(elevators);
        organizer.HandleRequest(new ElevatorRequest(4, Direction.Up));

        Assert.Contains(4, elevators[0].GetTestDestinations());
    }

    [Fact]
    public void Organizer_PicksClosestWhenNoneIdle()
    {
        var elevators = new List<Elevator>
        {
            new Elevator(1),
            new Elevator(2),
            new Elevator(3)
        };

        elevators[0].SetTestCurrentFloor(1);
        elevators[1].SetTestCurrentFloor(9);
        elevators[2].SetTestCurrentFloor(5);

        var organizer = new Organizer(elevators);
        organizer.HandleRequest(new ElevatorRequest(6, Direction.Up));

        Assert.Contains(6, elevators[2].GetTestDestinations());
    }

    [Fact]
    public void Organizer_ShouldNotPickFarIdleElevator_WhenCloserOneIsIdle()
    {
        var elevators = new List<Elevator>
        {
            new Elevator(1),
            new Elevator(2),
        };

        elevators[0].SetTestCurrentFloor(1);
        elevators[1].SetTestCurrentFloor(7);

        var organizer = new Organizer(elevators);
        organizer.HandleRequest(new ElevatorRequest(6, Direction.Up));

        var destinations1 = elevators[0].GetTestDestinations();
        var destinations2 = elevators[1].GetTestDestinations();

        Assert.Empty(destinations1);
        Assert.Contains(6, destinations2);
    }

    [Fact]
    public void Organizer_ShouldNotAssignMultipleElevatorsToSameRequest()
    {
        var elevators = new List<Elevator>
        {
            new Elevator(1),
            new Elevator(2),
            new Elevator(3),
        };

        var organizer = new Organizer(elevators);
        organizer.HandleRequest(new ElevatorRequest(4, Direction.Up));

        int assignedCount = elevators.Count(e => e.GetTestDestinations().Contains(4));
        Assert.Equal(1, assignedCount);
    }

    [Fact]
    public void Organizer_ShouldNotDropRequest_WhenAllElevatorsBusy()
    {
        var elevators = new List<Elevator>
        {
            new Elevator(1),
            new Elevator(2),
        };

        elevators[0].AddDestination(5);
        elevators[1].AddDestination(8);

        var organizer = new Organizer(elevators);
        organizer.HandleRequest(new ElevatorRequest(3, Direction.Down));

        var anyAssigned = elevators.Any(e => e.GetTestDestinations().Contains(3));
        Assert.True(anyAssigned);
    }
}
