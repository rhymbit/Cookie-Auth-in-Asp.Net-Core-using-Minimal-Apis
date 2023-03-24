import { useState } from "react";

const TestAuthentication = () => {
  const [results, setResults] = useState<string | null>(
    "Click to test authentication"
  );
  const [errors, setErrors] = useState<string | null>(null);

  const onClick = () => {
    setResults("Testing Auth ...");
    setErrors(null);
    fetch("https://localhost:7032/test-authentication", {
      credentials: "include",
    })
      .then(async (res) => {
        if (!res.ok) {
          const err = await res.json();
          throw new Error(err);
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
      <button onClick={onClick}>Test Authentication</button>
      <h4>{results ? results.toString() : ""}</h4>
      <h4>{errors ? errors.toString() : ""}</h4>
    </div>
  );
};

export default TestAuthentication;
