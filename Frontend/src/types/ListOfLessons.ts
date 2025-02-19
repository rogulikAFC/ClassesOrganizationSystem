import Lesson from "./Lesson";

type ListOfLessons = {
  date: string;
  dayOfWeek: string;
  lessons: Lesson[];
};

export default ListOfLessons;