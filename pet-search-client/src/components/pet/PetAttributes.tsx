import { Typography } from "@mui/material";
// Our import.
import type Pet from "../../models/pet";

type Props = {
  petData: Pet;
};

export default function PetAttributes(props: Props) {
  return (
    <Typography variant="body1" component="section">
      <Typography variant="subtitle1">
        <b>Age</b>
      </Typography>
      {props.petData.age}
      <Typography variant="subtitle1">
        <b>Size</b>
      </Typography>
      {props.petData.size}
      <Typography variant="subtitle1">
        <b>Gender</b>
      </Typography>
      {props.petData.gender}
      <Typography variant="subtitle1">
        <b>Status</b>
      </Typography>
      {props.petData.status}
    </Typography>
  );
}
