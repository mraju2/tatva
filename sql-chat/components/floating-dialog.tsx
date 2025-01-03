import React, { useEffect } from "react";
import {
  useFloating,
  offset,
  flip,
  shift,
  autoUpdate,
} from "@floating-ui/react";

interface FloatingUIProps {
  referenceElement: HTMLElement | null; // The element the floating content is anchored to
  isVisible: boolean; // Visibility toggle
  content: React.ReactNode; // Content to display in the floating element
  onClose: () => void; // Function to call when the floating element is closed
  placement?: "top" | "bottom" | "left" | "right"; // Optional placement
}

export const FloatingUI = ({
  referenceElement,
  isVisible,
  content,
  onClose,
  placement = "top",
}: FloatingUIProps) => {
  const { x, y, strategy, refs, update } = useFloating({
    placement,
    middleware: [offset(10), flip(), shift()],
    whileElementsMounted: autoUpdate,
  });

  // Assign the reference element from props
  useEffect(() => {
    refs.setReference(referenceElement);
    if (update) update();
  }, [referenceElement, refs, update]);

  if (!isVisible || !referenceElement) return null;

  return (
    <div
      ref={refs.setFloating}
      style={{
        position: strategy,
        top: y ?? 0,
        left: x ?? 0,
      }}
      className="bg-white text-black p-4 rounded-lg shadow-lg z-50 max-w-xs"
    >
      <div className="relative">
        {/* Close Button */}
        <button
          onClick={onClose}
          className="absolute top-1 right-1 text-gray-500 hover:text-black"
        >
          ✕
        </button>
        {content}
      </div>
    </div>
  );
};
