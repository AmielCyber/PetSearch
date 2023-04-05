import IconButton from "@mui/material/IconButton";
import CircleIcon from "@mui/icons-material/Circle";
import CircleOutlinedIcon from "@mui/icons-material/CircleOutlined";
// Our import.
import styles from "@/styles/image-container/ImageNavDots.module.css";

function getNavDotList(
  totalNavDots: number,
  currentIndex: number,
  onSelectDotNav: (index: number) => void
): Array<React.ReactNode> {
  const list = Array<React.ReactNode>(totalNavDots);
  for (let index = 0; index < list.length; index++) {
    if (index === currentIndex) {
      list[index] = (
        <li key={index}>
          <IconButton aria-label={`Select image ${index + 1}`} disabled>
            <CircleIcon fontSize="medium" color="primary" />
          </IconButton>
        </li>
      );
    } else {
      list[index] = (
        <li key={index}>
          <IconButton
            aria-label={`Select image ${index + 1}`}
            onClick={() => onSelectDotNav(index)}
            sx={outlinedCircleStyle}
          >
            <CircleOutlinedIcon fontSize="medium" color="primary" />
          </IconButton>
        </li>
      );
    }
  }
  return list;
}

const outlinedCircleStyle = {
  "&:hover": {
    backgroundColor: "rgb(189,189,189,0.4)",
  },
};
type Props = {
  totalNavDots: number;
  currentIndex: number;
  onSelectDotNav: (index: number) => void;
};
export default function ImageCircleNavButtons(props: Props) {
  return (
    <div className={styles.navDots}>
      <menu>{getNavDotList(props.totalNavDots, props.currentIndex, props.onSelectDotNav)}</menu>
    </div>
  );
}
