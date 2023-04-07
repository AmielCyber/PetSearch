import Head from "next/head";

export default function PetPageMeta() {
  return (
    <Head>
      <title>Pet Information</title>
      <meta
        name="description"
        content="Check if this pet is a great fit for you. Click the PetFinder link to view more information."
      />
    </Head>
  );
}
