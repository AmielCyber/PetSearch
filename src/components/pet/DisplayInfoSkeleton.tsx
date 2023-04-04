import Grid from "@mui/material/Grid";
import Skeleton from "@mui/material/Skeleton";
import Typography from "@mui/material/Typography";

export default function DisplayInfoSkeleton() {
  return (
    <Grid container justifyContent="center" columnSpacing={4} rowSpacing={4} alignItems="center">
      <Grid item xs={12} textAlign="center" mt={2}>
        <Typography variant="h2">
          <Skeleton />
        </Typography>
      </Grid>
      <Grid item marginTop="0">
        <Skeleton width={300} height={300} />
      </Grid>
      <Grid item textAlign="center">
        <Skeleton width={100} variant="text" />
        <Skeleton width={100} variant="text" />
        <Skeleton width={100} variant="text" />
        <Skeleton width={100} variant="text" />
      </Grid>
      <Grid item textAlign="center" xs={12}>
        <Typography variant="h4">Description</Typography>
        <Typography variant="body1">
          <Skeleton />
        </Typography>
      </Grid>
    </Grid>
  );
}
