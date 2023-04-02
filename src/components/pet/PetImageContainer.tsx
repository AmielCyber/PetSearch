import Image from "next/image";
import { PhotoSize } from "@/models/Pet";
import Link from "next/link";
import { Button } from "@mui/material";

type Props = {
  name: string;
  photos: PhotoSize[];
};
export default function PetImageContainer(props: Props) {
  if (props.photos.length === 0) {
    return <div>lol</div>;
  }
  const handleClick = () => {
    console.log("HELLO");
  };
  return (
    <div>
      <Image src={props.photos[0].large} alt={props.name} width={300} height={300} />
      <Link href={"/"} legacyBehavior>
        <Button onClick={handleClick}>Home</Button>
      </Link>
    </div>
  );
}
