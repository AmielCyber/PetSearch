import { Suspense, lazy } from "react";
import {useLocation, useNavigate, useParams} from "react-router-dom";
import {CircularProgress, Alert, Button} from "@mui/material";
import ArrowBackIcon from "@mui/icons-material/ArrowBack";
// Our import.
const DisplayInfo = lazy(() => import("../components/pet/DisplayInfo"));

export default function PetPage() {
  const params = useParams();
  const {state} = useLocation();
  const navigate = useNavigate();
  const id = params.id ?? "error";

  if (id === "error" || Number.isNaN(parseInt(id))) {
    return (
      <main>
        <Alert severity="error">Invalid pet id entered.</Alert>
      </main>
    );
  }
  return (
    <main>
      <Suspense fallback={<CircularProgress sx={{margin: "auto"}}/>}>
        {state?.fromSearch &&  <Button startIcon={<ArrowBackIcon />} onClick={() => navigate(-1)}>Back to search results</Button>}
        <DisplayInfo id={id} />
      </Suspense>
    </main>
  );
}
