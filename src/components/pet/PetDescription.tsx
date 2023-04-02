import Pet from "@/models/Pet";
type Props = {
  petData: Pet;
};
export default function PetDescription(props: Props) {
  return (
    <div>
      <p>Age: {props.petData.age}</p>
      <p>Size: {props.petData.size}</p>
      <p>Gender: {props.petData.gender}</p>
      <p>Status: {props.petData.status}</p>
      <h3>Description</h3>
      <p>{props.petData.description}</p>
    </div>
  );
}
