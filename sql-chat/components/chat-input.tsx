import { useState } from "react";

export const ChatInput = ({
  onSendMessage,
  isLoading,
}: {
  onSendMessage: (content: string) => void;
  isLoading: boolean;
}) => {
  const [content, setContent] = useState("");

  const handleSend = () => {
    if (content.trim()) {
      onSendMessage(content);
      setContent("");
    }
  };

  return (
    <div className="flex items-center w-full p-4 bg-slate-800 font-sans">
      <textarea
        value={content}
        onChange={(e) => setContent(e.target.value)}
        onKeyDown={(e) => e.key === "Enter" && !e.shiftKey && handleSend()}
        className="flex-1 resize-none rounded-lg bg-slate-700 text-slate-200 placeholder-slate-400 border border-slate-600 p-3 shadow-sm focus:outline-none focus:ring-2 focus:ring-emerald-400 focus:border-emerald-400 font-medium"
        placeholder="Send a message..."
        rows={1}
        disabled={isLoading}
      />
      <button
        onClick={handleSend}
        className="ml-4 px-4 py-2 text-slate-100 bg-emerald-600 rounded-md hover:bg-emerald-500 focus:ring-2 focus:ring-emerald-400 font-medium disabled:bg-slate-600 disabled:cursor-not-allowed"
        disabled={isLoading || content.trim() === ""}
      >
        {isLoading ? "Sending..." : "Send"}
      </button>
    </div>
  );
};
