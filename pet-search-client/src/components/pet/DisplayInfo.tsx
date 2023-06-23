import { Alert } from "@mui/material";
// Our imports.
import useSinglePet from "../../hooks/useSinglePet";
import DisplayInfoSkeleton from "./DisplayInfoSkeleton";
import PetSummary from "./PetSummary.tsx";

type Props = {
  id: string;
};

export default function DisplayInfo(props: Props) {
  const { petData, error, isLoading } = useSinglePet(props.id);

  if (error && !isLoading) {
    return <Alert severity="error">{error.message ? error.message : "Could not fetch pet data."}</Alert>;
  }

  if (isLoading || !petData) {
    return <DisplayInfoSkeleton />;
  }

  return (
      <PetSummary petData={petData} />
  );
}
