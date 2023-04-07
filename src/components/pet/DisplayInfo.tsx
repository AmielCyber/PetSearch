import dynamic from "next/dynamic";
import CircularProgress from "@mui/material/CircularProgress";
import Alert from "@mui/material/Alert";
import Grid from "@mui/material/Grid";
// Our imports.
import useSinglePet from "@/hooks/useSinglePet";
import DisplayInfoSkeleton from "@/components/pet/DisplayInfoSkeleton";
import PageTitle from "@/components/pet/PageTitle";
import PetAttributes from "@/components/pet/PetAttributes";
import Description from "@/components/pet/Description";
const PetImageContainer = dynamic(() => import("@/components/pet/PetImageContainer"), {
  loading: () => <CircularProgress />,
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

  return (
    <Grid container justifyContent="center" columnSpacing={4} rowSpacing={3} alignItems="center">
      <Grid item xs={12} textAlign="center" mt={2}>
        <PageTitle name={petData.name} />
      </Grid>
      <Grid item>
        <PetImageContainer name={petData.name} photos={petData.photos} />
      </Grid>
      <Grid item textAlign="center">
        <PetAttributes petData={petData} />
      </Grid>
      <Grid item textAlign="center" xs={12}>
        <Description description={petData.description} url={petData.url} />
      </Grid>
    </Grid>
  );
}
