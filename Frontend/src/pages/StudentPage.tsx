import { FC, useEffect, useState } from "react";
import useOAuth from "../hooks/oAuthHook";
import StudentsClass from "../types/StudentsClass";
import dayjs from "dayjs";
import LessonsSchedule from "../types/LessonsSchedule";
import LessonsScheduleForDayForClass from "../LessonsScheduleForDayForClass";
import { Stack } from "react-bootstrap";
import User from "../types/User";

const StudentPage: FC = () => {
  const { getCurrentUser: getCurrentUserOrRedirect } = useOAuth();
  const [user, setUser] = useState<User | undefined>();
  const [classes, setClasses] = useState<StudentsClass[]>([]);

  // const date = dayjs().format("YYYY-MM-DD");
  const date = "2025-01-06";

  useEffect(() => {
    getUser: (async () => {
      setUser(await getCurrentUserOrRedirect());
    })();
  }, []);

  useEffect(() => {
    getClassesOfUser: (async () => {
      if (user == null) return;

      const response = await fetch(
        "https://localhost:7290/api/StudentsClasses/get_classes_of_user/" +
          user?.id
      );

      const classes = (await response.json()) as StudentsClass[];

      console.log(classes);

      setClasses(classes);
    })();
  }, [user]);

  return (
    <div className="d-flex flex-column">
      {classes.map((studentsClass) => (
        <>
          <Stack gap={2}>
            <h3>{studentsClass.title}</h3>
            <LessonsScheduleForDayForClass
              studentsClassId={studentsClass.id}
              date={date}
            />
          </Stack>

          <hr className="mt-2" />
        </>
      ))}
    </div>
  );
};

export default StudentPage;
