import RoomAnnotation from "./RoomAnnotation";
import StudentsClassAnnotation from "./StudentsClassAnnotation";
import Subject from "./Subject";
import UserAnnotation from "./UserAnnotation";

type Lesson = {
  id: number;
  serialNumber: number;
  teacher: UserAnnotation;
  room: RoomAnnotation;
  subject: Subject;
  studentsClass: StudentsClassAnnotation;
};

export default Lesson;
