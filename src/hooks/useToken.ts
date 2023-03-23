import AccessToken from "@/types/AccessToken";
import useSWR from "swr";

// Will implement auto renew token later.

async function fetcher(uri: string): Promise<AccessToken> {
  const expirationDate = new Date(new Date().getTime() + 3600 * 1000);
  const response = await fetch(uri);
  const responseData = await response.json();
  if (!response.ok) {
    // Make SWR catch the failed response.
    throw new Error(responseData.message);
  }
  return {
    token: responseData.access_token,
    expirationDate: expirationDate,
  };
}

const revalidateOptions = {
  revalidateOnFocus: false,
  revalidateIfStale: false,
  revalidateOnReconnect: false,
};

export default function useToken() {
  const { data, error, isLoading } = useSWR("/api/token", fetcher, revalidateOptions);

  return {
    accessToken: data,
    isLoading,
    isError: error,
  };
}
