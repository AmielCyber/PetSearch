import { useState, useMemo } from "react";
import {CssBaseline, Container, ThemeProvider, SxProps, Theme, responsiveFontSizes} from "@mui/material";
import {Outlet, ScrollRestoration} from "react-router-dom";
// Our imports.
import { darkModeTheme, lightModeTheme } from "./theme/materialTheme";
import { LocationProvider } from "./hooks/LocationContext";
import NavBar from "./components/layout/NavBar";

const containerSx: SxProps<Theme> = {
  paddingBottom: "2rem"
}

export default function App() {
  const [isDarkMode, setIsDarkMode] = useState(true);

  const handleToggleDarkMode = () => {
    setIsDarkMode((currIsDarkMode) => !currIsDarkMode);
  };

  const theme =
      useMemo(() => isDarkMode? responsiveFontSizes(darkModeTheme) : responsiveFontSizes(lightModeTheme), [isDarkMode]);

  return (
    <ThemeProvider theme={theme}>
      <CssBaseline />
      <LocationProvider>
        <NavBar isDarkMode={isDarkMode} onToggleDarkMode={handleToggleDarkMode} />
        <Container maxWidth="xl" sx={containerSx}>
          <ScrollRestoration />
          <Outlet />
        </Container>
      </LocationProvider>
    </ThemeProvider>
  );
}
