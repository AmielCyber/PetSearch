import {Box, Link as MuiLink, Typography} from "@mui/material";
import {Link} from "react-router-dom";

export default function NotFoundPage() {
    return (
        <>
            <Box textAlign="center">
                <Typography variant="h1">Oops!</Typography>
                <Typography variant="h2">404</Typography>
                <Typography variant="h4">Page Not Found</Typography>
                <Typography variant="body1" paragraph>The page you are looking for does not exist.</Typography>
            </Box>
            <Box textAlign="center">
                <MuiLink sx={{textDecoration: "none"}} component={Link} to="/">
                    Take Me Home
                </MuiLink>
            </Box>
        </>
    )
}
