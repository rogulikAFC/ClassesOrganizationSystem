import { FC, useEffect, useState } from "react";
import FormControl from "react-bootstrap/FormControl";
import Form from "react-bootstrap/Form";
import Button from "react-bootstrap/Button"
import { Link } from "react-router";
import { OAuth2Client } from "@badgateway/oauth2-client";
import { useForm } from "react-hook-form";
import { useCookies } from "react-cookie";

type LoginForm = {
  username: string;
  password: string;
}

const LoginPage: FC = () => {
  const {
    register,
    handleSubmit,
  } = useForm<LoginForm>();

  const [cookies, setCookie, removeCookie] = useCookies(["access-token"])

  const oAuthClient = new OAuth2Client({
    server: "https://localhost:7290/",
    clientId: "application",
    clientSecret: "secret",  // Just test client secret
    discoveryEndpoint: "/.well-known/openid-configuration",
    authorizationEndpoint: "/connect/authorize",
    tokenEndpoint: "/connect/token"
  });

  const onSubmit = async (loginForm: LoginForm) => {
    console.log(loginForm);

    // Token have to be saved in cookies
    const token = await oAuthClient.password({
      username: loginForm.username,
      password: loginForm.password,
      scope: ["user"]
    });

    setCookie("access-token", token.accessToken)

    console.log(cookies["access-token"])
  } 

  return (
    <div className="d-flex flex-column h-100 align-items-center justify-content-center" style={{
      minHeight: "100vh"
    }}>
      <Form className="w-25 d-flex flex-column row-gap-1 align-items-center" onSubmit={handleSubmit(onSubmit)}>
        <FormControl placeholder="Имя пользователя" {...register("username")} />
        <FormControl type="password" {...register("password")} />
        <Button type="submit" className="w-100">Войти</Button>
      </Form>

      <Link className="mt-3 btn btn-secondary" to="/signup">Создать аккаунт</Link>
    </div>
  )
}

export default LoginPage;