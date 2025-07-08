using ElevatorSystem.Models;
using static ElevatorSystem.Models.Enums;

namespace ElevatorSystem.Tests
{
    public class ElevatorTests
    {
        [Fact]
        public void AddDestination_ShouldAddValidFloor()
        {
            var elevator = new Elevator(1);
            elevator.AddDestination(5);

            Assert.Equal(5, elevator.GetNextTestDestination());
        }

        [Fact]
        public void AddDestination_ShouldIgnoreInvalidFloor()
        {
            var elevator = new Elevator(1);
            elevator.AddDestination(11);
            elevator.AddDestination(0);

            var destinations = elevator.GetTestDestinations();
            Assert.Empty(destinations);
        }

        [Fact]
        public void SetDirection_ShouldUpdateCorrectly()
        {
            var elevator = new Elevator(1);
            elevator.AddDestination(8);

            elevator.InvokeTestSetDirection();

            Assert.Equal(Direction.Up, elevator.Direction);
        }

        [Fact]
        public async Task Elevator_ShouldReachDestination()
        {
            var elevator = new Elevator(1);
            var tokenSource = new CancellationTokenSource();

            elevator.AddDestination(2);

            _ = elevator.RunAsync(tokenSource.Token);

            await Task.Delay(25000);
            tokenSource.Cancel();

            Assert.Equal(2, elevator.CurrentFloor);
            Assert.Equal(ElevatorState.Idle, elevator.State);
        }

        [Fact]
        public void AddMultipleDestinations_ShouldSortProperly()
        {
            var elevator = new Elevator(1);
            elevator.AddDestination(5);
            elevator.AddDestination(3);

            var destinations = elevator.GetTestDestinations().ToList();

            Assert.Equal(new List<int> { 3, 5 }, destinations);
        }

        [Fact]
        public async Task IdleElevator_ShouldNotMove()
        {
            var elevator = new Elevator(1);
            var tokenSource = new CancellationTokenSource();

            _ = elevator.RunAsync(tokenSource.Token);
            await Task.Delay(3000);

            tokenSource.Cancel();
            Assert.Equal(ElevatorState.Idle, elevator.State);
            Assert.Equal(1, elevator.CurrentFloor);
        }

        [Fact]
        public void AddDestination_AboveMaxFloor_ShouldNotBeAdded()
        {
            var elevator = new Elevator(1);
            elevator.AddDestination(11);

            Assert.Empty(elevator.GetTestDestinations());
        }

        [Fact]
        public void Elevator_ShouldNotAcceptFloorOutsideRange()
        {
            var elevator = new Elevator(1);
            elevator.AddDestination(-1);
            elevator.AddDestination(0);
            elevator.AddDestination(11);
            elevator.AddDestination(999);

            Assert.Empty(elevator.GetTestDestinations());
        }
    }
}