using ElevatorSystem.Models;
using static ElevatorSystem.Models.Enums;

namespace ElevatorSystem;

public class Organizer
{
    private readonly List<Elevator> _elevators;

    public Organizer(List<Elevator> elevators)
    {
        _elevators = elevators;
    }

    public void HandleRequest(ElevatorRequest request)
    {
        var idleElevators = _elevators
            .Where(e => e.Direction == Direction.Idle)
            .OrderBy(e => Math.Abs(e.CurrentFloor - request.RequestedFloor))
            .ToList();

        var chosen = idleElevators.FirstOrDefault()
            ?? _elevators.OrderBy(e => Math.Abs(e.CurrentFloor - request.RequestedFloor)).First();

        Console.WriteLine($"[Organizer] Assigning elevator {chosen.Id} to floor {request.RequestedFloor}");
        chosen.AddDestination(request.RequestedFloor);
    }

    public IEnumerable<string> GetElevatorStatuses()
    {
        return _elevators.Select(e => e.GetStatus());
    }
}