import React from "react";

export const Skeleton = () => {
  return (
    <div className="w-full mt-2 mb-2">
      <div className="animate-pulse flex w-full">
        <div className="space-y-2 w-full">
          <div className="h-2 bg-gray-200 dark:bg-gray-600 rounded mr-14" />

          <div className="grid grid-cols-3 gap-4">
            <div className="h-2 bg-gray-200 dark:bg-gray-600 rounded col-span-2" />
            <div className="h-2 bg-gray-200 dark:bg-gray-600 rounded col-span-1" />
          </div>

          <div className="grid grid-cols-4 gap-4">
            <div className="h-2 bg-gray-200 dark:bg-gray-600 rounded col-span-1" />
            <div className="h-2 bg-gray-200 dark:bg-gray-600 rounded col-span-2" />
            <div className="h-2 bg-gray-200 dark:bg-gray-600 rounded col-span-1 mr-4" />
          </div>

          <div className="h-2 bg-gray-200 dark:bg-gray-600 rounded" />
        </div>
      </div>
    </div>
  );
};
