import { useContext } from "react";
import { Box, Typography } from "@mui/material";
// Our imports.
import type { LocationContextType } from "../hooks/LocationContext";
import { LocationContext } from "../hooks/LocationContext";
import PetSelectionCard from "../components/cards/PetSelectionCard";
import CatIcon from "../components/icons/CatIcon";
import DogIcon from "../components/icons/DogIcon";

// Styles
const petIcons = {
  fontSize: "150px",
};

const petCardBox = {
  display: "flex",
  flexWrap: "wrap",
  justifyContent: "center",
  gap: "2rem",
};

const titleStyles = {
  textAlign: "center",
  marginBottom: "40px",
  marginTop: "40px",
};

export default function Home() {
  const { zipCode } = useContext(LocationContext) as LocationContextType;

  return (
      <main>
        <Typography sx={titleStyles} variant="h2">
          Find your perfect furry companion!
        </Typography>
        <Box sx={petCardBox}>
          <PetSelectionCard petType="cats" location={zipCode}>
            <CatIcon sx={petIcons} />
          </PetSelectionCard>
          <PetSelectionCard petType="dogs" location={zipCode}>
            <DogIcon sx={petIcons} />
          </PetSelectionCard>
        </Box>
      </main>
  );
}
