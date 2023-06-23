import {Box, CircularProgress, Typography} from "@mui/material";

const circularSx = {
    textAlign: "center"
}

type Props = {
    pageName: string;
}
export default function LoadingPage(props: Props) {
    return (
        <>
            <Typography variant="h4" textAlign="center" mb={2}>
                Loading {props.pageName} page...
            </Typography>
            <Box display="flex" justifyContent="center">
                <CircularProgress sx={circularSx}/>
            </Box>
        </>
    );
}