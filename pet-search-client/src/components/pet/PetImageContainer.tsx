import { useState } from "react";
import Paper from "@mui/material/Paper";
import HideImageTwoToneIcon from "@mui/icons-material/HideImageTwoTone";
import ArrowCircleLeftOutlinedIcon from "@mui/icons-material/ArrowCircleLeftOutlined";
import ArrowCircleRightOutlinedIcon from "@mui/icons-material/ArrowCircleRightOutlined";
// Our imports.
import type { PhotoSize } from "../../models/pet.ts";
import ImagePointerNavButton from "../image-viewer/ImagePointerNavButton";
import ImageCircleNavButtons from "../image-viewer/ImageCircleNavButtons";
import styles from "../../styles/image-container/PetImageContainer.module.css";

type Props = {
  name: string;
  photos: PhotoSize[];
};

export default function PetImageContainer(props: Props) {
  const [imgIndex, setImageIndex] = useState(0);

  if (props.photos.length < 1) {
    // No photos.
    return (
      <Paper elevation={4}>
        <div className={styles.blankImage}>
          <HideImageTwoToneIcon fontSize="large" color="primary" />
        </div>
      </Paper>
    );
  }

  const hasPrev = imgIndex > 0;
  const hasNext = imgIndex < props.photos.length - 1;

  const handlePrevClick = () => {
    if (hasPrev) {
      setImageIndex(imgIndex - 1);
    } else {
      setImageIndex(props.photos.length - 1);
    }
  };

  const handleNextClick = () => {
    if (hasNext) {
      setImageIndex(imgIndex + 1);
    } else {
      setImageIndex(0);
    }
  };

  const handleDotNavigation = (index: number) => {
    setImageIndex(index);
  };

  return (
    <Paper elevation={4}>
      <div className={styles.imageContainer}>
        <img src={props.photos[imgIndex].full} alt={props.name} sizes="500px" />
          {props.photos.length > 1 && (
              <>
                <div className={styles.imgNavButtons}>
                  <ImagePointerNavButton onClickNavigation={handlePrevClick}>
                    <ArrowCircleLeftOutlinedIcon fontSize="large" />
                  </ImagePointerNavButton>
                  <ImagePointerNavButton onClickNavigation={handleNextClick}>
                    <ArrowCircleRightOutlinedIcon fontSize="large" />
                  </ImagePointerNavButton>
                </div>
                <ImageCircleNavButtons
                    totalNavDots={props.photos.length}
                    currentIndex={imgIndex}
                    onSelectDotNav={handleDotNavigation}
                />
              </>
          )}
      </div>
    </Paper>
  );
}
