import type { ParsedUrlQuery } from "querystring";
import Alert from "@mui/material/Alert";
import AlertTitle from "@mui/material/AlertTitle";
import { useRouter } from "next/router";
// Our component.
import DisplaySearch from "@/components/pet-search/DisplaySearch";

type Props = {
  petTypePlural: string;
  petType: string;
  invalidPetType: boolean;
  page: string;
  location: string;
};

const petSet = new Set(["dogs", "cats"]);

// Validate query parameters and return custom queryProperties
function getQueryProperties(query: ParsedUrlQuery): Props {
  const petTypePlural = query.petTypePlural ? (query.petTypePlural as string) : "unknown";

  if (!petSet.has(petTypePlural)) {
    return {
      petTypePlural: petTypePlural,
      petType: petTypePlural,
      invalidPetType: true,
      location: "92101",
      page: "1",
    };
  }

  const page = query.page ? (query.page as string) : "1";
  const location = query.location ? (query.location as string) : "92101";
  return {
    petTypePlural: petTypePlural,
    petType: petTypePlural.slice(0, petTypePlural.length - 1),
    invalidPetType: false,
    location: location,
    page,
  };
}

export default function PetSearchPage() {
  const router = useRouter();
  const props: Props = getQueryProperties(router.query);

  // Invalid petType entered.
  if (props.invalidPetType) {
    return (
      <Alert severity="error">
        <AlertTitle>Error</AlertTitle>
        Pet Type: {props.petTypePlural} not supported.
      </Alert>
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
  const searchQueryURL = "/api/search?" + params.toString();

  return (
    <DisplaySearch
      petTypePlural={props.petTypePlural}
      searchParams={params}
      searchQueryURL={searchQueryURL}
      onPageChange={handlePageChange}
    />
  );
}
