import { useEffect, useState } from "react";
import LessonsSchedule from "./types/LessonsSchedule";
import { Table } from "react-bootstrap";
import useOAuth from "./hooks/oAuthHook";
import { Link } from "react-router";

const LessonsScheduleForDayForClass = ({
  studentsClassId,
  date,
}: LessonsScheduleForDayForClassProps) => {
  const [lessonsSchedule, setLessonsSchedule] = useState<LessonsSchedule>();
  const { getAccessToken } = useOAuth();

  useEffect(() => {
    getLessonsSchedule: (async () => {
      const response = await fetch(
        `https://localhost:7290/api/LessonsSchedules/for_class/${studentsClassId}/for_day?date=${date}`,
        {
          method: "GET",
          headers: new Headers({
            Authorization: "Bearer " + getAccessToken(),
          }),
        }
      );

      const lessonsSchedule = (await response.json()) as LessonsSchedule;

      setLessonsSchedule(lessonsSchedule);
    })();
  }, [studentsClassId, date]);

  return (
    <Table striped>
      <thead>
        <tr>
          <th>№</th>
          <th>Предмет</th>
          <th>Преподаватель</th>
          <th>Кабинет</th>
        </tr>
      </thead>

      <tbody>
        {lessonsSchedule?.lessons.map((lesson) => (
          <tr key={lesson.id}>
            <td>{lesson.serialNumber}</td>
            <td>{lesson.subject.title}</td>
            <td><Link to={"#"}>{lesson.teacher.fullName}</Link></td>
            <td>{lesson.room.number}</td>
          </tr>
        ))}
      </tbody>
    </Table>
  );
};

type LessonsScheduleForDayForClassProps = {
  studentsClassId: number;
  date: string;
};

export default LessonsScheduleForDayForClass;
