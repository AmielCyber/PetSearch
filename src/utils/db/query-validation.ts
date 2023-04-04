import { z } from "zod";

export type petQuery = {
  petType: "dog" | "cat";
  location: string;
  page: string;
};
/**
 * Validates pet type and page search params.
 * @param petType pet type that we will search for.
 * @param page page number from the database
 * @returns SafeParseReturnType<petQuery, petQuery> will determine if input validation is valid.
 */
export function validateQuery(
  petType: string | undefined,
  location: string | undefined,
  page: string | undefined
): z.SafeParseReturnType<petQuery, petQuery> {
  const searchQuerySchema = z.object({
    petType: z.enum(["dog", "cat"], {
      required_error: "Is required.",
      invalid_type_error: "Not currently supported.",
    }),
    location: z
      .string()
      .regex(/^[1-9]\d*$/, "Must be a positive number.")
      .min(5, "Zip Code must be 5 digits.")
      .max(5, "Zip Code must be 5 digits."),
    page: z.string().regex(/^[1-9]\d*$/, "Must be a positive number."),
  });
  return searchQuerySchema.safeParse({
    petType: petType,
    location: location,
    page: page,
  });
}
