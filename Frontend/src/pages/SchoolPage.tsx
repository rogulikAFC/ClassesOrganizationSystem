import { FC, useEffect, useState } from "react";
import { Link, redirect, useParams } from "react-router";
import useOAuth from "../hooks/oAuthHook";
import School from "../types/School";
import { Stack, Table } from "react-bootstrap";
import ListOfRooms from "../ListOfRooms";

type SchoolPageParams = {
  schoolId: string;
};

const SchoolPage: FC = () => {
  const { user, getCurrentUser } = useOAuth();
  const { schoolId } = useParams<SchoolPageParams>();

  const [school, setSchool] = useState<School>();

  useEffect(() => {
    getSchool: (async () => {
      const response = await fetch(
        "https://localhost:7290/api/Schools/" + schoolId
      );

      setSchool((await response.json()) as School);
    })();

    getUser: (async () => {
      await getCurrentUser(false);
    })();
  }, [schoolId]);

  return (
    <Stack gap={3} className="align-items-center">
      <h2>{school?.title}</h2>

      <Table>
        <tbody>
          <tr>
            <td>Адрес</td>
            <td>{school?.address}</td>
          </tr>

          <tr>
            <td>Номер телефона</td>
            <td>{school?.phone}</td>
          </tr>

          <tr>
            <td>Email</td>
            <td>{school?.email}</td>
          </tr>
        </tbody>
      </Table>

      
      <h2>Кабинеты</h2>
      {schoolId && user && (
        <ListOfRooms schoolId={schoolId} pageSize={2} isOpened={null} />
      )}
    </Stack>
  );
};

export default SchoolPage;
