import Pagination from "@mui/material/Pagination";
import Stack from "@mui/material/Stack";
import { useRouter } from "next/router";
type Props = {
  totalPages: number;
};
export default function PetPageNavigation(props: Props) {
  const router = useRouter();
  const query = router.query;
  const page = query.page ? parseInt(query.page as string) : 1;
  const handlePageChange = (event: React.ChangeEvent<unknown>, value: number) => {
    router.push({
      query: {
        ...query,
        page: value,
      },
    });
  };
  return (
    <Stack spacing={2} direction="row" justifyContent="center" alignItems="center">
      <Pagination
        count={props.totalPages}
        page={page}
        color="primary"
        showFirstButton
        showLastButton
        onChange={handlePageChange}
      />
    </Stack>
  );
}
