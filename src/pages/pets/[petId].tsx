import { NextRouter, useRouter } from "next/router";
import Alert from "@mui/material/Alert";
// Our component.
import DisplayInfo from "@/components/pet/DisplayInfo";

function getId(router: NextRouter): string {
  const id = router.query?.petId;
  if (typeof id !== "string") {
    return "error";
  }
  if (Number.isNaN(parseInt(id))) {
    return "error";
  }
  return id;
}

export default function PetPage() {
  const router = useRouter();
  const id = getId(router);

  if (id === "error") {
    return (
      <main>
        <Alert severity="error">Invalid pet id entered.</Alert>
      </main>
    );
  }
  return <main>{id !== "" ? <DisplayInfo id={id} /> : null}</main>;
}
