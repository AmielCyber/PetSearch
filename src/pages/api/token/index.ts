import type { NextApiRequest, NextApiResponse } from "next";

const CLIENT_ID = process.env.CLIENT_ID;
const CLIENT_SECRET = process.env.CLIENT_SECRET;
const PET_OATH_URL = "https://api.petfinder.com/v2/oauth2/token";

export default async function handler(req: NextApiRequest, res: NextApiResponse) {
  if (req.method === "GET") {
    if (!CLIENT_ID || !CLIENT_SECRET) {
      console.log("Process env variable not set up!");
      res.status(500).json({ message: "Server can not process request at the moment." });
      return;
    }

    // Set needed params for access token.
    const params = new URLSearchParams();
    params.append("grant_type", "client_credentials");
    params.append("client_id", CLIENT_ID);
    params.append("client_secret", CLIENT_SECRET);

    let response;
    let result;
    try {
      response = await fetch(PET_OATH_URL, {
        method: "POST",
        body: params,
      });
      result = await response.json();
      if (!response.ok || !result) {
        throw new Error();
      }
    } catch (e) {
      res.status(500).json({ message: "Failed to obtain access token." });
      return;
    }
    // Successful fetch.
    res.status(200).json(result);
    return;
  } else {
    res.status(403).json({ message: "Forbidden" });
    return;
  }
}
