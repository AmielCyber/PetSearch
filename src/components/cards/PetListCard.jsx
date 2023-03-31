import { Card, CardContent, Typography } from "@mui/material";
import Image from "next/image";
import CatIcon from "@/components/icons/CatIcon";
import DogIcon from "@/components/icons/DogIcon";

const petIcons = {
  fontSize: "200px",
  color: "#212427",
};

export default function PetListCard({ pet }) {
  // Temporarily hardcoding to medium sized img, icon otherwise
  const img = pet.primary_photo_cropped?.medium
    ? pet.primary_photo_cropped.medium
    : null;

  return (
    <Card>
      {img ? (
        <Image src={img} width={200} height={200} alt="Picture of pet" />
      ) : pet.type == "Cat" ? (
        <CatIcon sx={petIcons} />
      ) : (
        <DogIcon sx={petIcons} />
      )}
      <CardContent>
        <Typography>
          <p>
            <b>Name:</b> {pet.name}
            <br />
            <b>ID:</b> {pet.id}
            <br />
            <b>Gender:</b> {pet.gender}
            <br />
            <b>Age:</b> {pet.age}
          </p>
        </Typography>
      </CardContent>
    </Card>
  );
}
