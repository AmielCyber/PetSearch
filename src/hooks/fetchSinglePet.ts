// Our imports.
import useSWR from "swr";
import type AccessToken from "@/types/AccessToken";
import type Pet from "@/models/Pet";
import useToken from "./useToken";
import { useSWRConfig } from "swr";

// All errors are handled by swr.
const fetcher = async (url: string, accessToken: AccessToken | undefined) => {
  // Check if we have an access token.
  if (!accessToken) {
    throw new Error("Failed to validate token.");
  }

  // Call our api to fetch a pet with the passed id.
  const response: Response = await fetch(url, {
    method: "GET",
    headers: {
      Authorization: `Bearer ${accessToken.token}`,
      "Content-Type": "application/json",
    },
  });

  // Check if response is successful.
  if (!response.ok) {
    const responseData = await response.json();
    // Make SWR catch the failed response.
    throw new Error(responseData.statusText);
  }

  // Successful response.
  const responseData: Promise<Pet> = await response.json();
  return responseData;
};

// Set revalidation options, fetches data again if true during the following conditions:
const revalidateOptions = {
  revalidateOnFocus: false,
  revalidateIfStale: false,
  revalidateOnReconnect: false,
};

export default function FetchSinglePet(id: string) {
  const { accessToken } = useToken(); // Get access token.
  const { data, error, isLoading } = useSWR(
    accessToken !== undefined ? [`/api/pets/${id}`, accessToken] : null,
    ([url, accessToken]) => fetcher(url, accessToken),
    revalidateOptions
  );

  return {
    petData: data,
    error: error,
    isLoading: isLoading,
  };
}
