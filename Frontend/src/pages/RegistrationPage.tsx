import { FC } from "react";
import { Button, Form, FormControl } from "react-bootstrap";
import { useForm } from "react-hook-form";
import { Link, useNavigate } from "react-router";
import useOAuth from "../hooks/oAuthHook";
import User from "../types/User";

type RegistrationForm = {
  name: string;
  surname: string;
  schoolId: number | null;
  email: string;
  phoneNumber: string;
  userName: string;
  password: string;
};

const RegistrationPage: FC = () => {
  const { register, handleSubmit } = useForm<RegistrationForm>();
  const { oAuthClient, setCookie, cookies } = useOAuth(false);
  const navigate = useNavigate();

  const onSubmit = async (registrationForm: RegistrationForm) => {
    console.log(registrationForm);

    var response = await fetch("https://localhost:7290/api/Users", {
      method: "POST",
      body: JSON.stringify(registrationForm),
      headers: {
        "Content-Type": "application/json",
      },
    });

    var user = (await response.json()) as User;

    if (!user) {
      return;
    }

    const token = await oAuthClient.password({
      username: user.userName,
      password: registrationForm.password,
      scope: ["user", "openid", "offline_access"],
    });

    setCookie("access-token", token.accessToken);
    setCookie("refresh-token", token.refreshToken);

    console.log(cookies["access-token"]);
    console.log(cookies["refresh-token"]);

    navigate("/profile");
  };

  return (
    <div
      className="d-flex flex-column h-100 align-items-center justify-content-center"
      style={{
        minHeight: "100vh",
      }}
    >
      <Form
        className="w-25 d-flex flex-column row-gap-1 align-items-center"
        onSubmit={handleSubmit(onSubmit)}
      >
        <FormControl placeholder="Имя пользователя" {...register("userName")} />

        <FormControl placeholder="Имя" {...register("name")} />

        <FormControl placeholder="Фамилия" {...register("surname")} />

        <FormControl
          placeholder="ID школы"
          {...register("schoolId", {
            required: false,
          })}
        />

        <FormControl placeholder="Email" {...register("email")} />

        <FormControl
          placeholder="Номер телефона"
          {...register("phoneNumber")}
        />

        <FormControl type="password" {...register("password")} placeholder="Пароль" />

        <Button type="submit" className="w-100">
          Создать аккаунт
        </Button>
      </Form>

      <Link className="mt-3 btn btn-secondary" to="/login">
        Войти в аккаунт
      </Link>
    </div>
  );
};

export default RegistrationPage;
