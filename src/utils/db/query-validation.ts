import { z } from "zod";

export type petQuery = {
  petType: "dog" | "cat";
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
  page: string | undefined
): z.SafeParseReturnType<petQuery, petQuery> {
  const searchQuerySchema = z.object({
    petType: z.enum(["dog", "cat"], {
      required_error: "Pet type is required",
      invalid_type_error: "Pet type not supported",
    }),
    page: z.string().regex(/^[1-9]\d*$/, "Pages must be a positive number"),
  });
  return searchQuerySchema.safeParse({
    petType: petType,
    page: page,
  });
}
