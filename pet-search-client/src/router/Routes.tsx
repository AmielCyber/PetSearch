import {lazy, Suspense} from "react";
import {createBrowserRouter, Navigate} from "react-router-dom";
// Our imports.
import App from "../App";
import ErrorPage from "../pages/ErrorPage.tsx";
import HomePage from "../pages/HomePage";
import LoadingPage from "../pages/LoadingPage.tsx";
import NotFoundPage from "../pages/NotFoundPage.tsx";

const PetSearchPage = lazy(() => import("../pages/PetSearchPage"))
const PetPage = lazy(() => import("../pages/PetPage"))

const router = createBrowserRouter([
    {
        path: "/",
        element: <App/>,
        errorElement: <ErrorPage />,
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
            {
                path: "/not-found",
                element: <NotFoundPage />
            },
            {path: "*", element: <Navigate replace to="/not-found"/>},
        ],
    },
]);

export default router;
