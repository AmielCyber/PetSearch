import { Card, Typography, Grid, CardActionArea } from "@mui/material";
import Image from "next/image";
import CatIcon from "@/components/icons/CatIcon";
import DogIcon from "@/components/icons/DogIcon";
import useSinglePet from "@/hooks/useSinglePet";
import { useSWRConfig } from "swr";
import Link from "next/link";
import type Pet from "@/models/Pet";

type Props = {
  pet: Pet,
};

// Styles
const petIcons = {
  fontSize: "200px",
  color: "primary",
};
const textStyles = {
  marginTop: "5px",
  marginBottom: "16px",
  color: "primary",
};

export default function PetListCard(props: Props) {
  const { mutate } = useSWRConfig();
  function handleClick() {
    mutate(`/api/pets/${props.pet.id}`, props.pet);
  }
  // Temporarily hardcoding to medium sized img, icon otherwise
  const img = props.pet.primary_photo_cropped?.medium
    ? props.pet.primary_photo_cropped.medium
    : null;

  return (
    <Grid item key={props.pet.id}>
      <Link
        style={{ textDecoration: "none" }}
        href={{
          pathname: `/api/pets/${props.pet.id}`,
        }}
      >
        <CardActionArea onClick={handleClick}>
          <Card sx={{ maxWidth: "200px" }}>
            {img ? (
              <Image src={img} width={200} height={200} alt="Picture of pet" />
            ) : props.pet.type == "Cat" ? (
              <CatIcon sx={petIcons} />
            ) : (
              <DogIcon sx={petIcons} />
            )}
            <Typography sx={textStyles}>
              <b>{props.pet.name}</b>
              <br />
              <br />
              <b>Gender:</b> {props.pet.gender}
              <br />
              <b>Age:</b> {props.pet.age}
            </Typography>
          </Card>
        </CardActionArea>
      </Link>
    </Grid>
  );
}
