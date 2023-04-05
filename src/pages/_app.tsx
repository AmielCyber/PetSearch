import type { AppProps } from "next/app";
import useMediaQuery from "@mui/material/useMediaQuery";
import { useMemo } from "react";
import { ThemeProvider } from "@mui/material/styles";
import CssBaseline from "@mui/material/CssBaseline";
import Container from "@mui/material/Container";
// Our imports.
import { darkModeTheme, lightModeTheme } from "@/utils/theme/material-theme";
import { LocationProvider } from "@/hooks/LocationContext";
import NavBar from "@/components/layout/NavBar";
import "@/styles/globals.css";

export default function App({ Component, pageProps }: AppProps) {
  const prefersDarkMode = useMediaQuery("(prefers-color-scheme: dark)");
  const theme = useMemo(() => (prefersDarkMode ? darkModeTheme : lightModeTheme), [prefersDarkMode]);

  return (
    <ThemeProvider theme={theme}>
      <CssBaseline />
      <LocationProvider>
        <NavBar />
        <Container maxWidth="lg">
          <Component {...pageProps} />
        </Container>
      </LocationProvider>
    </ThemeProvider>
  );
}
