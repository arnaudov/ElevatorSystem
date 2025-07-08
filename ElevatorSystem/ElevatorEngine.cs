using ElevatorSystem.Models;
using static ElevatorSystem.Models.Enums;

namespace ElevatorSystem;

public class SimulationEngine
{
    private readonly Organizer _organizer;
    private readonly Random _random = new();
    private readonly CancellationTokenSource _cts = new();

    public SimulationEngine(Organizer organizer)
    {
        _organizer = organizer;
    }

    public async Task StartAsync()
    {
        Console.WriteLine("[Simulation] Starting simulation...");

        _ = Task.Run(() => GenerateRandomRequests(_cts.Token));
        Thread.Sleep(1000);
        _ = Task.Run(() => LogElevatorStates(_cts.Token));

        Console.CancelKeyPress += (s, e) =>
        {
            Console.WriteLine("[Simulation] Stopping...");
            _cts.Cancel();
            e.Cancel = true;
        };

        while (!_cts.IsCancellationRequested)
        {
            await Task.Delay(1000);
        }
    }

    private async Task GenerateRandomRequests(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            int floor = _random.Next(1, 11);
            Direction direction = floor == 10 ? Direction.Down : floor == 1 ? Direction.Up : (_random.Next(2) == 0 ? Direction.Up : Direction.Down);

            Console.WriteLine($"\n[Simulation] Request at floor {floor}, going {direction}");
            _organizer.HandleRequest(new ElevatorRequest(floor, direction));

            await Task.Delay(_random.Next(5000, 15000), token);
        }
    }

    private async Task LogElevatorStates(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            Console.WriteLine("\n[Status Update] Elevator States:");
            foreach (var status in _organizer.GetElevatorStatuses())
            {
                Console.WriteLine(status);
            }
            Console.WriteLine();
            await Task.Delay(10000, token);
        }
    }
}
