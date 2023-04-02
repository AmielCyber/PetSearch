type Props = {
  name: string;
};
export default function PetTitle(props: Props) {
  return (
    <div>
      <h2>{props.name}</h2>
    </div>
  );
}
