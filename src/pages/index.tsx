import Head from "next/head";
import { Inter } from "next/font/google";
import Box from "@mui/material/Box";
import Typography from "@mui/material/Typography";
// Our imports.
import CatIcon from "@/components/icons/CatIcon";
import DogIcon from "@/components/icons/DogIcon";
import PetSelectionCard from "@/components/cards/PetSelectionCard";

const inter = Inter({ subsets: ["latin"] });

// Will implement location with useContext later.
const LOCATION = "92101";

// Styles
const petIcons = {
  fontSize: "150px",
  color: "#212427",
};

const petCardBox = {
  display: "flex",
  flexWrap: "wrap",
  justifyContent: "center",
  gap: "2rem",
};

const titleStyles = {
  color: "#212427",
  textAlign: "center",
  "margin-bottom": "40px",
  marginTop: "40px",
};

export default function Home() {
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
          <PetSelectionCard petType="cats" location={LOCATION}>
            <CatIcon sx={petIcons} />
          </PetSelectionCard>
          <PetSelectionCard petType="dogs" location={LOCATION}>
            <DogIcon sx={petIcons} />
          </PetSelectionCard>
        </Box>
      </main>
    </>
  );
}
