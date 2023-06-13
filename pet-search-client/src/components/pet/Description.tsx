import { Typography, Link as MaterialLink } from "@mui/material";

type Props = {
  description: string | null;
  url: string;
};

function decodeHtmlEntityString(encodedStr: string | null): string {
  if (!encodedStr) {
    return "No description provided.";
  }
  const tempElement = document.createElement("div");
  tempElement.innerHTML = encodedStr;
  return tempElement.textContent as string;
}

export default function Description(props: Props) {
  const decodedDescription = decodeHtmlEntityString(props.description);

  return (
    <section>
      <Typography variant="h4">Description</Typography>
      <Typography variant="body1">{decodedDescription}</Typography>

      <MaterialLink href={props.url}>
        More at PetFinder.com.
      </MaterialLink>
    </section>
  );
}
