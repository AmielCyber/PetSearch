import type { ParsedUrlQuery } from "querystring";
import Alert from "@mui/material/Alert";
import AlertTitle from "@mui/material/AlertTitle";
import { useContext } from "react";
import { useRouter } from "next/router";
// Our imports.
import type { LocationContextType } from "@/hooks/LocationContext";
import { LocationContext } from "@/hooks/LocationContext";
import SearchPageMeta from "@/components/meta/SearchPageMeta";
import DisplaySearch from "@/components/pet-search/DisplaySearch";

type QueryProps = {
  petTypePlural: string;
  petType: string;
  invalidPetType: boolean;
  page: string;
  location: string;
};

const petSet = new Set(["dogs", "cats"]);

// Validate query parameters and return custom queryProperties
function getQueryProperties(query: ParsedUrlQuery, currentZipCode: string): QueryProps {
  const petTypePlural = query.petTypePlural ? (query.petTypePlural as string) : "unknown";

  if (!petSet.has(petTypePlural)) {
    return {
      petTypePlural: petTypePlural,
      petType: petTypePlural,
      invalidPetType: true,
      location: "",
      page: "",
    };
  }

  const page = query.page ? (query.page as string) : "1";
  const location = query.location ? (query.location as string) : currentZipCode;
  return {
    petTypePlural: petTypePlural,
    petType: petTypePlural.slice(0, petTypePlural.length - 1),
    invalidPetType: false,
    location: location,
    page,
  };
}

export default function PetSearchPage() {
  const { zipCode } = useContext(LocationContext) as LocationContextType;
  const router = useRouter();
  const props: QueryProps = getQueryProperties(router.query, zipCode);

  // Invalid petType entered.
  if (props.invalidPetType) {
    return (
      <>
        <SearchPageMeta />
        <Alert severity="error">
          <AlertTitle>Error</AlertTitle>
          Pet Type: {props.petTypePlural} not supported.
        </Alert>
      </>
    );
  }

  const handlePageChange = (event: React.ChangeEvent<unknown>, value: number) => {
    // For pagination change.
    router.push({
      query: {
        ...router.query,
        page: value,
      },
    });
  };

  const params = new URLSearchParams();
  params.append("petType", props.petType);
  params.append("location", props.location);
  params.append("page", props.page);
  const searchQueryURL = "/api/pets?" + params.toString();

  return (
    <>
      <SearchPageMeta />
      <DisplaySearch
        petTypePlural={props.petTypePlural}
        searchParams={params}
        searchQueryURL={searchQueryURL}
        onPageChange={handlePageChange}
      />
    </>
  );
}
