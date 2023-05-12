import IconButton from "@mui/material/IconButton";

const iconButtonStyles = {
  opacity: "0.7",
  "&:hover": {
    opacity: "0.9",
  },
};

type Props = {
  onClickNavigation: VoidFunction;
  children: React.ReactNode;
};

export default function ImagePointerNavButton(props: Props) {
  return (
    <IconButton aria-label="next-image" color="primary" onClick={props.onClickNavigation} sx={iconButtonStyles}>
      {props.children}
    </IconButton>
  );
}
