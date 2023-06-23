import {  useContext } from "react";
import { useParams, useSearchParams } from "react-router-dom";
import { Alert, AlertTitle } from "@mui/material";
// Our imports.
import DisplaySearch from "../components/pet-search/DisplaySearch.tsx";
import type { LocationContextType } from "../hooks/LocationContext";
import { LocationContext } from "../hooks/LocationContext";

const petMap = new Map().set("dogs","dog").set("cats","cat");

export default function PetSearchPage() {
  const { zipCode } = useContext(LocationContext) as LocationContextType;
  const params = useParams();
  const [searchParams, setSearchParams] = useSearchParams();

  const petType = params.petType ?? "";
  // Invalid petType entered.
  if (!petMap.has(petType)) {
    return (
      <Alert severity="error">
        <AlertTitle>Error</AlertTitle>
        Pet Type: {petType} not supported.
      </Alert>
    );
  }

  const handlePageChange = (_: React.ChangeEvent<unknown>, value: number) => {
    // For pagination change.
    const newSearchParams = new URLSearchParams(searchParams);
    newSearchParams.set("page", value.toString());
    setSearchParams(newSearchParams);
  };

  const location = searchParams.get("location") ?? zipCode;
  const page = searchParams.get("page") ?? "1";

  // petType, location, page
  const apiSearchParams = new URLSearchParams();
  apiSearchParams.append("type", petMap.get(petType));
  apiSearchParams.append("location", location);
  apiSearchParams.append("page", page);
  const searchQueryURL = "pets?" + apiSearchParams.toString();

  return (
      <DisplaySearch
        searchParams={apiSearchParams}
        searchQueryURL={searchQueryURL}
        onPageChange={handlePageChange}
      />
  );
}
