import { FC, useEffect, useState } from "react";
import useOAuth from "../hooks/oAuthHook";
import User from "../types/User";
import LessonsScheduleForDayForTeacher from "../LessonsScheduleForDayForTeacher";

const TeacherPage: FC = () => {
  const { getCurrentUserOrRedirect } = useOAuth();
  const [user, setUser] = useState<User | undefined>();

  // const date = dayjs().format("YYYY-MM-DD");
  const date = "2025-01-06";

  useEffect(() => {
    getUser: (async () => {
      setUser(await getCurrentUserOrRedirect());
    })();
  }, []);

  return (
    <div className="d-flex flex-column">
      {user ? (
        <LessonsScheduleForDayForTeacher teacherId={user?.id} date={date} />
      ) : (
        "Загрузка..."
      )}
    </div>
  );
};

export default TeacherPage;
