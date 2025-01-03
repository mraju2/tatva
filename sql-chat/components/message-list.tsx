import React, { useState } from "react";
import MarkdownIt from "markdown-it";
import { Skeleton } from "./skeleton";
import { FloatingUI } from "./floating-dialog"; // Import the common FloatingUI component
import { Message } from "@/types/chat";

const mdParser = new MarkdownIt({
  html: true,
  linkify: true,
  typographer: false,
});

interface MessageListProps {
  messages: Message[];
  loading: boolean;
}

export const MessageList = ({ messages, loading }: MessageListProps) => {
  const [showQuery, setShowQuery] = useState<string | null>(null);
  const [referenceElement, setReferenceElement] = useState<HTMLElement | null>(
    null
  );

  const handleIconClick = (query: string | undefined, ref: HTMLElement) => {
    console.log("Reference Element:", ref); // Debug reference element
    console.log("Query:", query); // Debug query
    setReferenceElement(ref);
    setShowQuery(query || null);
    setReferenceElement(ref);
    setShowQuery(query || null);
  };
  console.log(messages);

  const handleClose = () => {
    setShowQuery(null);
    setReferenceElement(null);
  };

  return (
    <div className="flex flex-col gap-4">
      {messages.map((message) => (
        <div
          key={message.id}
          className={`relative group p-4 rounded-lg ${
            message.role === "assistant"
              ? "bg-emerald-700 text-white self-start"
              : "bg-slate-800 text-white self-end"
          }`}
        >
          {/* Render message content */}
          <div
            dangerouslySetInnerHTML={{
              __html: mdParser.render(message.content),
            }}
          ></div>

          {/* Icon displayed on hover */}
          {message.role === "assistant" && (
            <div className="absolute top-2 right-2 opacity-0 group-hover:opacity-100 transition-opacity">
              <button
                className="p-2 bg-gray-200 text-black rounded-full hover:bg-gray-300"
                onClick={(e) =>
                  handleIconClick(message.sqlQuery, e.currentTarget)
                }
              >
                <svg
                  xmlns="http://www.w3.org/2000/svg"
                  width="24"
                  height="24"
                  viewBox="0 0 24 24"
                  fill="none"
                  stroke="currentColor"
                  strokeWidth="2"
                  strokeLinecap="round"
                  strokeLinejoin="round"
                  className="icon icon-tabler icons-tabler-outline icon-tabler-file-type-sql"
                >
                  <path stroke="none" d="M0 0h24v24H0z" fill="none" />
                  <path d="M14 3v4a1 1 0 0 0 1 1h4" />
                  <path d="M14 3v4a1 1 0 0 0 1 1h4" />
                  <path d="M5 20.25c0 .414 .336 .75 .75 .75h1.25a1 1 0 0 0 1 -1v-1a1 1 0 0 0 -1 -1h-1a1 1 0 0 1 -1 -1v-1a1 1 0 0 1 1 -1h1.25a.75 .75 0 0 1 .75 .75" />
                  <path d="M5 12v-7a2 2 0 0 1 2 -2h7l5 5v4" />
                  <path d="M18 15v6h2" />
                  <path d="M13 15a2 2 0 0 1 2 2v2a2 2 0 1 1 -4 0v-2a2 2 0 0 1 2 -2z" />
                  <path d="M14 20l1.5 1.5" />
                </svg>
              </button>
            </div>
          )}
        </div>
      ))}

      {/* Floating UI for Query */}
      <FloatingUI
        referenceElement={referenceElement}
        content={<p>{showQuery}</p>}
        isVisible={!!showQuery}
        onClose={handleClose}
        placement="top"
      />

      {/* Loading skeleton */}
      {loading && <Skeleton />}
    </div>
  );
};
