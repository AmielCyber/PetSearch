import { useRouter } from "next/router";
import Typography from "@mui/material/Typography";
import LinearProgress from "@mui/material/LinearProgress";
import Alert from "@mui/material/Alert";
// Out imports.
import fetchPets from "@/hooks/fetch-pets";

export default function AvailablePets() {
  const { pets, isLoading, isError } = fetchPets("/api/search");
  const router = useRouter();

  if (isLoading) {
    return <LinearProgress />;
  }
  if (isError) {
    return <Alert severity="error">Something went wrong...</Alert>;
  }

  // May change the query parameters later.
  let petType = router.query.petType as string;
  const location = router.query.location as string;
  petType = petType.slice(0, 1).toUpperCase() + petType.slice(1);

  return (
    <Typography sx={{ textAlign: "center", marginBottom: "2rem" }} variant="h2">
      {petType} in zip code: {location}
    </Typography>
  );
}
