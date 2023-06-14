import {Card, Typography, Grid, CardActionArea, Link as MuiLink } from "@mui/material";
import { useSWRConfig } from "swr";
import { Link } from "react-router-dom";
// Our imports.
import type Pet from "../../models/pet.ts";
import styles from "../../styles/cards/PetListCard.module.css";
import CatIcon from "../icons/CatIcon";
import DogIcon from "../icons/DogIcon";

type Props = {
  pet: Pet;
};

// Styles
const petIcons = {
  fontSize: "300px",
  color: "primary",
};
const textStyles = {
  marginBottom: "16px",
  textAlign: "center"
};
const headingStyles = {
  marginBottom: "6px",
  textAlign: "center"
};

export default function PetListCard(props: Props) {
  const { mutate } = useSWRConfig();

  async function handleClick() {
    await mutate(`pets/${props.pet.id}`, props.pet);
  }
  // Temporarily hard coding to medium-sized img, icon otherwise
  const img = props.pet.primary_photo_cropped?.small ?? null;
  const distance = props.pet.distance? props.pet.distance.toFixed(1) : "0";
  let distanceMsg = "";
  if(distance !== "0"){
    distanceMsg = `${distance} miles away`;
  }

  return (
    <Grid item key={props.pet.id}>
      <MuiLink
      component={Link}
        style={{ textDecoration: "none" }}
        to={`/pets/${props.pet.id}`}
        state={{fromSearch: true}}
        preventScrollReset={false}
      >
        <CardActionArea onClick={handleClick}>
          <Card sx={{ maxWidth: "300px" }} elevation={3}>
            {img ? (
                <div className={styles.imageContainer}>
                  <img src={img} alt={props.pet.name} loading="lazy" />
                </div>
            ) : props.pet.type == "Cat" ? (
              <CatIcon sx={petIcons} />
            ) : (
              <DogIcon sx={petIcons} />
            )}
            <Typography variant="h6" noWrap sx={headingStyles}>{props.pet.name}</Typography>
            <Typography sx={textStyles}>
              {props.pet.age} {props.pet.gender}
              <br />
              {distanceMsg}
            </Typography>
          </Card>
        </CardActionArea>
      </MuiLink>
    </Grid>
  );
}
