import Pet from "./Pet";

export interface Pagination {
  count_per_page: number;
  total_count: number;
  current_page: number;
  total_pages: number;
  _links: {
    next: string | undefined; // Undefined if at last page
    previous: string | undefined; // Undefined if at first page
  };
}

export default interface PetResponse {
  pets: Pet[];
  pagination: Pagination;
}
