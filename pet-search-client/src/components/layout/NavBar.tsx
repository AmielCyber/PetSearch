import { useSearchParams } from "react-router-dom";
import { useContext } from "react";
import { AppBar, Box, Button, IconButton, Toolbar, Typography } from "@mui/material";
import { Link } from "react-router-dom";
import DarkModeIcon from "@mui/icons-material/DarkMode";
import LightModeIcon from "@mui/icons-material/LightMode";
// Our imports.
import type { LocationContextType } from "../../hooks/LocationContext";
import { LocationContext } from "../../hooks/LocationContext";
import LocationButton from "./LocationButton";

type Props = {
  isDarkMode: boolean;
  onToggleDarkMode: VoidFunction;
};

export default function MainNavigation(props: Props) {
  const [searchParams, setSearchParams] = useSearchParams();
  const { zipCode, setZipCode } = useContext(LocationContext) as LocationContextType;

  const handleZipCodeChange = (newZipCode: string) => {
    setZipCode(newZipCode);
    // Set the new zip code in our search query URL.
    const newParams = new URLSearchParams(searchParams);
    if (searchParams.has("page")) {
      // Set page to 1 to restart search with new query.
      newParams.set("page", "1");
    }
    newParams.set("location", newZipCode);
    setSearchParams(newParams);
  };

  return (
    <Box sx={{ flexGrow: 1 }}>
      <AppBar position="static">
        <Toolbar>
          <Typography variant="h6" component="div" sx={{ flexGrow: 1 }}>
            <Button component={Link} to="/" color="inherit" sx={{ textDecoration: "none" }}>
              PetSearch
            </Button>
          </Typography>
          <LocationButton onZipCodeChange={handleZipCodeChange} currentZip={zipCode} />
          <IconButton onClick={props.onToggleDarkMode} color="inherit">
            {props.isDarkMode ? <LightModeIcon /> : <DarkModeIcon />}
          </IconButton>
        </Toolbar>
      </AppBar>
    </Box>
  );
}
