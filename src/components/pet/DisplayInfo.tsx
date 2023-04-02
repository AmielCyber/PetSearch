import FetchSinglePet from "@/hooks/fetchSinglePet";
import PetImageContainer from "./PetImageContainer";
import DisplayInfoSkeleton from "./DisplayInfoSkeleton";
import PetTitle from "./PetTitle";
import PetDescription from "./PetDescription";

type Props = {
  id: number;
};

export default function DisplayInfo(props: Props) {
  const { petData, error, isLoading } = FetchSinglePet(props.id);

  if (error) {
    return <p>Display error message here.</p>;
  }
  if (isLoading || !petData) {
    return <DisplayInfoSkeleton />;
  }

  return (
    <div>
      <PetImageContainer name={petData.name} photos={petData.photos} />
      <PetTitle name={petData.name} />
      <PetDescription petData={petData} />
    </div>
  );
}
