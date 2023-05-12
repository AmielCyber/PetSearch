import Pagination from "@mui/material/Pagination";
import Stack from "@mui/material/Stack";

type Props = {
  currentPage: number;
  totalPages: number;
  isLoading: boolean;
  onPageChange: (event: React.ChangeEvent<unknown>, value: number) => void;
};
export default function PetPageNavigation(props: Props) {
  return (
    <Stack spacing={2} direction="row" justifyContent="center" alignItems="center">
      <Pagination
        count={props.totalPages}
        page={props.currentPage}
        color="primary"
        showFirstButton
        onChange={props.onPageChange}
        disabled={props.isLoading}
        size="large"
      />
    </Stack>
  );
}
