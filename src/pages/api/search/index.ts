import type { NextApiRequest, NextApiResponse } from "next";
import type Pet from "@/models/Pet";
import PetResponse from "@/models/PetResponse";

const PETS = ["cat", "dog"];
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

    let response, result;
    let petResponse: PetResponse | null = null;
    try {
      response = await fetch(PET_FINDER_URL + searchQuery.toString(), {
        headers: {
          Authorization: accessToken,
        },
      });
      result = await response.json();
      // Removes sensitive data so we do not handle sensitive data
      // We will use the URL for a person to find more about getting a pet.
      const filteredPets: Pet[] = result.animals.map((pet: any) => ({
        id: pet.id,
        url: pet.url,
        type: pet.type,
        age: pet.age,
        gender: pet.gender,
        size: pet.size,
        name: pet.name,
        description: pet.description,
        photos: pet.photos,
        primary_photo_cropped: pet.primary_photo_cropped,
        status: pet.status,
      }));
      petResponse = {
        pets: filteredPets,
        pagination: result.pagination,
      };
      if (!response.ok) {
        throw new Error();
      }
    } catch (e) {
      res.status(500).json({ message: "Something went wrong...." });
    }
    console.log(result);
    res.status(200).json(petResponse);
    return;
  }
  res.status(403).json({ message: "Forbidden" });
  return;
}
