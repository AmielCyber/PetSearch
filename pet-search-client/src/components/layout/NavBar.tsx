import {useContext, useState} from "react";
import { AppBar, Box, Button, IconButton, Toolbar, Typography } from "@mui/material";
import {Link, useSearchParams} from "react-router-dom";
import PetsIcon from '@mui/icons-material/Pets';
import DarkModeIcon from "@mui/icons-material/DarkMode";
import LightModeIcon from "@mui/icons-material/LightMode";
import NearMeIcon from "@mui/icons-material/NearMe";
import {enqueueSnackbar} from "notistack";
// Our imports.
import type {LocationResponse} from "./fetchLocation.ts";
import {LocationContext, LocationContextType} from "../../hooks/LocationContext.tsx";
import LocationButton from "./LocationButton";
import {getLocationFromZipCode, getUserLocation} from "./fetchLocation.ts";

function getNewSearchParamsLocation(zipcode: string, searchParams: URLSearchParams): URLSearchParams{
  const newParams = new URLSearchParams(searchParams);
  if (searchParams.has("page")) {
    newParams.set("page", "1");
  }
  newParams.set("location", zipcode);

  return newParams;
}

type Props = {
  isDarkMode: boolean;
  onToggleDarkMode: VoidFunction;
};

export default function MainNavigation(props: Props) {
  const [loadingNewLocation, setLoadingNewLocation] = useState(false);
  const [located, setLocated] = useState(false);
  const {location,setLocation} = useContext(LocationContext) as LocationContextType;
  const [searchParams, setSearchParams] = useSearchParams();

  const toggleDarkModeTitle = props.isDarkMode? "Light Mode" : "Dark Mode";

  const handleZipCodeChange = async (newZipcode: string) => {
    setLoadingNewLocation(true);
    const locationResponse: LocationResponse = await getLocationFromZipCode(newZipcode)

    if(locationResponse.location !== undefined){
      setLocation(locationResponse.location);
      setSearchParams(getNewSearchParamsLocation(locationResponse.location.zipcode, searchParams))
      enqueueSnackbar("Updated zipcode!", {variant: "success"})
    }else {
      enqueueSnackbar(locationResponse.errorMessage ?? "Failed to update zipcode.", {variant: "error"});
    }

    setLoadingNewLocation(false);
  }

  const onLocateMe = async () => {
    if(loadingNewLocation){
      return;
    }
    if(located){
      enqueueSnackbar("Already located.", {variant: "info"});
      return;
    }
    setLoadingNewLocation(true);
    const locationResponse: LocationResponse = await getUserLocation();

    if(locationResponse.location !== undefined){
      setLocation(locationResponse.location);
      setSearchParams(getNewSearchParamsLocation(locationResponse.location.zipcode, searchParams))
      enqueueSnackbar("Updated zipcode!", {variant: "success"})
      setLocated(true);
    }else {
      enqueueSnackbar(locationResponse.errorMessage ?? "Failed to retrieve location.", {variant: "error"});
    }

    setLoadingNewLocation(false);
  }

  return (
    <Box sx={{ flexGrow: 1 }}>
      <AppBar position="static">
        <Toolbar>
          <Typography variant="h6" component="div" sx={{ flexGrow: 1 }}>
            <Button component={Link} to="/" color="inherit" startIcon={<PetsIcon />}>
              Pet Search
            </Button>
          </Typography>
          <LocationButton location={location} loadingNewZipcode={loadingNewLocation} onZipcodeChange={handleZipCodeChange}/>
          <IconButton color="inherit" title="Locate Me!" onClick={onLocateMe}>
            <NearMeIcon />
          </IconButton>
          <IconButton onClick={props.onToggleDarkMode} color="inherit" title={toggleDarkModeTitle}>
            {props.isDarkMode ? <LightModeIcon /> : <DarkModeIcon />}
          </IconButton>
        </Toolbar>
      </AppBar>
    </Box>
  );
}
