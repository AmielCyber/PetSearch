import { useState } from "react";
import { Button, Modal, Typography } from "@mui/material";
// Our component.
import LocationModal from "./LocationModal";

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
      <Button onClick={handleOpen}>
        <Typography variant="button" component="text" color="white">
          ZIP Code: {props.currentZip}
        </Typography>
      </Button>
      <Modal open={open} onClose={handleClose} aria-labelledby="modal-modal-title">
        <div>
          <LocationModal onSubmit={handleLocationChange} onClose={handleClose} />
        </div>
      </Modal>
    </div>
  );
}
