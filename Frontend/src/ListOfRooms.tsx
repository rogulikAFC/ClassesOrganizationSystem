import { FC, useEffect, useState } from "react";
import { Button } from "react-bootstrap";
import RoomAnnotation from "./types/RoomAnnotation";
import useOAuth from "./hooks/oAuthHook";
import Room from "./types/Room";
import { Link } from "react-router";

const ListOfRooms: FC<ListOfRoomProps> = ({
  schoolId,
  isOpened,
  pageSize = 2
}) => {
  const [limitReached, setLimitReached] = useState<boolean>(false);
  const [pageNum, setPageNum] = useState<number>(1);
  const [rooms, setRooms] = useState<Room[]>([]);
  const { getAccessToken } = useOAuth();

  useEffect(() => {
    loadRooms: (async () => {
      if (limitReached) {
        return;
      }

      const response = await fetch(
        `https://localhost:7290/api/Rooms/in_school/${schoolId}?pageSize=${pageSize}&pageNum=${pageNum}${
          isOpened == null ? "" : "&isOpened=" + isOpened
        }`,
        {
          method: "GET",
          headers: new Headers({
            Authorization: "Bearer " + getAccessToken(),
          }),
        }
      );

      const newRooms = (await response.json()) as Room[];

      setRooms((prevRooms) => ([...prevRooms, ...newRooms]));

      if (newRooms.length < pageSize) {
        setLimitReached(true);
      }
    })();
  }, [pageNum]);

  return (
    <div className="d-flex flex-column align-items-center gap-4">
      <div className="d-flex flex-row flex-wrap gap-2 justify-content-center">
        {rooms.map((room) => (
          <Link className={`btn btn-${room.status.isOpened ? "success" : "secondary"}`} key={room.id} to={"/room/" + room.id}>
            {room.number}
          </Link>
        ))}
      </div>

      {limitReached || (
        <Button
          variant="primary"
          onClick={() => setPageNum((prevPage) => prevPage + 1)}
        >
          Загрузить еще
        </Button>
      )}
    </div>
  );
};

type ListOfRoomProps = {
  schoolId: string;
  isOpened: boolean | null;
  pageSize: number;
};

export default ListOfRooms;
