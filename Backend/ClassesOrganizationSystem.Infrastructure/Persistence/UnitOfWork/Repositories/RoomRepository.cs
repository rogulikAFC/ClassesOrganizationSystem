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

        public void CreateEquipment(Equipment equipment)
        {
            _context.Add(equipment);
        }

        public void RemoveEquipment(Equipment equipment)
        {
            _context.Remove(equipment);
        }

        public async Task<Room?> GetRoomByIdAsync(int id)
        {
            return await _context.Rooms
                .Include(room => room.School)
                .Include(room => room.Status)
                .Include(room => room.RoomsToEqipments)
                .ThenInclude(roomToEquipment => roomToEquipment.Equipment)
                .FirstOrDefaultAsync(room => room.Id == id);
        }

        public async Task<IEnumerable<Equipment>> ListEquipments(
            string? query, int pageNum = 1, int pageSize = 10)
        {
            return await _context.Equipments

                .Where(equipment => 
                    query == null || equipment.Title.ToLower().Contains(query.ToLower()))

                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

        }

        public async Task<IEnumerable<Room>> ListSchoolRooms(
            School school, bool? isOpened, string? query,
            int pageNum = 1, int pageSize = 10)
        {
            return await _context.Rooms
                .Include(room => room.School)
                .Include(room => room.Status)

                .Where(room =>
                    (room.School == school)
                    &&
                    (isOpened == null || room.Status.IsOpened == isOpened)
                    &&
                    (query == null || room.Number.ToLower().Contains(query.ToLower())))

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
            if (room.Status != null)
            {
                var status = room.Status;

                _context.RoomStatuses.Remove(status);
            }

            room.Status = roomStatus;
        }

        public async Task<Equipment?> GetEquipmentByIdAsync(int id)
        {
            return await _context.Equipments
                .FirstOrDefaultAsync(equipment => equipment.Id == id);
        }

        public void AddRoomStatus(RoomStatus roomStatus)
        {
            _context.Add(roomStatus);
        }
    }
}
