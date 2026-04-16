using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservationsController : ControllerBase
{
    [HttpGet("{id:int}")]
    public IActionResult GetReservation(int id)
    {
        var reservation = Database.Reservations.FirstOrDefault(x => x.Id == id);
        if (reservation == null)
        {
            return NotFound();
        }
        return Ok(reservation);
    }
    
    [HttpGet]
    public IActionResult GetFiltered(DateOnly? date, string? status, int? roomId)
    {
        var reservations = Database.Reservations.AsEnumerable();

        if(date.HasValue)
        {
            reservations = reservations.Where(r => r.Date == date.Value);
        }

        if (!string.IsNullOrEmpty(status))
        {
            reservations = reservations.Where(r => r.Status == status);
        }

        if (roomId.HasValue)
        {
            reservations = reservations.Where(r => r.RoomId == roomId.Value);
        }
        
        return Ok(reservations.ToList());
    }

    [HttpPost]
    public IActionResult CreateReservation([FromBody] Reservation reservation)
    {
        var room = Database.Rooms.FirstOrDefault(x => x.Id == reservation.RoomId);
        if (room == null)
        {
            return BadRequest("Podana sala nie istnieje");
        }

        if (!room.IsActive)
        {
            return BadRequest("Podana sala jest nieaktywna");
        }
        
        var hasConflict = Database.Reservations.Any(x => x.RoomId == reservation.RoomId && x.Date == reservation.Date && reservation.StartTime < x.EndTime && reservation.EndTime > x.StartTime);
        if (hasConflict)
        {
            return Conflict("Sala jest zajęta w wybranym terminie");
        }
        
        var maxId = Database.Reservations.Max(x => x.Id);
        reservation.Id = maxId + 1;
        Database.Reservations.Add(reservation);
        return CreatedAtAction(nameof(GetReservation), new { id = maxId + 1 }, reservation);
    }

    [HttpPut("{id:int}")]
    public IActionResult UpdateReservation(int id, [FromBody] Reservation reservation)
    {
        var targetReservation = Database.Reservations.FirstOrDefault(r => r.Id == id);

        if (targetReservation == null)
        {
            return NotFound($"Nie znaleziono sali o ID: {id}");
        }
        
        var targetRoom = Database.Rooms.FirstOrDefault(x => x.Id == reservation.RoomId);
        if (targetRoom == null)
        {
            return BadRequest($"Nie znaleziono danej sali");
        }

        if (!targetRoom.IsActive)
        {
            return BadRequest($"Podana sala jest nieaktywna");
        }
        
        var hasConflict = Database.Reservations.Any(x => x.Id != id && x.RoomId == reservation.RoomId && x.Date == reservation.Date && reservation.StartTime < x.EndTime && reservation.EndTime > x.StartTime);

        if (hasConflict)
        {
            return Conflict("Sala jest zajęta w wybranym terminie");
        }
        
        targetReservation.RoomId = reservation.RoomId;
        targetReservation.OrganizerName = reservation.OrganizerName;
        targetReservation.Topic = reservation.Topic;
        targetReservation.Date = reservation.Date;
        targetReservation.StartTime = reservation.StartTime;
        targetReservation.EndTime = reservation.EndTime;
        targetReservation.Status = reservation.Status;
        
        return NoContent();
    }

    [HttpDelete("{Id:int}")]
    public IActionResult DeleteReservation(int id)
    {
        var reservation = Database.Reservations.FirstOrDefault(x => x.Id == id);
        if (reservation == null)
        {
            return NotFound("Taka rezerwacja nie istnieje :)");
        }
        
        Database.Reservations.Remove(reservation);
        return Ok("Usunięto podaną rezerwacje");
    }
    
}