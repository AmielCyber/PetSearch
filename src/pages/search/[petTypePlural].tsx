import { GetServerSideProps, InferGetStaticPropsType } from "next";
import DisplaySearch from "@/components/pet-search/DisplaySearch";

const petSet = new Set(["dogs", "cats"]);

type Props = {
  petTypePlural: string;
  petType: string;
  invalidPetType: boolean;
  page: string;
  location: string;
};

export const getServerSideProps: GetServerSideProps<Props> = async (context) => {
  const petTypePlural = context.params?.petTypePlural as string;
  const page = context.query.page ? (context.query.page as string) : "1";
  const location = context.query.location ? (context.query.location as string) : "92101";
  if (!petSet.has(petTypePlural)) {
    return {
      props: {
        petTypePlural: petTypePlural,
        petType: petTypePlural,
        invalidPetType: true,
        page: "1",
        location: location,
      },
    };
  }

  return {
    props: {
      petTypePlural: petTypePlural,
      petType: petTypePlural.slice(0, petTypePlural.length - 1),
      invalidPetType: false,
      page,
      location: location,
    },
  };
};

export default function PetSearchPage(props: Props) {
  if (props.invalidPetType) {
    return <p>Pet Type: {props.petType} not supported.</p>;
  }

  const params = new URLSearchParams();
  params.append("petType", props.petType);
  params.append("page", props.page);
  params.append("location", props.location);

  return <DisplaySearch petTypePlural={props.petTypePlural} searchParams={params} />;
}
