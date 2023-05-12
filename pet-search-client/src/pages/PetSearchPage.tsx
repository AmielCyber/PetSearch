import { Suspense, lazy, useContext } from "react";
import { useParams, useSearchParams } from "react-router-dom";
import { Alert, AlertTitle, CircularProgress } from "@mui/material";
// Our imports.
import type { LocationContextType } from "../hooks/LocationContext";
import { LocationContext } from "../hooks/LocationContext";
const DisplaySearch = lazy(() => import("../components/pet-search/DisplaySearch"));

const petSet = new Set(["dogs", "cats"]);

export default function PetSearchPage() {
  const { zipCode } = useContext(LocationContext) as LocationContextType;
  const params = useParams();
  const [searchParams, setSearchParams] = useSearchParams();

  const petType = params.petType ?? "";
  // Invalid petType entered.
  if (!petSet.has(petType)) {
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
  apiSearchParams.append("petType", petType);
  apiSearchParams.append("location", location);
  apiSearchParams.append("page", page);
  const searchQueryURL = "/api/pets?" + apiSearchParams.toString();

  return (
    <Suspense fallback={<CircularProgress />}>
      <DisplaySearch
        petTypePlural={petType}
        searchParams={apiSearchParams}
        searchQueryURL={searchQueryURL}
        onPageChange={handlePageChange}
      />
    </Suspense>
  );
}
