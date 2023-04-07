import Grid from "@mui/material/Grid";
// Our imports.
import type Pet from "@/models/Pet";
import PageTitle from "@/components/pet/PageTitle";
import PetImageContainer from "@/components/pet/PetImageContainer";
import PetAttributes from "@/components/pet/PetAttributes";
import Description from "@/components/pet/Description";
type Props = {
  petData: Pet;
};
export default function PetSummary(props: Props) {
  return (
    <Grid container justifyContent="center" columnSpacing={4} rowSpacing={3} alignItems="center">
      <Grid item xs={12} textAlign="center" mt={2}>
        <PageTitle name={props.petData.name} />
      </Grid>
      <Grid item>
        <PetImageContainer name={props.petData.name} photos={props.petData.photos} />
      </Grid>
      <Grid item textAlign="center">
        <PetAttributes petData={props.petData} />
      </Grid>
      <Grid item textAlign="center" xs={12}>
        <Description description={props.petData.description} url={props.petData.url} />
      </Grid>
    </Grid>
  );
}
