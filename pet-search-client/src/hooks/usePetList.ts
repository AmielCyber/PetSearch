import useSWR from "swr";
import { useRef } from "react";
// Our imports.
import type AccessToken from "../models/accessToken";
import type PetResponse from "../models/petResponse.ts";
import useToken from "./useToken";

const BASE_URL = import.meta.env.VITE_API_URL;

const ITEMS_PER_PAGE = 20;

// Let SWR handle all errors.
const fetcher = async (url: string, accessToken: AccessToken | undefined) => {
  // Check for access token.
  if (!accessToken) {
    throw new Error("Failed to validate token");
  }

  // Call our endpoint.
  const response: Response = await fetch(BASE_URL + url, {
    method: "GET",
    headers: {
      Authorization: accessToken.token,
      "Content-Type": "application/json",
    },
  });

  if (!response.ok) {
    const responseData = await response.json();
    // Make SWR catch the failed response.
    throw new Error(responseData?.message || response.statusText);
  }
  // Return result.
  const responseData: Promise<PetResponse> = await response.json();
  return responseData;
};

// Set revalidation options, fetches data again if true during the following conditions:
const revalidateOptions = {
  revalidateOnFocus: false,
  revalidateIfStale: false,
  revalidateOnReconnect: false,
};

export default function usePetList(url: string) {
  const { accessToken } = useToken(); // Get access token.
  const currentPageRef = useRef(1); // Maintain current page while fetching data.
  const totalPagesRef = useRef(1); // Maintain total pages while fetching data.
  // Only fetch data if we have an access token.
  const { data, error, isLoading } = useSWR(
    accessToken ? url : null,
    (url) => fetcher(url, accessToken),
    revalidateOptions
  );

  // Adjust current page or total pages if they had changed.
  if (data?.pagination) {
    if (totalPagesRef.current !== data.pagination.total_pages) {
      totalPagesRef.current = data.pagination.total_pages;
    }
    if (currentPageRef.current !== data.pagination.current_page) {
      currentPageRef.current = data.pagination.current_page;
    }
  }

  return {
    petListData: data?.pets,
    error: error,
    isLoading: isLoading,
    currentPage: currentPageRef.current,
    totalPages: totalPagesRef.current,
    itemsPerPage: ITEMS_PER_PAGE,
  };
}
