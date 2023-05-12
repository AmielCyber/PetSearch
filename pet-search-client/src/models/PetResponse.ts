import Pet from "./pet";

export interface Pagination {
  count_per_page: number;
  total_count: number;
  current_page: number;
  total_pages: number;
}

export default interface PetResponse {
  pets: Pet[];
  pagination: Pagination;
}
