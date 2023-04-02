// Our imports.
import useSWR from "swr";
import type AccessToken from "@/types/AccessToken";
import type Pet from "@/models/Pet";
import useToken from "./useToken";

// All errors are handled by swr.
const fetcher = async (id: number, accessToken: AccessToken | undefined) => {
  // Check if we have an access token.
  if (!accessToken) {
    throw new Error("Failed to validate token");
  }

  // Call our api to fetch a pet with the passed id.
  const response: Response = await fetch(`/api/pets/${id.toString()}`, {
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

export default function FetchSinglePet(id: number) {
  const { accessToken } = useToken(); // Get access token.
  const { data, error, isLoading, mutate } = useSWR(
    accessToken !== undefined ? [id, accessToken] : null,
    ([id, accessToken]) => fetcher(id, accessToken),
    revalidateOptions
  );

  return {
    petData: data,
    error: error,
    isLoading: isLoading,
    mutate: mutate,
  };
}
