import type {FormEvent} from "react";
import {useState, useRef} from "react";
import { Typography, TextField, Button, Paper, InputAdornment } from "@mui/material";
import ErrorIcon from "@mui/icons-material/Error";
import LocationOnIcon from "@mui/icons-material/LocationOn";

const style = {
  position: "absolute",
  top: "25%",
  left: "50%",
  transform: "translate(-50%, -50%)",
  borderRadius: "24px",
  boxShadow: 24,
  p: 4,
};

const buttonGroupStyle = {
  display: "flex",
  justifyContent: "space-between",
  gap: "4px",
  marginTop: "4px",
};

function getAdornmentIcon(isError: boolean) {
  return <InputAdornment position="end">{isError ? <ErrorIcon color="error" /> : <LocationOnIcon />}</InputAdornment>;
}

type Props = {
  onClose: () => void;
  onZipcodeChange: (newZipcode: string) => void;
};

export default function LocationModal(props: Props) {
  const [error, setError] = useState<string>();
  const locationRef = useRef<HTMLInputElement>(null); // Get the value of the input tag.

  const submitHandler = async (event: FormEvent) => {
    event.preventDefault();

    // Verify entered zip code. We could verify with an api provider in the future.
    const enteredZipCode = locationRef.current ? locationRef.current.value : "";
    const isValidZipCode = /\d{5}$/.test(enteredZipCode);
    if (!isValidZipCode) {
      // Give the input an error style.
      setError("Enter a valid 5 digit zipcode.")
    } else {
      // Close modal and handle new input of zip code.
      props.onZipcodeChange(enteredZipCode);
      props.onClose();
    }
  };

  const hasError = !!error;
  const adornmentInputIcon = getAdornmentIcon(hasError);

  return (
    <Paper sx={style} variant="outlined">
      <form onSubmit={submitHandler}>
        <Typography id="modal-modal-title" variant="h6" component="h2" textAlign="center" marginBottom="1rem">
          New Zip Code
        </Typography>
        <TextField
          id="filled-helperText"
          label="Enter Zip Code"
          variant="outlined"
          autoFocus
          type="text"
          inputProps={{
            inputMode: "numeric",
            minLength: "5",
            maxLength: "5",
            autoComplete: "postal-code",
          }}
          InputProps={{
            endAdornment: adornmentInputIcon,
          }}
          error={hasError}
          inputRef={locationRef}
          helperText={error ?? "Enter a 5 digit zip code."}
          size="small"
        />
        <div style={buttonGroupStyle}>
          <Button type="button" onClick={props.onClose} variant="outlined" color="secondary">
            Cancel
          </Button>
          <Button type="submit" onSubmit={submitHandler} variant="outlined">
            Submit
          </Button>
        </div>
      </form>
    </Paper>
  );
}
