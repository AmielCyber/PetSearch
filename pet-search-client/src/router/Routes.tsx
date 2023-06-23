import {lazy, Suspense} from "react";
import {createBrowserRouter, Navigate} from "react-router-dom";
// Our imports.
import App from "../App";
import HomePage from "../pages/HomePage";
import LoadingPage from "../pages/LoadingPage.tsx";

const PetSearchPage = lazy(() => import("../pages/PetSearchPage"))
const PetPage = lazy(() => import("../pages/PetPage"))

const router = createBrowserRouter([
    {
        path: "/",
        element: <App/>,
        children: [
            {
                path: "/",
                element: <HomePage/>
            },
            {
                path: "/search/:petType",
                element:
                    <Suspense fallback={<LoadingPage pageName="search"/>}>
                        <PetSearchPage/>
                    </Suspense>
            },
            {
                path: "/pets/:id",
                element:
                    <Suspense fallback={<LoadingPage pageName="pet"/>}>
                        <PetPage/>
                    </Suspense>
            },
            {path: "*", element: <Navigate replace to="/not-found"/>},
        ],
    },
]);

export default router;
