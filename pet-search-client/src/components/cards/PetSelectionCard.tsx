import { Link as MuiLink, Paper, Typography } from "@mui/material";
import { Link } from "react-router-dom";

// Mui Styles
const petCards = {
  padding: "3rem",
  borderRadius: "30px",
  borderStyle: "solid",
  borderColor: "transparent",
  borderWidth: "4px",
  "&:hover": {
    borderColor: "#00a693",
  },
};
const petTextLabels = {
  fontSize: "2rem",
  color: "primary",
  textAlign: "center",
  paddingTop: "10px",
  textTransform: "capitalize",
};

type Props = {
  petType: string;
  location: string;
  children: React.ReactNode;
};

export default function PetSelectionCard(props: Props) {
  const linkPath = `/search/${props.petType}?location=${props.location}`;

  return (
    <MuiLink sx={{ textDecoration: "none" }} component={Link} to={linkPath}>
      <Paper sx={petCards} elevation={10}>
        {props.children}
        <Typography sx={petTextLabels}>{props.petType}</Typography>
      </Paper>
    </MuiLink>
  );
}
