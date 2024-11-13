using ClassesOrganizationSystem.Application.UnitOfWork.Repositories;
using ClassesOrganizationSystem.Domain.Entities;
using ClassesOrganizationSystem.Domain.Entities.RoomEntities;
using Microsoft.EntityFrameworkCore;

namespace ClassesOrganizationSystem.Infrastructure.Persistence.UnitOfWork.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly ClassesOrganizationSystemDbContext _context;

        public RoomRepository(ClassesOrganizationSystemDbContext context)
        {
            _context = context;
        }

        public void AddEquipmentToRoom(Room room, Equipment equipment)
        {
            var roomToEquipment = new RoomToEquipment()
            {
                Room = room,
                Equipment = equipment
            };

            _context.Add(roomToEquipment);
        }

        public void AddRoom(Room room)
        {
            _context.Add(room);
        }

        public void CreateEquipment(string title)
        {
            var equipment = new Equipment()
            {
                Title = title
            };

            _context.Add(equipment);
        }

        public void DeleteEquipment(Equipment equipment)
        {
            _context.Remove(equipment);
        }

        public async Task<Room?> GetRoomByIdAsync(int id)
        {
            return await _context.Rooms
                .FirstOrDefaultAsync(room => room.Id == id);
        }

        public async Task<IEnumerable<Equipment>> ListEquipments(
            string? query, int pageNum = 1, int pageSize = 10)
        {
            return await _context.Equipments

                .Where(equipment => 
                    query == null || equipment.Title
                        .Contains(query, StringComparison.CurrentCultureIgnoreCase))

                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

        }

        public async Task<IEnumerable<Room>> ListSchoolRooms(
            School school, bool? isOpened, string? query,
            int pageNum = 1, int pageSize = 10)
        {
            return await _context.Rooms

                .Where(room =>
                    room.School == school
                    &&
                    isOpened == null || room.Status.IsOpened == isOpened
                    &&
                    query == null || room.Number
                        .Contains(query!, StringComparison.CurrentCultureIgnoreCase))

                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task RemoveEquipmentFromRoomAsync(
            Room room, Equipment equipment)
        {
            var roomToEquipment = await _context.RoomsToEquipments
                .FirstOrDefaultAsync(roomToEquipment => 
                    roomToEquipment.Room == room && roomToEquipment.Equipment == equipment);

            if (roomToEquipment != null)
            {
                _context.Remove(roomToEquipment);   
            }
        }

        public void RemoveRoom(Room room)
        {
            _context.Remove(room);
        }

        public void SetRoomStatusForRoom(
            Room room, RoomStatus roomStatus)
        {
            _context.Remove(room.Status);

            room.Status = roomStatus;
        }
    }
}
