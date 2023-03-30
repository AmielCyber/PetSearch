import { useRouter } from "next/router";
import { useContext } from "react";
import AppBar from "@mui/material/AppBar";
import Box from "@mui/material/Box";
import Toolbar from "@mui/material/Toolbar";
import Typography from "@mui/material/Typography";
import Link from "next/link";
// Our components.
import type { LocationContextType } from "@/hooks/LocationContext";
import { LocationContext } from "@/hooks/LocationContext";
import LocationButton from "./LocationButton";

export default function MainNavigation() {
  const router = useRouter();
  const { zipCode, setZipCode } = useContext(LocationContext) as LocationContextType;

  const handleZipCodeChange = (newZipCode: string) => {
    setZipCode(newZipCode);

    // Set the new zip code in our search query URL.
    const hasPage = router.query.page ? true : false;
    if (hasPage) {
      // Set page to 1 to restart search with new query.
      router.push({
        query: {
          ...router.query,
          page: "1",
          location: newZipCode,
        },
      });
    } else {
      router.push({
        query: {
          ...router.query,
          location: newZipCode,
        },
      });
    }
  };

  return (
    <Box sx={{ flexGrow: 1 }}>
      <AppBar position="static">
        <Toolbar>
          <Typography variant="h6" component="div" sx={{ flexGrow: 1 }}>
            <Link href={"/"} style={{ color: "inherit", textDecoration: "none" }}>
              PetSearch
            </Link>
          </Typography>
          <LocationButton onZipCodeChange={handleZipCodeChange} currentZip={zipCode} />
        </Toolbar>
      </AppBar>
    </Box>
  );
}
