import type { NextApiRequest, NextApiResponse } from "next";

const PETS = ["cats", "dogs"];
const PET_FINDER_URL = "https://api.petfinder.com/v2/animals?";

//"Authorization: Bearer {YOUR_ACCESS_TOKEN}" GET https://api.petfinder.com/v2/{CATEGORY}/{ACTION}?{parameter_1}={value_1}&{parameter_2}={value_2}
export default async function handler(req: NextApiRequest, res: NextApiResponse) {
  if (req.method === "GET") {
    // Will validate later.
    // Access token from the client.
    const accessToken = req.headers.authorization;
    if (!accessToken) {
      res.status(400).json({ message: "Invalid token passed." });
      return;
    }
    let page, location, petType;
    //{ page, location } = req.query;
    // Needs validation
    petType = "cat";
    page = "1";
    location = "92101";

    const searchQuery = new URLSearchParams();
    searchQuery.append("type", petType);
    searchQuery.append("page", page);
    searchQuery.append("location", location);

    let response;
    let result;
    try {
      response = await fetch(PET_FINDER_URL + searchQuery.toString(), {
        headers: {
          Authorization: accessToken,
        },
      });
      result = await response.json();
      if (!response.ok) {
        throw new Error();
      }
    } catch (e) {
      res.status(500).json({ message: "Something went wrong...." });
    }
    res.status(200).json(result);
    return;
  }
  res.status(403).json({ message: "Forbidden" });
  return;
}
