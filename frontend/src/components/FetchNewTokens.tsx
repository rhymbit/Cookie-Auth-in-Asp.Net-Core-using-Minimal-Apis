import { useState } from "react";

const FetchNewTokens = () => {
  const [results, setResults] = useState<string | null>(
    "Click to fetch new tokens"
  );
  const [errors, setErrors] = useState<string | null>(null);

  const onClick = () => {
    setResults("Fetching ...");
    setErrors(null);
    fetch("https://localhost:7032/refresh-tokens", {
      credentials: "include",
    })
      .then(async (res) => {
        if (!res.ok) {
          const err = await res.json();
          throw new Error(err.message);
        }
        const results = await res.json();
        setResults(results);
      })
      .catch((err) => {
        setResults(null);
        setErrors(err);
      });
  };

  return (
    <div>
      <button onClick={onClick}>New Tokens</button>
      <h4>{results ? results.toString() : ""}</h4>
      <h4>{errors ? errors.toString() : ""}</h4>
    </div>
  );
};

export default FetchNewTokens;
