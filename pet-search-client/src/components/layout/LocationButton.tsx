import {Suspense, lazy, useState} from "react";
import {Modal, Typography, CircularProgress, Box} from "@mui/material";
import {LoadingButton} from "@mui/lab";
import LocationOnIcon from "@mui/icons-material/LocationOn";
// My import.
import type {Location} from "../../hooks/LocationContext.tsx"

// Our component.
const LocationModal = lazy(() => import("./LocationModal"));

type Props = {
    location: Location;
    loadingNewZipcode: boolean;
    onZipcodeChange: (newZipcode: string) => void;
}

export default function LocationButton(props: Props) {
  const [open, setOpen] = useState(false);

  const handleOpen = () => setOpen(true);
  const handleClose = () => setOpen(false);

  console.log(location);
  return (
    <div>
        <LoadingButton
            aria-label="zipcode"
            title="Change zipcode"
            type="button"
            onClick={handleOpen}
            color="inherit"
            loading={props.loadingNewZipcode}
            loadingPosition="start"
            startIcon={<LocationOnIcon />}
        >
            <Typography fontSize="large">{props.location.zipcode}</Typography>
        </LoadingButton>
      <Modal open={open} onClose={handleClose} aria-labelledby="modal-modal-title">
        <Box display="flex" justifyContent="center" marginTop={7}>
          <Suspense fallback={<CircularProgress />}>
            <LocationModal onClose={handleClose} onZipcodeChange={props.onZipcodeChange} />
          </Suspense>
        </Box>
      </Modal>
    </div>
  );
}
