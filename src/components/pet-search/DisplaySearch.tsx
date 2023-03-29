import PetSearchHeader from "./PetSearchHeader";
import fetchPets from "@/hooks/fetch-pets";
import PetPageNavigation from "./PetPageNavigation";
type Props = {
  petTypePlural: string;
  searchParams: URLSearchParams;
};
export default function DisplaySearch(props: Props) {
  const pets = fetchPets("/api/search?" + props.searchParams.toString());

  const petType = props.searchParams.get("petType") as string;
  const location = props.searchParams.get("location") as string;
  const totalPages = pets.pets?.pagination.total_pages || 1;

  if (!pets || !pets.pets) {
    return (
      <>
        <PetSearchHeader petType={petType} zipCode={location} />
        <p>loading....</p>
      </>
    );
  }
  return (
    <>
      <PetSearchHeader petType={petType} zipCode={location} />
      <PetPageNavigation totalPages={totalPages} />
    </>
  );
}
