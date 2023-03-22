import Head from "next/head";
import { Inter } from "next/font/google";
import Box from "@mui/material/Box";
import Paper from "@mui/material/Paper";
import Typography from "@mui/material/Typography";
import Link from "next/link";
// Our imports.
import styles from "@/styles/Home.module.css";
import CatIcon from "@/components/icons/CatIcon";
import DogIcon from "@/components/icons/DogIcon";
import PetSelectionCard from "@/components/cards/PetSelectionCard";

const inter = Inter({ subsets: ["latin"] });

export default function Home() {
  return (
    <>
      <Head>
        <title>Create Next App</title>
        <meta name="description" content="Search a pet to adopt!" />
        <meta name="viewport" content="width=device-width, initial-scale=1" />
        <link rel="icon" href="/favicon.ico" />
      </Head>
      <main className={styles.main}>
        <Typography sx={{ textAlign: "center", marginBottom: "2rem" }} variant="h2">
          Find Your Fur Ever Friend!
        </Typography>
        <Box
          sx={{
            display: "flex",
            flexWrap: "wrap",
            justifyContent: "center",
            gap: "1rem",
          }}
        >
          <PetSelectionCard petUrl="/cats">
            <CatIcon sx={{ fontSize: 180 }} />
          </PetSelectionCard>
          <PetSelectionCard petUrl="/dogs">
            <DogIcon sx={{ fontSize: 180 }} />
          </PetSelectionCard>
        </Box>
      </main>
    </>
  );
}
