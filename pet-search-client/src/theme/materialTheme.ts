import { createTheme } from "@mui/material";

export const lightModeTheme = createTheme({
  palette: {
    mode: "light",
    primary: {
      main: "#009680",
      light: "#4db6a4",
      dark: "#004d3a",
      contrastText: "#FFF",
    },
    secondary: {
      main: "#c3212d",
      light: "#ca6469",
      dark: "#960017",
    },
    info: {
      main: "#22b4ec",
      light: "#5bcbef",
      dark: "#006296",
    },
    success: {
      main: "#17a840",
      light: "#4fc366",
      dark: "#008529",
    },
  },
});

export const darkModeTheme = createTheme({
  palette: {
    mode: "dark",
    primary: {
      main: "#b2dfd7",
      light: "#e0f2f0",
      dark: "#80cbbe",
    },
    secondary: {
      main: "#f3c6cc",
      light: "#fae8eb",
      dark: "#db8e92",
    },
    info: {
      main: "#b6e9f7",
      light: "#e2f7fc",
      dark: "#87daf3",
    },
    success: {
      main: "#c4e9c9",
      light: "#e6f6e9",
      dark: "#9ddba6",
    },
    text: {
      primary: "#80cbbe",
    },
  },
});
