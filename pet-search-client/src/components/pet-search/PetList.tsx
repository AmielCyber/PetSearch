//import type Pet from "@/models/Pet";
import { Grid, Skeleton } from "@mui/material";
import PetListCard from "../cards/PetListCard";
import type Pet from "@/models/Pet";

type Props = {
  petData: Pet[] | undefined;
  isLoading: boolean;
  itemsPerPage: number;
};

export default function PetSelectionCard(props: Props) {
  return (
    <Grid container spacing={2} justifyContent="center">
      {!props.petData || props.isLoading
        ? [...Array(props.itemsPerPage)].map((e, i) => (
            <Grid item key={"grid-item-key-" + i} xs>
              <Skeleton
                key={"skeleton-item-key-" + i}
                variant="rectangular"
                animation="wave"
                width={200}
                height={250}
              />
            </Grid>
          ))
        : props.petData.map((pet, i) => (
            <PetListCard key={"pet-id-" + i} pet={pet} />
          ))}
    </Grid>
  );
}
