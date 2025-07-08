using static ElevatorSystem.Models.Enums;

namespace ElevatorSystem.Models;

public class ElevatorRequest
{
    public int RequestedFloor { get; set; }
    public Direction RequestedDirection { get; set; }

    public ElevatorRequest(int requestedFloor, Direction requestedDirection)
    {
        RequestedFloor = requestedFloor;
        RequestedDirection = requestedDirection;
    }
}
