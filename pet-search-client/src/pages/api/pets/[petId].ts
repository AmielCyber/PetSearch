import type { NextApiRequest, NextApiResponse } from "next";
import type Pet from "@/models/Pet";

export default async function handler(req: NextApiRequest, res: NextApiResponse) {
  if (req.method === "GET") {
    // Check for client's token
    const accessToken = req.headers.authorization as string;
    if (!accessToken) {
      res.status(401).json({ message: "Token needed to access endpoint." });
      return;
    }

    // Get id from search query params and validate.
    const id = req.query.petId as string;
    if (Number.isNaN(id)) {
      res.status(400).json({ message: "Pet id must be a number." });
      return;
    }

    // Get single pet if there is one.
    const petFinderURL = new URL(`https://api.petfinder.com/v2/animals/${id}`);
    let response: Response;
    let result;
    let petResult: Pet;
    let filteredPets: Pet;
    try {
      // Fetch data.
      response = await fetch(petFinderURL, {
        headers: {
          Authorization: accessToken,
        },
      });
      if (!response) {
        throw new Error();
      }
      if (response?.status === 404) {
        res.status(404).json({ message: `Pet id '${id}' cannot be found or pet has been adopted.` });
        return;
      }
      if (!response?.ok) {
        throw new Error();
      }

      result = await response.json();
      petResult = result.animal;
      // Removes sensitive data so we do not handle sensitive data
      // We will use the URL for a person to find more about getting a pet.
      filteredPets = {
        id: petResult.id,
        url: petResult.url,
        type: petResult.type,
        age: petResult.age,
        gender: petResult.gender,
        size: petResult.size,
        name: petResult.name,
        description: petResult.description,
        photos: petResult.photos,
        primary_photo_cropped: null,
        status: petResult.status,
        distance: null,
      };
    } catch (e) {
      res.status(500).json({ message: "Something went wrong..." });
      return;
    }
    res.status(200).json(filteredPets);
    return;
  }
  res.status(400).json({ message: "HTTP method not supported." });
}
