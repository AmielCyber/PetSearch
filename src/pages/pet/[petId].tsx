import { NextRouter, useRouter } from "next/router";
import DisplayInfo from "@/components/pet/DisplayInfo";

function getId(router: NextRouter): number {
  const id = router.query?.petId;
  if (typeof id !== "string") {
    return 0;
  }
  if (Number.isNaN(id)) {
    return 0;
  }
  return parseInt(id);
}

export default function PetPage() {
  const router = useRouter();
  const id = getId(router);
  if (id === 0) {
    return <p>Display error message here.</p>;
  }
  return <DisplayInfo id={id} />;
}
