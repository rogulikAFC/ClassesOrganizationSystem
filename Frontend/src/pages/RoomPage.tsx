import { FC, useEffect, useState } from "react";
import { Link, useParams } from "react-router";
import Room from "../types/Room";
import { Button, Placeholder, Stack, Table } from "react-bootstrap";
import LessonsScheduleForDayForRoom from "../LessonsScheduleForDayForRoom";

type RoomPageParams = {
  roomId: string;
};

const RoomPage: FC = () => {
  const { roomId } = useParams<RoomPageParams>();
  const [room, setRoom] = useState<Room>();

  // const date = dayjs().format("YYYY-MM-DD");
  const date = "2025-01-06";

  useEffect(() => {
    getRoom: (async () => {
      const response = await fetch(
        "https://localhost:7290/api/Rooms/" + roomId
      );

      const room = (await response.json()) as Room;

      setRoom(room);
    })();
  }, [roomId]);

  return (
    <Stack gap={3} className="align-items-center">
      <h2></h2>

      <Table>
        <tbody>
          <tr>
            <td>Номер (название)</td>
            <td>{room?.number}</td>
          </tr>

          <tr>
            <td>Школа</td>
            <td>
              <Link to={"/school/" + room?.school.id}>
                {room?.school.title}
              </Link>
            </td>
          </tr>

          <tr>
            <td>Вместимость</td>
            <td>{room?.capacity}</td>
          </tr>

          <tr>
            <td>Статус</td>
            <td>
              <Placeholder bg={room?.status.isOpened ? "success" : "secondary"}>
                {room?.status.description}
              </Placeholder>
            </td>
          </tr>
        </tbody>
      </Table>

      <h2>Расписание на день</h2>
      <LessonsScheduleForDayForRoom roomId={Number.parseInt(roomId!)} date={date} />

      <h2>Оборудование</h2>

      <div className="d-flex flex-row flex-wrap gap-2 justify-content-center">
        {room?.equipments.map((equipment) => (
          <Button variant="info" key={equipment.id}>
            {equipment.title}
          </Button>
        ))}
      </div>

    </Stack>
  );
};

export default RoomPage;
