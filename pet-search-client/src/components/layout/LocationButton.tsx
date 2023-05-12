import dynamic from "next/dynamic";
import CircularProgress from "@mui/material/CircularProgress";
import { useState } from "react";
import { Button, Modal, Typography } from "@mui/material";
// Our component.
const LocationModal = dynamic(() => import("@/components/layout/LocationModal"), {
  loading: () => <CircularProgress />,
});

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
        <div>
          <LocationModal onSubmit={handleLocationChange} onClose={handleClose} />
        </div>
      </Modal>
    </div>
  );
}
