import { Suspense, lazy, useState } from "react";
import {Button, Modal, Typography, CircularProgress, Box} from "@mui/material";

// Our component.
const LocationModal = lazy(() => import("./LocationModal"));

type Props = {
  onZipCodeChange: (newZipCode: string) => void;
  currentZip: string;
};

export default function LocationButton(props: Props) {
  const [open, setOpen] = useState(false);
  const handleOpen = () => setOpen(true);
  const handleClose = () => setOpen(false);

  const handleLocationChange = (newZipCode: string) => {
    // Close modal and change query search.
    setOpen(false);
    props.onZipCodeChange(newZipCode);
  };

  return (
    <div>
      <Button onClick={handleOpen} color="inherit">
        <Typography>ZIP Code: {props.currentZip}</Typography>
      </Button>
      <Modal open={open} onClose={handleClose} aria-labelledby="modal-modal-title">
        <Box display="flex" justifyContent="center" marginTop={7}>
          <Suspense fallback={<CircularProgress />}>
            <LocationModal onSubmit={handleLocationChange} onClose={handleClose} />
          </Suspense>
        </Box>
      </Modal>
    </div>
  );
}
