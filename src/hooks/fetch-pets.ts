import useSWR from "swr";
// Our imports.
import type AccessToken from "@/types/AccessToken";
import type PetResponse from "@/models/PetResponse";
import useToken from "./useToken";

const fetcher = async (url: string, accessToken: AccessToken) => {
  if (!accessToken) {
    throw new Error("Failed to validate token");
  }
  const response = await fetch(url, {
    method: "GET",
    headers: {
      Authorization: `Bearer ${accessToken.token}`,
      "Content-Type": "application/json",
    },
  });
  if (!response.ok) {
    const responseData = await response.json();
    // Make SWR catch the failed response.
    throw new Error(responseData.message);
  }
  const responseData: Promise<PetResponse> = await response.json();
  return responseData;
};

// Set revalidation options, fetches data again if true during the following conditions:
const revalidateOptions = {
  revalidateOnFocus: false,
  revalidateIfStale: false,
  revalidateOnReconnect: false,
};

export default function FetchPets(url: string) {
  const { accessToken } = useToken(); // Get access token.
  // Only fetch data if we have an access token.
  // Will pass a conditional function later to handle tokens stored in browser.
  const { data, error, isLoading } = useSWR(
    accessToken ? [url, accessToken] : null,
    ([url, accessToken]) => fetcher(url, accessToken),
    revalidateOptions
  );

  return {
    petData: data,
    error: error,
    isLoading: isLoading,
  };
}
