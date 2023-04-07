import { useContext } from "react";
import Head from "next/head";
import Typography from "@mui/material/Typography";
import Box from "@mui/material/Box";
// Our imports.
import type { LocationContextType } from "@/hooks/LocationContext";
import { LocationContext } from "@/hooks/LocationContext";
import PetSelectionCard from "@/components/cards/PetSelectionCard";
import CatIcon from "@/components/icons/CatIcon";
import DogIcon from "@/components/icons/DogIcon";

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
    <>
      <Head>
        <title>Pet Search</title>
        <meta name="description" content="Search a pet in your area to adopt!" />
      </Head>
      <main>
        <Typography sx={titleStyles} variant="h2">
          Find Your Fur Ever Friend!
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
    </>
  );
}
