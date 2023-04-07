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
        <meta
          name="description"
          content="Search adoptable pets in your area!"
        />
        <link
          rel="apple-touch-icon"
          sizes="180x180"
          href="/apple-touch-icon.png"
        />
        <link
          rel="icon"
          type="image/png"
          sizes="32x32"
          href="/favicon-32x32.png"
        />
        <link
          rel="icon"
          type="image/png"
          sizes="16x16"
          href="/favicon-16x16.png"
        />
        <link rel="manifest" href="/site.webmanifest" />
        <meta name="msapplication-TileColor" content="#da532c" />
        <meta name="theme-color" content="#ffffff" />
      </Head>
      <main>
        <Typography sx={titleStyles} variant="h2">
          Find your perfect furry companion today!
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
