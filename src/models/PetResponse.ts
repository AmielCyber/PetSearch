import Pet from "./Pet";

export default interface PetResponse {
  pets: Pet[];
  pagination: {
    count_per_page: number;
    total_count: number;
    current_page: number;
    total_pages: number;
    links: {
      next: string | undefined; // Undefined if at last page
      previous: string | undefined; // Undefined if at first page
    };
  };
}
