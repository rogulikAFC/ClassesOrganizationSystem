import { useEffect, useState } from "react";
import { Link } from "react-router";
import useOAuth from "./hooks/oAuthHook";
import LessonsSchedule from "./types/LessonsSchedule";
import ListOfLessons from "./types/ListOfLessons";
import { Table } from "react-bootstrap";

const LessonsScheduleForDayForTeacher = ({
  teacherId,
  date,
}: LessonsScheduleForDayForTeacherProps) => {
  const [listOfLessons, setLessonsSchedule] = useState<ListOfLessons>();
  const { getAccessToken } = useOAuth();

  useEffect(() => {
    getLessonsSchedule: (async () => {
      const response = await fetch(
        `https://localhost:7290/api/LessonsSchedules/for_teacher/${teacherId}/for_day?date=${date}`,
        {
          method: "GET",
          headers: new Headers({
            Authorization: "Bearer " + getAccessToken(),
          }),
        }
      );

      const lessonsSchedule = (await response.json()) as ListOfLessons;

      setLessonsSchedule(lessonsSchedule);
    })();
  }, [teacherId, date]);

  return (
    <Table striped>
      <thead>
        <tr>
          <th>№</th>
          <th>Предмет</th>
          <th>Преподаватель</th>
          <th>Класс</th>
        </tr>
      </thead>

      <tbody>
        {listOfLessons
          ? listOfLessons.lessons.map((lesson) => (
              <tr key={lesson.id}>
                <td>{lesson.serialNumber}</td>
                <td>{lesson.subject.title}</td>
                <td>{lesson.room.number}</td>
                <td>{lesson.studentsClass.title}</td>
              </tr>
            ))
          : "Загрузка..."}
      </tbody>
    </Table>
  );
};

type LessonsScheduleForDayForTeacherProps = {
  teacherId: number;
  date: string;
};

export default LessonsScheduleForDayForTeacher;
