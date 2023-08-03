import type {Location} from "../../hooks/LocationContext.tsx";

const BASE_URL = import.meta.env.VITE_API_URL;

export type LocationResponse = {
    location?: Location;
    errorMessage?: string;
}

function getErrorMessage(responseCode: number, fromZipcode: boolean): string{
    if(fromZipcode){
        switch (responseCode){
            case 400:
                return "Invalid zipcode entered."
            case 404:
                return "Zipcode does not exist."
            default:
                return "There was an error in validating your entered zipcode."
        }
    }
    switch (responseCode){
        case 400:
            return "Invalid coordinates entered."
        case 404:
            return "Could not locate with the given location."
        default:
            return "There was an error in locating you."
    }
}

export async function getLocationFromZipCode(zipcode: string): Promise<LocationResponse>{
    try {
        const response: Response = await fetch(BASE_URL + `Location/Zipcode/${zipcode}`, {
            method: "Get"
        })
        if(!response.ok){
            return {
                errorMessage: getErrorMessage(response.status, true)
            }
        }
        const responseData = await response.json();
        return {
            location: responseData as Location,
        }
    }catch(e){
        return {
            errorMessage: getErrorMessage(500, true)
        }
    }
}

function getCurrentPosition(): Promise<GeolocationPosition>{
    return new Promise((resolve, reject) => {
        if("geolocation" in navigator){
            navigator.geolocation.getCurrentPosition(resolve, reject);
        }else{
            reject(new Error("Geolocation is not supported by your browser."))
        }
    })
}

async function getLocationFromCoordinates(longitude: number, latitude: number): Promise<LocationResponse>{
    try {
        const response: Response = await fetch(BASE_URL + `location/coordinates?longitude=${longitude}&latitude=${latitude}`)
        if(!response.ok){
            return {
                errorMessage: getErrorMessage(response.status, false)
            }
        }
        const responseData = await response.json();
        return {
            location: responseData as Location,
        }
    }catch(e){
        return {
            errorMessage: getErrorMessage(500, false)
        }
    }
}

export async function getUserLocation(): Promise<LocationResponse>{
    try{
        const position = await getCurrentPosition();
        const longitude = position.coords.longitude;
        const latitude = position.coords.latitude;
        return await getLocationFromCoordinates(longitude, latitude);
    }catch(e){
        if(e instanceof Error){
            return {
                errorMessage: e.message
            }
        }
    }
    return {
        errorMessage: "Unable to retrieve your location."
    }
}

