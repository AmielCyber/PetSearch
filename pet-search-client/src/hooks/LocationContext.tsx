import type { Dispatch, SetStateAction } from "react";
import { createContext, useState } from "react";

export type LocationContextType = {
  zipCode: string;
  setZipCode: Dispatch<SetStateAction<string>>;
};

export const LocationContext = createContext<LocationContextType | null>(null);
const defaultZipCode = "92101";

type Props = {
  children: React.ReactNode;
};
export function LocationProvider(props: Props) {
  const [zipCode, setZipCode] = useState(defaultZipCode);

  return <LocationContext.Provider value={{ zipCode, setZipCode }}>{props.children}</LocationContext.Provider>;
}
