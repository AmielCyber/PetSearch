import Alert from "@mui/material/Alert";
// Our imports.
import usePetList from "../../hooks/usePetList";
import PetSearchHeader from "./PetSearchHeader";
import PetPageNavigation from "./PetPageNavigation";
import PetList from "./PetList";

type Props = {
  locationName: string;
  searchParams: URLSearchParams;
  searchQueryURL: string;
  onPageChange: (event: React.ChangeEvent<unknown>, value: number) => void;
};

export default function DisplaySearch(props: Props) {
  const { petListData, error, isLoading, currentPage, totalPages, itemsPerPage } = usePetList(props.searchQueryURL);
  const petType = props.searchParams.get("type") as string;

  // Handle error.
  if (error) {
    return <Alert severity="error">Failed to retrieve {petType} data...</Alert>;
  }

  return (
    <>
      <PetSearchHeader petType={petType} locationName={props.locationName} />
      <PetList petData={petListData} isLoading={isLoading} itemsPerPage={itemsPerPage} />
      <PetPageNavigation
        currentPage={currentPage}
        totalPages={totalPages}
        isLoading={isLoading}
        onPageChange={props.onPageChange}
      />
    </>
  );
}
