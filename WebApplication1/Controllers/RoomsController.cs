using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomsController : ControllerBase
{

    [HttpGet("{id:int}")]
    public IActionResult GetRoom(int id)
    {
        var room = Database.Rooms.FirstOrDefault(x => x.Id == id);
        if (room == null)
        {
            return NotFound($"Nie znaleziono sali o ID: {id}");
        }
        return Ok(room);
    }
    
    [HttpGet("building/{buildingCode}")]
    public IActionResult GetRoomsByBuilding(string buildingCode)
    {
        return Ok(Database.Rooms.Where(room => room.BuildingCode == buildingCode).ToList());
    }

    [HttpGet]
    public IActionResult GetFiltered(int? minCapacity, bool? hasProjector, bool? activeOnly)
    {
        var rooms = Database.Rooms.AsEnumerable();
        if (minCapacity.HasValue)
        {
            rooms = rooms.Where(room => room.Capacity >= minCapacity.Value);
        }

        if (hasProjector.HasValue)
        {
            rooms = rooms.Where(room => room.HasProjector == hasProjector.Value);
        }

        if (activeOnly.HasValue)
        {
            rooms = rooms.Where(room => room.IsActive == activeOnly.Value);
        }
        
        return Ok(rooms.ToList());
    }

    [HttpPost]
    public IActionResult CreateRoom([FromBody] Room room)
    {
        var maxId = Database.Rooms.Max(x => x.Id);
        room.Id = maxId + 1;
        Database.Rooms.Add(room);
        return CreatedAtAction(nameof(GetRoom), new { id = maxId + 1 }, room);
    }

    [HttpPut("{id:int}")]
    public IActionResult UpdateRoom(int id,[FromBody] Room room)
    {
        var targetRoom = Database.Rooms.FirstOrDefault(r => r.Id == id);

        if (targetRoom == null)
        {
            return NotFound($"Nie znaleziono sali o ID: {id}");
        }
        
        targetRoom.Name = room.Name;
        targetRoom.BuildingCode = room.BuildingCode;
        targetRoom.Floor =  room.Floor;
        targetRoom.Capacity = room.Capacity;
        targetRoom.HasProjector = room.HasProjector;
        targetRoom.IsActive = room.IsActive;
        
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public IActionResult DeleteRoom(int id)
    {
        var room = Database.Rooms.FirstOrDefault(x => x.Id == id);
        
        if (room == null)
        {
            return NotFound($"Nie znaleziono sali o ID: {id}");
        }
        
        var hasReservation = Database.Reservations.Any(x => x.RoomId == id);
        if (hasReservation)
        {
            return Conflict($"Nie można usunąć sali, ponieważ ma przypisane rezerwacje.");
        }
        
        Database.Rooms.Remove(room);
        return NoContent();
    }
    
    
    
}