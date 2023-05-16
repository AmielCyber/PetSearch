import useSWR from "swr";
// Our imports.
import type AccessToken from "../models/accessToken";
import { getStoredToken, storeAccessToken, tokenIsExpired } from "../token/tokenManager";

const BASE_URL = import.meta.env.VITE_API_URL;
const TOKEN_URL = "token";

async function fetcher(url: string): Promise<AccessToken> {
  // Fetch stored token if we have one from a previous session.
  const storedToken = getStoredToken();
  if (storedToken) {
    return storedToken;
  }

  // Expire token in 55 minutes.
  const expirationTime = new Date(new Date().getTime() + 3300 * 1000);

  const response = await fetch(BASE_URL + url);
  const responseData = await response.json();
  if (!response.ok) {
    // Make SWR catch the failed response.
    throw new Error(responseData.message);
  }
  const accessToken: AccessToken = {
    token: "Bearer " + responseData.access_token,
    expirationDate: expirationTime,
  };

  // Store newly fetched token from server.
  storeAccessToken(accessToken);
  return accessToken;
}

const revalidateOptions = {
  revalidateOnFocus: false,
  revalidateIfStale: false,
  revalidateOnReconnect: false,
};

export default function useToken() {
  const { data, error, isLoading, mutate } = useSWR(TOKEN_URL, fetcher, revalidateOptions);

  if (data && tokenIsExpired(data)) {
    // Token has expired so renew token.
    mutate();
  }

  return {
    accessToken: data,
    isLoading,
    isError: error,
  };
}
