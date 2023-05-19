import {Grid, Skeleton} from "@mui/material";
// Out imports.
import type Pet from "../../models/pet.ts";
import PetListCard from "../cards/PetListCard";

function getSkeletonItems(itemsPerPage: number): React.ReactNode[] {
  const skeletonList = new Array<React.ReactNode>(itemsPerPage);
  for (let i = 0; i < skeletonList.length; i++) {
    skeletonList[i] = (
      <Grid item key={"grid-item-key-" + i}>
          <Skeleton variant="rectangular" animation="wave" width={300} height={300} />
      </Grid>
    );
  }
  return skeletonList;
}

type Props = {
  petData: Pet[] | undefined;
  isLoading: boolean;
  itemsPerPage: number;
};

export default function PetSelectionCard(props: Props) {
  return (
    <Grid container spacing={2} justifyContent="center">
      {!props.petData || props.isLoading
        ? getSkeletonItems(props.itemsPerPage)
        : props.petData.map((pet, i) => <PetListCard key={"pet-id-" + i} pet={pet} />)}
    </Grid>
  );
}
