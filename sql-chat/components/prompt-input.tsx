import React, { useState } from "react";

interface PromptInputProps {
  onSend: (prompt: string) => void;
  onStop: () => void;
  isPromptProcessCompleted: boolean;
}

const PromptInput: React.FC<PromptInputProps> = ({
  onSend,
  onStop,
  isPromptProcessCompleted,
}) => {
  const [prompt, setPrompt] = useState("");
  const [isSending, setIsSending] = useState(false);

  const handleSend = () => {
    if (!prompt.trim()) return; // Prevent empty prompts

    setIsSending(true);
    onSend(prompt);
  };

  const handleStop = () => {
    setIsSending(false);
    onStop();
  };

  return (
    <div>
      <input
        type="text"
        value={prompt}
        onChange={(e) => setPrompt(e.target.value)}
        placeholder="Enter your prompt"
        disabled={isSending || !isPromptProcessCompleted}
      />
      <button
        onClick={handleSend}
        disabled={isSending || !isPromptProcessCompleted}
      >
        {isSending ? "Sending..." : "Send"}
      </button>
      {isSending ||
        (!isPromptProcessCompleted && (
          <button onClick={handleStop}>Stop</button>
        ))}
    </div>
  );
};

export default PromptInput;
