/**
 * @vitest-environment jsdom
 */

import { screen, render } from "@testing-library/react";
import LocationModal from "./LocationModal";
import { afterEach, beforeEach, describe, expect, it, vi } from "vitest";

type Props = {
  onSubmit: (newZipCode: string) => void;
  onClose: () => void;
};

const onSubmitMock = (newZipCode: string) => console.log(newZipCode);
const onCloseMock = () => console.log("close");

describe("LocationModal", () => {
  beforeEach(() => {
    render(<LocationModal onClose={onCloseMock} onSubmit={onSubmitMock} />);
  });

  test("displays 'New Zip Code'", () => {
    const headerElement = screen.getByRole("heading");
    expect(headerElement.textContent).toMatch(/New Zip Code/i);
  });
});
