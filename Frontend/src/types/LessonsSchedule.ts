import Lesson from "./Lesson";
import StudentsClass from "./StudentsClass";

type LessonsSchedule = {
  id: number;
  studentsClass: StudentsClass;
  dayOfWeek: number;
  date: Date;
  lessons: Lesson[];
};

export default LessonsSchedule;
