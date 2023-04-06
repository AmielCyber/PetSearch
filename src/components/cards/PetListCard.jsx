import { Card, Typography, Grid } from "@mui/material";
import Image from "next/image";
import CatIcon from "@/components/icons/CatIcon";
import DogIcon from "@/components/icons/DogIcon";

const petIcons = {
  fontSize: "200px",
  color: "primary",
};
const textStyles = {
  marginTop: "5px",
  marginBottom: "16px",
  color: "primary",
};

export default function PetListCard({ pet }) {
  // Temporarily hardcoding to medium sized img, icon otherwise
  const img = pet.primary_photo_cropped?.medium
    ? pet.primary_photo_cropped.medium
    : null;

  return (
    <Grid item key={pet.id}>
      <Card sx={{ maxWidth: "200px" }}>
        {img ? (
          <Image src={img} width={200} height={200} alt="Picture of pet" />
        ) : pet.type == "Cat" ? (
          <CatIcon sx={petIcons} />
        ) : (
          <DogIcon sx={petIcons} />
        )}
        <Typography sx={textStyles}>
          <b>{pet.name}</b>
          <br />
          <br />
          <b>Gender:</b> {pet.gender}
          <br />
          <b>Age:</b> {pet.age}
        </Typography>
      </Card>
    </Grid>
  );
}
