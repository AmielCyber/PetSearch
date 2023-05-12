import { Suspense, lazy } from "react";
import { useParams } from "react-router-dom";
import { CircularProgress, Alert } from "@mui/material";
// Our import.
const DisplayInfo = lazy(() => import("../components/pet/DisplayInfo"));

export default function PetPage() {
  const params = useParams();
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
      {id !== "" ? (
        <Suspense fallback={<CircularProgress />}>
          <DisplayInfo id={id} />{" "}
        </Suspense>
      ) : null}
    </main>
  );
}
