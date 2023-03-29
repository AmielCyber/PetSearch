import Alert from "@mui/material/Alert";
import PetSearchHeader from "./PetSearchHeader";
import fetchPets from "@/hooks/fetch-pets";
import PetPageNavigation from "./PetPageNavigation";

type Props = {
  petTypePlural: string;
  searchParams: URLSearchParams;
};
export default function DisplaySearch(props: Props) {
  const { petData, error, isLoading } = fetchPets("/api/search?" + props.searchParams.toString());
  const petType = props.searchParams.get("petType") as string;
  const location = props.searchParams.get("location") as string;

  if (error) {
    return <Alert severity="error">Failed to retrieve pet data...</Alert>;
  }
  if (isLoading || !petData) {
    return (
      <>
        <PetSearchHeader petType={petType} zipCode={location} />
        <p>loading....</p>
      </>
    );
  }

  const totalPages = petData.pagination.total_pages;
  return (
    <>
      <PetSearchHeader petType={petType} zipCode={location} />
      <PetPageNavigation totalPages={totalPages} />
    </>
  );
}
