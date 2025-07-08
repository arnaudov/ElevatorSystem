using ElevatorSystem.Models;
using static ElevatorSystem.Models.Enums;

namespace ElevatorSystem.Tests
{
    public class IntegrationTests
    {
        [Fact]
        public async Task Integration_WhenSimulationRuns_ThenElevatorsShouldMoveTowardRequests()
        {
            var elevators = new List<Elevator>
            {
                new Elevator(1),
                new Elevator(2),
                new Elevator(3),
                new Elevator(4),
            };

            elevators[0].SetTestCurrentFloor(1);
            elevators[1].SetTestCurrentFloor(8);

            var organizer = new Organizer(elevators);
            var engine = new SimulationEngine(organizer);

            var tokenSource = new CancellationTokenSource();
            _ = engine.StartAsync();
            _ = elevators.Select(e => e.RunAsync(tokenSource.Token)).ToList();

            await Task.Delay(35000);

            tokenSource.Cancel();

            Assert.Contains(elevators, e => e.CurrentFloor != 1);
        }

        [Fact]
        public async Task Integration_WhenSimultaneousRequests_ThenDistributeRequestsCorrectly()
        {
            var elevators = new List<Elevator>
            {
                new Elevator(1),
                new Elevator(2),
            };

            elevators[0].SetTestCurrentFloor(2);
            elevators[1].SetTestCurrentFloor(9);

            var organizer = new Organizer(elevators);

            organizer.HandleRequest(new ElevatorRequest(3, Direction.Up));
            organizer.HandleRequest(new ElevatorRequest(10, Direction.Down));

            await Task.Delay(500);

            Assert.Contains(3, elevators[0].GetTestDestinations());
            Assert.Contains(10, elevators[1].GetTestDestinations());
        }

        [Fact]
        public async Task Integration_WhenBoundaryFloorRequests_ThenHandleProperly()
        {
            var elevators = new List<Elevator>
            {
                new Elevator(1),
                new Elevator(2),
            };

            elevators[0].SetTestCurrentFloor(5);
            elevators[1].SetTestCurrentFloor(10);

            var organizer = new Organizer(elevators);
            var tokenSource = new CancellationTokenSource();

            organizer.HandleRequest(new ElevatorRequest(1, Direction.Up));
            organizer.HandleRequest(new ElevatorRequest(10, Direction.Down));

            _ = elevators.Select(e => e.RunAsync(tokenSource.Token)).ToList();

            await Task.Delay(60000);
            tokenSource.Cancel();

            var floors = elevators.Select(e => e.CurrentFloor);
            Assert.Contains(1, floors);
            Assert.Contains(10, floors);
        }
    }
}
