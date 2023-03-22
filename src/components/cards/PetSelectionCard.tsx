import Paper from "@mui/material/Paper";
import Link from "next/link";
import CatIcon from "@/components/icons/CatIcon";
type Props = {
  petUrl: string;
  children: React.ReactNode;
};
export default function PetSelectionCard(props: Props) {
  return (
    <Paper elevation={10} sx={{ padding: "2rem", borderRadius: "30px" }}>
      <Link href={props.petUrl}>{props.children}</Link>
    </Paper>
  );
}
