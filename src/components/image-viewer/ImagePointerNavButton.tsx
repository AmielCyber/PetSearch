import IconButton from "@mui/material/IconButton";

type Props = {
  onClickNavigation: VoidFunction;
  children: React.ReactNode;
};
const iconButtonStyles = {
  backgroundColor: "ButtonShadow",
  opacity: "0.4",
  "&:hover": {
    opacity: "0.7",
    backgroundColor: "ButtonShadow",
  },
};

export default function ImagePointerNavButton(props: Props) {
  return (
    <IconButton aria-label="next-image" color="primary" onClick={props.onClickNavigation} sx={iconButtonStyles}>
      {props.children}
    </IconButton>
  );
}
