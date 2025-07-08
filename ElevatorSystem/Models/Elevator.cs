using static ElevatorSystem.Models.Enums;

namespace ElevatorSystem.Models;

public class Elevator
{
    public int Id { get; }
    public int CurrentFloor { get; private set; } = 1;
    public Direction Direction { get; private set; } = Direction.Idle;
    public ElevatorState State { get; private set; } = ElevatorState.Idle;

    private SortedSet<int> _destinations = new();
    private readonly object _lock = new();

    private const int MinFloor = 1;
    private const int MaxFloor = 10;

    public Elevator(int id)
    {
        Id = id;
    }

    public void AddDestination(int floor)
    {
        if (floor < MinFloor || floor > MaxFloor)
            return;

        lock (_lock)
        {
            _destinations.Add(floor);
            SetDirection();
        }
    }

    public async Task RunAsync(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            int? next = null;
            lock (_lock)
            {
                if (_destinations.Any())
                {
                    State = ElevatorState.Moving;
                    next = GetNextDestination();
                }
                else
                {
                    State = ElevatorState.Idle;
                    Direction = Direction.Idle;
                }
            }

            if (next.HasValue)
            {
                while (CurrentFloor != next)
                {
                    await Task.Delay(10000, token);
                    if ((Direction == Direction.Up && CurrentFloor < MaxFloor) ||
                        (Direction == Direction.Down && CurrentFloor > MinFloor))
                    {
                        CurrentFloor += Direction == Direction.Up ? 1 : -1;
                        Console.WriteLine($"[Elevator {Id}] Floor: {CurrentFloor}");
                    }
                    else
                    {
                        break;
                    }
                }

                Console.WriteLine($"[Elevator {Id}] arrived at floor {CurrentFloor}. Loading...");
                State = ElevatorState.Loading;

                await Task.Delay(10000, token);

                lock (_lock)
                {
                    _destinations.Remove(CurrentFloor);
                    SetDirection();
                }
            }
            else
            {
                await Task.Delay(1000, token);
            }
        }
    }

    private void SetDirection()
    {
        if (!_destinations.Any())
        {
            Direction = Direction.Idle;
            return;
        }

        Direction = _destinations.Min > CurrentFloor ? Direction.Up : Direction.Down;
    }

    private int GetNextDestination()
    {
        return Direction == Direction.Up ? _destinations.First() : _destinations.Last();
    }

    public string GetStatus()
    {
        return $"Elevator {Id} - Floor: {CurrentFloor}, Direction: {Direction}, State: {State}";
    }
}