import Head from "next/head";
import Box from "@mui/material/Box";
import Typography from "@mui/material/Typography";
// Our imports.
import type { LocationContextType } from "@/hooks/LocationContext";
import { useContext } from "react";
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
        <title>Create Next App</title>
        <meta name="description" content="Search a pet to adopt!" />
        <meta name="viewport" content="width=device-width, initial-scale=1" />
        <link rel="icon" href="/favicon.ico" />
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
