import Link from "next/link";
import Paper from "@mui/material/Paper";
import Typography from "@mui/material/Typography";

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
  return (
    <Link
      style={{ textDecoration: "none" }}
      href={{
        pathname: `/search/${props.petType}`,
        query: { location: props.location },
      }}
    >
      <Paper sx={petCards} elevation={10}>
        {props.children}
        <Typography sx={petTextLabels}>{props.petType}</Typography>
      </Paper>
    </Link>
  );
}
