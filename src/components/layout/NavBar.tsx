import AppBar from "@mui/material/AppBar";
import Box from "@mui/material/Box";
import Toolbar from "@mui/material/Toolbar";
import Typography from "@mui/material/Typography";
import Button from "@mui/material/Button";
import Link from "next/link";

export default function MainNavigation() {
  return (
    <Box sx={{ flexGrow: 1 }}>
      <AppBar position="static">
        <Toolbar>
          <Typography variant="inherit" component="div" sx={{ flexGrow: 1 }}>
            <Link href={"/"}>Home</Link>
          </Typography>
          <Button color="inherit">
            <Typography variant="inherit" component="div">
              Location
            </Typography>
          </Button>
        </Toolbar>
      </AppBar>
    </Box>
  );
}
