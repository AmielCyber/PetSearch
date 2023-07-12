import {isRouteErrorResponse, Link, useRouteError} from "react-router-dom";
import {Box, Link as MuiLink, Typography} from "@mui/material";

export default function ErrorPage() {
    const error: unknown = useRouteError();
    console.log(error);

    let errorMsg: string;
    if (isRouteErrorResponse(error)) {
        errorMsg = error.error?.message || error.statusText;
    } else if (error instanceof Error) {
        errorMsg = error.message;
    } else {
        errorMsg = "";
    }

    return (
        <>
            <Box textAlign="center">
                <Typography variant="h1">Oops!</Typography>
                <Typography variant="body1" paragraph>Sorry, an unexpected error has occurred.</Typography>
                <Typography variant="body2"><i>{errorMsg}</i></Typography>
            </Box>
            <Box textAlign="center">
                <MuiLink sx={{textDecoration: "none"}} component={Link} to="/">
                    Take Me Home
                </MuiLink>
            </Box>
        </>
    )
}
