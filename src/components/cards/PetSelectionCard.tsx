import Paper from "@mui/material/Paper";
import Link from "next/link";
import Typography from "@mui/material/Typography";

type Props = {
  petType: string;
  location: string;
  children: React.ReactNode;
};

// Styles
const petCards = {
  padding: "3rem",
  borderRadius: "30px",
  "&:hover": {
    outlineColor: "#00a693",
    outlineStyle: "solid",
  },
};
const petTextLabels = {
  fontSize: "2rem",
  color: "#00a693",
  textAlign: "center",
  paddingTop: "10px",
  textTransform: "capitalize",
};

// May change the pathname to support dynamic routing: /search/cats?.... /search/dogs?....

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
