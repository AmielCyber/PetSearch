//import type Pet from "@/models/Pet";
import { Grid, Skeleton } from "@mui/material";
import PetListCard from "../cards/PetListCard";

// type Props = {
//   petData: Pet[];
// };

export default function PetSelectionCard({ petData, isLoading, itemsPerPage }) {
  return (
    <Grid container spacing={2} justifyContent="center">
      {!petData || isLoading
        ? [...Array(itemsPerPage)].map((e, i) => (
            <Grid item key={i} xs>
              <Skeleton
                variant="rectangular"
                animation="wave"
                width={200}
                height={250}
              />
            </Grid>
          ))
        : petData.map((pet) => <PetListCard pet={pet} />)}
    </Grid>
  );
}
