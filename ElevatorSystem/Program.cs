using ElevatorSystem;
using ElevatorSystem.Models;

var elevators = new List<Elevator>
    {
        new Elevator(1),
        new Elevator(2),
        new Elevator(3),
        new Elevator(4)
    };

var organizer = new Organizer(elevators);

foreach (var elevator in elevators)
{
    _ = elevator.RunAsync(default);
}

var simulation = new SimulationEngine(organizer);
await simulation.StartAsync();