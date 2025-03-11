import { useEffect, useState } from "react";
import ListOfLessons from "./types/ListOfLessons";
import useOAuth from "./hooks/oAuthHook";
import { Table } from "react-bootstrap";

const LessonsScheduleForDayForRoom = ({
  roomId,
  date,
}: LessonsScheduleForDayForRoomProps) => {
  const [listOfLessons, setListOfLessons] = useState<ListOfLessons>();
  const { getAccessToken } = useOAuth();

  useEffect(() => {
    getLessonsSchedule: (async () => {
      const response = await fetch(
        `https://localhost:7290/api/LessonsSchedules/for_room/${roomId}/for_day?date=${date}`,
        {
          method: "GET",
          headers: new Headers({
            Authorization: "Bearer " + getAccessToken(),
          }),
        }
      );

      const lessonsSchedule = (await response.json()) as ListOfLessons;

      setListOfLessons(lessonsSchedule);
    })();
  }, [roomId, date]);

  return (
    <Table striped>
      <thead>
        <tr>
          <th>№</th>
          <th>Предмет</th>
          <th>Учитель</th>
          <th>Класс</th>
        </tr>
      </thead>

      <tbody>
        {listOfLessons
          ? listOfLessons.lessons.map((lesson) => (
              <tr key={lesson.id}>
                <td>{lesson.serialNumber}</td>
                <td>{lesson.subject.title}</td>
                <td>{lesson.teacher.fullName}</td>
                <td>{lesson.studentsClass.title}</td>
              </tr>
            ))
          : "Загрузка..."}
      </tbody>
    </Table>
  );
};

type LessonsScheduleForDayForRoomProps = {
  roomId: number;
  date: string;
};

export default LessonsScheduleForDayForRoom;
