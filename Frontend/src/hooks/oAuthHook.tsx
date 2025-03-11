import { OAuth2Client, OAuth2Token } from "@badgateway/oauth2-client";
import { useCookies } from "react-cookie";
import User from "../types/User";
import { useEffect, useState } from "react";
import { useNavigate } from "react-router";
import dayjs from "dayjs";

const useOAuth = (getUserAtFirst: boolean = true) => {
  const [cookies, setCookie, removeCookie] = useCookies([
    "access-token",
    "refresh-token",
    "exp-at",
  ]);
  const [user, setUser] = useState<User>();
  const navigate = useNavigate();

  const oAuthClient = new OAuth2Client({
    server: "https://localhost:7290/",
    clientId: "application",
    clientSecret: "secret", // Just test client secret
    discoveryEndpoint: "/.well-known/openid-configuration",
    authorizationEndpoint: "/connect/authorize",
    tokenEndpoint: "/connect/token",
  });

  // console.log(parseInt(cookies["exp-at"]))
  // console.log(dayjs().utcOffset())

  const getAccessToken = (): string => {
    console.log({
      token: cookies["access-token"],
      // timeLeft: parseInt(cookies["exp-at"]) - dayjs().utcOffset(),
    });

    return cookies["access-token"];
  };

  const getCurrentUser = async (redirect: boolean = true): Promise<User> => {
    let userInfoResponse = await fetch(
      "https://localhost:7290/connect/userinfo",
      {
        method: "GET",
        headers: new Headers({
          Authorization: "Bearer " + getAccessToken(),
        }),
      }
    );

    console.log({ userInfoResponse });

    if (!userInfoResponse.ok) {
      console.log("Response wasn't ok");

      const oAuth2Token = {
        accessToken: cookies["access-token"],
        refreshToken: cookies["refresh-token"],
      } as OAuth2Token;

      try {
        const newToken = await oAuthClient.refreshToken(oAuth2Token, {
          scope: ["user", "admin", "openid", "offline_access"],
          // resource: ["openid", "profile"],

        });

        console.log({ newToken });

        setCookie("access-token", newToken.accessToken);
        setCookie("refresh-token", newToken.refreshToken);

        userInfoResponse = await fetch(
          "https://localhost:7290/connect/userinfo",
          {
            method: "GET",
            headers: new Headers({
              Authorization: "Bearer " + getAccessToken(),
            }),
          }
        );
      } catch {
        redirect && navigate("/login");
      }
    }

    const userId = (await userInfoResponse.json()).sub;

    const getUserResponse = await fetch(
      "https://localhost:7290/api/Users/" + userId,
      {
        method: "GET",
        headers: new Headers({
          Authorization: "Bearer " + getAccessToken(),
        }),
      }
    );

    return (await getUserResponse.json()) as User;
  };

  useEffect(() => {
    getUserAtFirst &&
      (async () => {
        const currentUser = await getCurrentUser();

        setUser(currentUser);
      })();
  }, [cookies["access-token"]]);

  return {
    getAccessToken,
    getCurrentUser,
    oAuthClient,
    user,
    cookies,
    setCookie,
    removeCookie,
  };
};

export default useOAuth;
