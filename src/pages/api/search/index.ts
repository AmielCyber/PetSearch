import type { NextApiRequest, NextApiResponse } from "next";
import { SafeParseReturnType } from "zod";
// Our imports.
import type Pet from "@/models/Pet";
import type PetResponse from "@/models/PetResponse";
import { petQuery, validateQuery } from "@/utils/db/query-validation";

export default async function handler(req: NextApiRequest, res: NextApiResponse) {
  if (req.method === "GET") {
    // Check for client's token
    const accessToken = req.headers.authorization;
    if (!accessToken) {
      res.status(401).json({ message: "Token needed to access endpoint." });
      return;
    }

    // Get search query params.
    let petType = req.query.petType as string;
    let page = req.query.page as string;
    let location = req.query.location as string;
    // Pre-defined undefined types right now until we have our front-end done.
    petType = petType ? petType : "cat";
    page = page ? page : "1";
    location = location ? location : "92101";

    // Validate search params
    const searchQueryValidation: SafeParseReturnType<petQuery, petQuery> = validateQuery(petType, page);
    if (!searchQueryValidation.success) {
      res.status(400).json({
        message: searchQueryValidation.error.issues.map((issue) => issue.path + ": " + issue.message).join(" "),
      });
      return;
    }

    // Set our url with our search params
    const petFinderURL = new URL("https://api.petfinder.com/v2/animals");
    petFinderURL.searchParams.set("type", petType);
    petFinderURL.searchParams.set("page", page);
    petFinderURL.searchParams.set("location", location);

    let response, result;
    let petResponse: PetResponse | null = null;
    try {
      // Fetch data.
      response = await fetch(petFinderURL, {
        headers: {
          Authorization: accessToken,
        },
      });
      if (!response.ok) {
        throw new Error();
      }

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
    } catch (e) {
      res.status(500).json({ message: "Something went wrong...." });
      return;
    }
    res.status(200).json(petResponse);
    return;
  }
  res.status(400).json({ message: "HTTP method not supported." });
  return;
}
