import Image, { ImageLoader } from "next/image";
import { useState } from "react";
import Paper from "@mui/material/Paper";
import HideImageTwoToneIcon from "@mui/icons-material/HideImageTwoTone";
import NavigateBeforeIcon from "@mui/icons-material/NavigateBefore";
import NavigateNextIcon from "@mui/icons-material/NavigateNext";
// Our imports.
import type { PhotoSize } from "@/models/Pet";
import ImagePointerNavButton from "@/components/image-viewer/ImagePointerNavButton";
import ImageCircleNavButtons from "@/components/image-viewer/ImageCircleNavButtons";
import styles from "@/styles/image-container/PetImageContainer.module.css";

// Image loader so Next.js does not pass invalid args.
const myLoader: ImageLoader = ({ src, width }) => {
  return `${src}&width=${width}`;
};

type Props = {
  name: string;
  photos: PhotoSize[];
};

export default function PetImageContainer(props: Props) {
  const [imgIndex, setImageIndex] = useState(0);

  if (props.photos.length < 1) {
    // No photos.
    return (
      <Paper elevation={1}>
        <div className={styles.blankImage}>
          <HideImageTwoToneIcon fontSize="large" />
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
    <Paper elevation={1}>
      <section className={styles.imageContainer}>
        <Image
          loader={myLoader}
          src={props.photos[imgIndex].large}
          alt={props.name}
          sizes="600px"
          blurDataURL="/blur/grayBlur.png"
          fill
        />
        <div className={styles.imgNavButtons}>
          <ImagePointerNavButton onClickNavigation={handlePrevClick}>
            <NavigateBeforeIcon />
          </ImagePointerNavButton>
          <ImagePointerNavButton onClickNavigation={handleNextClick}>
            <NavigateNextIcon />
          </ImagePointerNavButton>
        </div>
        <ImageCircleNavButtons
          totalNavDots={props.photos.length}
          currentIndex={imgIndex}
          onSelectDotNav={handleDotNavigation}
        />
      </section>
    </Paper>
  );
}
