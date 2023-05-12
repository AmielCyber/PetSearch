import type { AppProps } from "next/app";
import { useMemo, useState } from "react";
import { ThemeProvider } from "@mui/material/styles";
import CssBaseline from "@mui/material/CssBaseline";
import Container from "@mui/material/Container";
// Our imports.
import { darkModeTheme, lightModeTheme } from "@/utils/theme/material-theme";
import { LocationProvider } from "@/hooks/LocationContext";
import NavBar from "@/components/layout/NavBar";
import HeadMeta from "@/components/meta/HeadMeta";
import "@/styles/globals.css";

export default function App({ Component, pageProps }: AppProps) {
  const [isDarkMode, setIsDarkMode] = useState(true);

  const handleToggleDarkMode = () => {
    setIsDarkMode((currIsDarkMode) => !currIsDarkMode);
  };

  const theme = useMemo(() => (isDarkMode ? darkModeTheme : lightModeTheme), [isDarkMode]);

  return (
    <ThemeProvider theme={theme}>
      <CssBaseline />
      <LocationProvider>
        <HeadMeta />
        <NavBar isDarkMode={isDarkMode} onToggleDarkMode={handleToggleDarkMode} />
        <Container maxWidth="lg">
          <Component {...pageProps} />
        </Container>
      </LocationProvider>
    </ThemeProvider>
  );
}
