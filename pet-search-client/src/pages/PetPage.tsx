import {useLocation, useNavigate, useParams} from "react-router-dom";
import {Alert, Button} from "@mui/material";
import ArrowBackIcon from "@mui/icons-material/ArrowBack";
import DisplayInfo from "../components/pet/DisplayInfo.tsx";
// Our import.

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
        {state?.fromSearch &&  <Button startIcon={<ArrowBackIcon />} onClick={() => navigate(-1)} size="large">Back to search results</Button>}
        <DisplayInfo id={id} />
    </main>
  );
}
