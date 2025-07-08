# Elevator Control System Application

A simulation of a basic elevator control system which is implemented in C#. 
The project once started simulates 4 elevators serving requests in a building with 10 floors, focusing on timing and handling. 
As part of the projects, tests are also included for testability of the project.

---

## Features

- Contains 4 elevators for 10 floors.
- Elevators move one floor every 10 seconds.
- Stop on a floor takes 10 seconds.
- The requests are generated randomly
- Thread safe handling of requests and elevator state.

---

##️ Technologies Used

- C# (.NET)
- xUnit for testing

---

## Running the Simulation

1. Open the solution in Visual Studio.
2. Run the Project.
3. Console will output elevator movement and request logs every few seconds.

---

### Unit Tests
- `ElevatorTests` — Adding destinations, direction logic, floor movement
- `ORganizerTests` — Request handling and assignment logic

### Integration Tests
- When simultaneous requests happened, assign them to correct elevators
- When all elevators are busy, requests are still honored
- When boundary floor requests happened,  requests are handled correctly