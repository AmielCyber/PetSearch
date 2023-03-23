import Paper from "@mui/material/Paper";
import Link from "next/link";

type Props = {
  petType: string;
  location: string;
  children: React.ReactNode;
};

// May change the pathname to support dynamic routing: /search/cats?.... /search/dogs?....

export default function PetSelectionCard(props: Props) {
  return (
    <Paper elevation={10} sx={{ padding: "2rem", borderRadius: "30px" }}>
      <Link href={{ pathname: "/search/", query: { petType: props.petType, location: props.location } }}>
        {props.children}
      </Link>
    </Paper>
  );
}
