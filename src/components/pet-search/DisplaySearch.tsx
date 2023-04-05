import Alert from "@mui/material/Alert";
// Our imports.
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
  const { petData, error, isLoading, currentPage, totalPages, itemsPerPage } = fetchPets(props.searchQueryURL);
  const petType = props.searchParams.get("petType") as string;
  const location = props.searchParams.get("location") as string;

  // Handle error.
  if (error) {
    return <Alert severity="error">Failed to retrieve {petType} data...</Alert>;
  }

  return (
    <>
      <PetSearchHeader petType={petType} zipCode={location} />
      <h3 style={{ textAlign: "center" }}>
        <PetList petData={petData} isLoading={isLoading} itemsPerPage={itemsPerPage} />
      </h3>
      <PetPageNavigation
        currentPage={currentPage}
        totalPages={totalPages}
        isLoading={isLoading}
        onPageChange={props.onPageChange}
      />
    </>
  );
}
