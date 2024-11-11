using ClassesOrganizationSystem.Domain.Entities;
using ClassesOrganizationSystem.Domain.Entities.RoomEntities;

namespace ClassesOrganizationSystem.Application.UnitOfWork.Repositories
{
    public interface IRoomRepository
    {
        Task<Room?> GetRoomByIdAsync(int id);

        Task<IEnumerable<Room>> ListSchoolRooms(
            School school, bool? isOpened, string? query,
            int pageNum = 1, int pageSize = 10);

        void AddRoom(Room room);

        void RemoveRoom(Room room);

        void SetRoomStatusForRoom(Room room, RoomStatus roomStatus);

        void AddEquipmentToRoom(Room room, Equipment equipment);

        Task RemoveEquipmentFromRoomAsync(Room room, Equipment equipment);

        void CreateEquipment(string title);

        void DeleteEquipment(int id);

        Task<IEnumerable<Equipment>> ListEquipments(
            string? query, int pageNum = 1, int pageSize = 10);
    }
}
