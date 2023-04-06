import IconButton from "@mui/material/IconButton";

type Props = {
  onClickNavigation: VoidFunction;
  children: React.ReactNode;
};
const iconButtonStyles = {
  opacity: "0.7",
  "&:hover": {
    opacity: "0.9",
  },
};

export default function ImagePointerNavButton(props: Props) {
  return (
    <IconButton aria-label="next-image" color="primary" onClick={props.onClickNavigation} sx={iconButtonStyles}>
      {props.children}
    </IconButton>
  );
}
