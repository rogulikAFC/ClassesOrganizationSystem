import RoleInSchool from "./RoleInSchool";
import SchoolAnnotation from "./SchoolAnnotation";

type User = {
  id: number;
  name: string;
  surname: string;
  email: string;
  userName: string;
  rolesInSchools: RoleInSchool[];
  schools: SchoolAnnotation[];
  roles: string[];
};

export default User;
