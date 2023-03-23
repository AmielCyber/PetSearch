import useSWR from "swr";
import useToken from "./useToken";
import AccessToken from "@/types/AccessToken";

const fetcher = async (url: string, accessToken: AccessToken) => {
  console.log(accessToken);
  if (!accessToken) {
    throw new Error("Failed to validate token");
  }
  const response = await fetch(url, {
    method: "GET",
    headers: {
      Authorization: accessToken.token,
      "Content-Type": "application/json",
    },
  });
  const responseData = await response.json();
  if (!response.ok) {
    // Make SWR catch the failed response.
    throw new Error(responseData.message);
  }
  return responseData;
};

const revalidateOptions = {
  revalidateOnFocus: false,
  revalidateIfStale: false,
  revalidateOnReconnect: false,
};

export default function FetchPets(url: string) {
  const { accessToken } = useToken();
  const { data, error, isLoading } = useSWR(
    accessToken ? [url, accessToken] : null,
    ([url, accessToken]) => fetcher(url, accessToken),
    revalidateOptions
  );

  return {
    pets: data,
    isLoading: isLoading,
    isError: error,
  };
}
