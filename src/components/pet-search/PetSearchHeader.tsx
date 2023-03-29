import Typography from "@mui/material/Typography";
const titleStyles = {
  color: "#212427",
  textAlign: "center",
  "margin-bottom": "40px",
  marginTop: "40px",
};
type Props = {
  petType: string;
  zipCode: string;
};
export default function PetSearchHeader(props: Props) {
  const upperCasePetType = props.petType.slice(0, 1).toUpperCase() + props.petType.slice(1);
  return (
    <Typography sx={titleStyles} variant="h2">
      {upperCasePetType}s close to zip code: {props.zipCode}
    </Typography>
  );
}
