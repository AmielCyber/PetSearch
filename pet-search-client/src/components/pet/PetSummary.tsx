import { Grid } from "@mui/material";
// Our imports.
import type Pet from "../../models/pet";
import PageTitle from "./PageTitle";
import PetImageContainer from "./PetImageContainer";
import PetAttributes from "./PetAttributes";
import Description from "./Description";

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
