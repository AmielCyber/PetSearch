import Typography from "@mui/material/Typography";

const titleStyles = {
  textAlign: "center",
  marginBottom: "1rem",
  marginTop: "2rem",
};
const zipStyles = {
  textAlign: "center",
  marginBottom: "20px",
};

type Props = {
  petType: string;
  zipCode: string;
};

export default function PetSearchHeader(props: Props) {
  const upperCasePetType = props.petType.slice(0, 1).toUpperCase() + props.petType.slice(1);

  return (
    <>
      <Typography sx={titleStyles} variant="h2">
        {upperCasePetType} within a 50 mile radius.
      </Typography>
      <Typography sx={zipStyles} variant="subtitle1">
        Adoptable {props.petType} around Zip Code: {props.zipCode}
      </Typography>
    </>
  );
}
