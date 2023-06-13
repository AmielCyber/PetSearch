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

  return (
    <>
      <Typography sx={titleStyles} variant="h2">
        Adoptable {props.petType}s within 50 mile{props.petType.length > 1? "s" : ""}.
      </Typography>
      <Typography sx={zipStyles} variant="subtitle1">
        Zip Code: {props.zipCode}
      </Typography>
    </>
  );
}
