import Head from "next/head";
import { memo } from "react";
function HeadMeta() {
  return (
    <Head>
      <meta name="viewport" content="width=device-width, initial-scale=1" />
      <link rel="icon" href="/favicon.ico" />
    </Head>
  );
}

export default memo(HeadMeta);
