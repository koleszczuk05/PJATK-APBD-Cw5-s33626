using WebApplication1.Models;

namespace WebApplication1.Data;

public class Database
{
    public static List<Room> Rooms =
    [
        new Room
        {
            Id = 1, Name = "Lab 101", BuildingCode = "A", Floor = 1, Capacity = 30, HasProjector = true, IsActive = true
        },
        new Room
        {
            Id = 2, Name = "Lab 102", BuildingCode = "A", Floor = 1, Capacity = 15, HasProjector = false,
            IsActive = true
        },
        new Room
        {
            Id = 3, Name = "Aula Główna", BuildingCode = "B", Floor = 0, Capacity = 150, HasProjector = true,
            IsActive = true
        },
        new Room
        {
            Id = 4, Name = "Sala 204", BuildingCode = "C", Floor = 2, Capacity = 20, HasProjector = true,
            IsActive = false
        }, // Ta sala jest nieaktywna!
        new Room
        {
            Id = 5, Name = "Pokój cichy", BuildingCode = "C", Floor = 3, Capacity = 4, HasProjector = false,
            IsActive = true
        }
    ];

    public static List<Reservation> Reservations =
    [
        new Reservation
        {
            Id = 1, RoomId = 1, OrganizerName = "Jan Kowalski", Topic = "Wprowadzenie do C#",
            Date = new DateOnly(2026, 5, 10), StartTime = new TimeOnly(8, 0), EndTime = new TimeOnly(10, 0),
            Status = "confirmed"
        },
        new Reservation
        {
            Id = 2, RoomId = 1, OrganizerName = "Anna Nowak", Topic = "Konsultacje projektowe",
            Date = new DateOnly(2026, 5, 10), StartTime = new TimeOnly(10, 30), EndTime = new TimeOnly(12, 0),
            Status = "planned"
        },
        new Reservation
        {
            Id = 3, RoomId = 3, OrganizerName = "Piotr Wiśniewski", Topic = "Wykład gościnny",
            Date = new DateOnly(2026, 5, 12), StartTime = new TimeOnly(14, 0), EndTime = new TimeOnly(16, 0),
            Status = "confirmed"
        },
        new Reservation
        {
            Id = 4, RoomId = 5, OrganizerName = "Kasia Lewandowska", Topic = "Praca w ciszy",
            Date = new DateOnly(2026, 5, 12), StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(15, 0),
            Status = "planned"
        },
        new Reservation
        {
            Id = 5, RoomId = 2, OrganizerName = "Marek Dąbrowski", Topic = "Warsztaty REST API",
            Date = new DateOnly(2026, 5, 15), StartTime = new TimeOnly(16, 0), EndTime = new TimeOnly(18, 30),
            Status = "cancelled"
        }
    ];
}