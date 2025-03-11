import { FC, useEffect, useState } from "react";
import useOAuth from "../hooks/oAuthHook";
import User from "../types/User";
import {
  Stack,
} from "react-bootstrap";
import { Link } from "react-router";
import RoleInSchool from "../types/RoleInSchool";

const ProfilePage: FC = () => {
  const { getCurrentUser } = useOAuth();
  const [ user, setUser ] = useState<User | undefined>();
  const [ userRoles, setUserRoles ] = useState<string[]>([]);

  useEffect(() => {
    getUser: (async () => {
      setUser(await getCurrentUser())
    })()
  }, [])

  useEffect(() => {
    setUserRoles(
      [...new Set(user?.rolesInSchools.map(roleInSchool => roleInSchool.role))] // Lefts only unique roles
    )
  }, [user])

  return (
    <Stack gap={3} className="align-items-center">
      <h2>Здравствуйте, {user?.name}!</h2>

      <div className="d-flex flex-col gap-2">
        {userRoles.map((role, i) => (
          <Link className="btn btn-primary" key={i} to={`/profile/${role}`} >
            {role}
          </Link>
        ))}
      </div>
    </Stack>

    
  );
};

export default ProfilePage;
