import Typography from "@mui/material/Typography";
import Link from "next/link";

type Props = {
  description: string | null;
  url: string;
};

export default function Description(props: Props) {
  return (
    <section>
      <Typography variant="h4">Description</Typography>
      <Typography variant="body1">{props.description ? props.description : "No description provided."}</Typography>
      <Link href={props.url}>Click Here For More Info at PetFinder.</Link>
    </section>
  );
}
