import Paper from "@mui/material/Paper";
import Link from "next/link";
import Typography from "@mui/material/Typography";
import styles from "@/styles/layout/PetSelectionCards.module.css";

type Props = {
  petType: string;
  location: string;
  children: React.ReactNode;
};

// May change the pathname to support dynamic routing: /search/cats?.... /search/dogs?....

export default function PetSelectionCard(props: Props) {
  return (
    <Paper className={styles.petCards} elevation={10}>
      <Link
        href={{
          pathname: "/search/",
          query: { petType: props.petType, location: props.location },
        }}
        className={styles.petIcons}
      >
        {props.children}
      </Link>
      <Typography className={styles.petTextLabels}>{props.petType}</Typography>
    </Paper>
  );
}
