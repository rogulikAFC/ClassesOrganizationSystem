import Room from "./Room";
import StudentsClassAnnotation from "./StudentsClassAnnotation";
import Subject from "./Subject";
import UserAnnotation from "./UserAnnotation";

type Lesson = {
  id: number;
  serialNumber: number;
  teacher: UserAnnotation;
  room: Room;
  subject: Subject;
  studentsClass: StudentsClassAnnotation;
};

export default Lesson;
