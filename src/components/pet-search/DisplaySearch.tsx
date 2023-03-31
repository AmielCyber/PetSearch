import Alert from "@mui/material/Alert";
import Box from "@mui/material/Box";
import LinearProgress from "@mui/material/LinearProgress";
// Our components.
import fetchPets from "@/hooks/fetch-pets";
import PetSearchHeader from "./PetSearchHeader";
import PetPageNavigation from "./PetPageNavigation";
import PetList from "./PetList";

type Props = {
  petTypePlural: string;
  searchParams: URLSearchParams;
  searchQueryURL: string;
  onPageChange: (event: React.ChangeEvent<unknown>, value: number) => void;
};

export default function DisplaySearch(props: Props) {
  const { petData, error, isLoading, currentPage, totalPages } = fetchPets(
    props.searchQueryURL
  );
  const petType = props.searchParams.get("petType") as string;
  const location = props.searchParams.get("location") as string;

  // Handle error.
  if (error) {
    return <Alert severity="error">Failed to retrieve {petType} data...</Alert>;
  }

  return (
    <>
      <PetSearchHeader petType={petType} zipCode={location} />
      {isLoading || !petData ? (
        <Box sx={{ width: "100%" }}>
          <LinearProgress />
        </Box>
      ) : (
        <h3 style={{ textAlign: "center" }}>
          <PetList petData={petData} />
        </h3>
      )}
      <PetPageNavigation
        currentPage={currentPage}
        totalPages={totalPages}
        isLoading={isLoading}
        onPageChange={props.onPageChange}
      />
    </>
  );
}
