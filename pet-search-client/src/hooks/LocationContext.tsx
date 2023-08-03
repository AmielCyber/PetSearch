import type {Dispatch, ReactNode, SetStateAction} from "react";
import { createContext, useState } from "react";

export type Location = {
  zipcode: string;
  locationName: string;
}

export type LocationContextType = {
  location: Location;
  setLocation: Dispatch<SetStateAction<Location>>;
};

export const LocationContext = createContext<LocationContextType | null>(null);

const defaultLocation: Location = {
  zipcode: "92101",
  locationName: "San Diego, California 92101, United States",
};

type Props = {
  children: ReactNode;
};
export function LocationProvider(props: Props) {
  const [location, setLocation] = useState(defaultLocation);

  return <LocationContext.Provider value={{ location, setLocation}}>{props.children}</LocationContext.Provider>;
}
