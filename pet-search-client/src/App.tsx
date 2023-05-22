import { useState, useMemo } from "react";
import {CssBaseline, Container, ThemeProvider, SxProps, Theme} from "@mui/material";
import { Outlet } from "react-router-dom";
// Our imports.
import { darkModeTheme, lightModeTheme } from "./theme/materialTheme";
import { LocationProvider } from "./hooks/LocationContext";
import NavBar from "./components/layout/NavBar";

const containerSx: SxProps<Theme> ={
  paddingBottom: "2rem"
}

export default function App() {
  const [isDarkMode, setIsDarkMode] = useState(true);

  const handleToggleDarkMode = () => {
    setIsDarkMode((currIsDarkMode) => !currIsDarkMode);
  };

  const theme = useMemo(() => (isDarkMode ? darkModeTheme : lightModeTheme), [isDarkMode]);

  return (
    <ThemeProvider theme={theme}>
      <CssBaseline />
      <LocationProvider>
        <NavBar isDarkMode={isDarkMode} onToggleDarkMode={handleToggleDarkMode} />
        <Container maxWidth="xl" sx={containerSx}>
          <Outlet />
        </Container>
      </LocationProvider>
    </ThemeProvider>
  );
}
