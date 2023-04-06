import Image, { ImageLoader } from "next/image";
import { useState } from "react";
import Paper from "@mui/material/Paper";
import HideImageTwoToneIcon from "@mui/icons-material/HideImageTwoTone";
import ArrowCircleLeftOutlinedIcon from "@mui/icons-material/ArrowCircleLeftOutlined";
import ArrowCircleRightOutlinedIcon from "@mui/icons-material/ArrowCircleRightOutlined";
// Our imports.
import type { PhotoSize } from "@/models/Pet";
import ImagePointerNavButton from "@/components/image-viewer/ImagePointerNavButton";
import ImageCircleNavButtons from "@/components/image-viewer/ImageCircleNavButtons";
import styles from "@/styles/image-container/PetImageContainer.module.css";

// Image loader so Next.js does not pass invalid args.
const myLoader: ImageLoader = ({ src, width }) => {
  return `${src}&width=${width}`;
};

const emptyImageStyle = {
  width: "300",
  height: "300",
  display: "flex",
  justifyContent: "center",
  alignItems: "center",
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
      <Paper elevation={4}>
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
    <Paper elevation={4}>
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
      </section>
    </Paper>
  );
}
