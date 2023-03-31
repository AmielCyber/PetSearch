//import type Pet from "@/models/Pet";
import { Grid } from "@mui/material";
import PetListCard from "../cards/PetListCard";

// type Props = {
//   petData: Pet[];
// };

export default function PetSelectionCard({ petData }) {
  return (
    <Grid container spacing={2} justifyContent="center">
      {petData.map((pet) => (
        <Grid item key={pet.id} xs>
          <PetListCard pet={pet} />
        </Grid>
      ))}
    </Grid>
  );
}
