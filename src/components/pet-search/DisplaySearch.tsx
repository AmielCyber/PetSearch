import Alert from "@mui/material/Alert";
// Our imports.
import usePetList from "@/hooks/usePetList";
import PetSearchHeader from "@/components/pet-search/PetSearchHeader";
import PetPageNavigation from "@/components/pet-search/PetPageNavigation";
import PetList from "@/components/pet-search/PetList";

type Props = {
  petTypePlural: string;
  searchParams: URLSearchParams;
  searchQueryURL: string;
  onPageChange: (event: React.ChangeEvent<unknown>, value: number) => void;
};

export default function DisplaySearch(props: Props) {
  const { petListData, error, isLoading, currentPage, totalPages, itemsPerPage } = usePetList(props.searchQueryURL);
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
        <PetList petData={petListData} isLoading={isLoading} itemsPerPage={itemsPerPage} />
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
