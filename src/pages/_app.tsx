import type { AppProps } from "next/app";
import "@/styles/globals.css";
import NavBar from "@/components/layout/NavBar";

export default function App({ Component, pageProps }: AppProps) {
  return (
    <>
      <NavBar />
      <Component {...pageProps} />
    </>
  );
}
