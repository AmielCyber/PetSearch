import { Navigate, createBrowserRouter } from "react-router-dom";
// Our imports.
import App from "../App";
import HomePage from "../pages/HomePage";
import PetSearchPage from "../pages/PetSearchPage";
import PetPage from "../pages/PetPage";

const router = createBrowserRouter([
  {
    path: "/",
    element: <App />,
    children: [
      { path: "/", element: <HomePage /> },
      { path: "/search/:petType", element: <PetSearchPage /> },
      { path: "/pets/:id", element: <PetPage /> },
      { path: "*", element: <Navigate replace to="/not-found" /> },
    ],
  },
]);

export default router;
