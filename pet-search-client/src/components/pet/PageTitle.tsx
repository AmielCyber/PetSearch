import { Typography } from "@mui/material";

type Props = {
  name: string;
};
export default function PageTitle(props: Props) {
  return <Typography variant="h2">{props.name}</Typography>;
}
