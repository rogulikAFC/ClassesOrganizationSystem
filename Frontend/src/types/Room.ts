import Equipment from "./Equipment";
import RoomStatus from "./RoomStatus";
import SchoolAnnotation from "./SchoolAnnotation";

type Room = {
  id: number;
  number: string;
  capacity: number;
  school: SchoolAnnotation;
  status: RoomStatus;
  equipments: Equipment[];
};

export default Room;
