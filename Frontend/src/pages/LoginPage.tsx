import { FC, useEffect, useState } from "react";
import FormControl from "react-bootstrap/FormControl";
import Form from "react-bootstrap/Form";
import Button from "react-bootstrap/Button";
import { Link, useNavigate } from "react-router";
import { OAuth2Client } from "@badgateway/oauth2-client";
import { useForm } from "react-hook-form";
import { useCookies } from "react-cookie";
import useOAuth from "../hooks/oAuthHook";
import dayjs from "dayjs";

type LoginForm = {
  username: string;
  password: string;
};

const LoginPage: FC = () => {
  const { register, handleSubmit } = useForm<LoginForm>();
  const { oAuthClient, setCookie, cookies } = useOAuth(false);
  const navigate = useNavigate();

  const onSubmit = async (loginForm: LoginForm) => {
    console.log(loginForm);

    const token = await oAuthClient.password({
      username: loginForm.username,
      password: loginForm.password,
      scope: ["user", "openid", "offline_access"],
    });

    setCookie("access-token", token.accessToken);
    setCookie("refresh-token", token.refreshToken);

    console.log({refreshToken: cookies["refresh-token"]})
    console.log({expAt: token.expiresAt})

    setCookie("exp-at", token.expiresAt);

    navigate("/profile")
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
        <FormControl placeholder="Имя пользователя" {...register("username")} />
        <FormControl type="password" {...register("password")} />
        <Button type="submit" className="w-100">
          Войти
        </Button>
      </Form>

      <Link className="mt-3 btn btn-secondary" to="/signup">
        Создать аккаунт
      </Link>
    </div>
  );
};

export default LoginPage;
