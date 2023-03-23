import type { AppProps } from "next/app";
import { ThemeProvider } from "@mui/material/styles";
import theme from "@/utils/materialTheme";
import "@/styles/globals.css";
import NavBar from "@/components/layout/NavBar";
import Container from "@mui/material/Container";
import useToken from "@/hooks/useToken";

export default function App({ Component, pageProps }: AppProps) {
  useToken(); // Load up token on startup so we can search faster.
  return (
    <ThemeProvider theme={theme}>
      <NavBar />
      <Container maxWidth="lg">
        <Component {...pageProps} />
      </Container>
    </ThemeProvider>
  );
}
