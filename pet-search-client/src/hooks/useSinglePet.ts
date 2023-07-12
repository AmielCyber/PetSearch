import useSWR from "swr";
// Our imports.
import type Pet from "../models/pet.ts";

const BASE_URL = import.meta.env.VITE_API_URL;

// All errors are handled by swr.
const fetcher = async (url: string) => {
  // Call our api to fetch a pet with the passed id.
  const response: Response = await fetch(BASE_URL + url, {
    method: "GET",
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

export default function useSinglePet(id: string) {
  const { data, error, isLoading } = useSWR(
    `pets/${id}`,
    (url) => fetcher(url),
    revalidateOptions
  );

  return {
    petData: data,
    error: error,
    isLoading: isLoading,
  };
}
