import dynamic from "next/dynamic";
import Alert from "@mui/material/Alert";
// Our imports.
import useSinglePet from "@/hooks/useSinglePet";
import DisplayInfoSkeleton from "@/components/pet/DisplayInfoSkeleton";
const PetSummary = dynamic(() => import("@/components/pet/PetSummary"), {
  loading: () => <DisplayInfoSkeleton />,
});

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

  return <PetSummary petData={petData} />;
}
