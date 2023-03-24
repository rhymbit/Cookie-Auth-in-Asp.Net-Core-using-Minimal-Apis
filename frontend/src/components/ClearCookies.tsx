import { useState } from "react";

const ClearCookies = () => {
  const [results, setResults] = useState<string | null>(
    "Click to clear cookies"
  );
  const [errors, setErrors] = useState<string | null>(null);

  const onClick = () => {
    setResults("Clearing ...");
    setErrors(null);
    fetch("https://localhost:7032/clear-cookies", {
      credentials: "include",
    })
      .then((res) => res.json())
      .then((res) => setResults(res))
      .catch((err) => {
        setResults(null);
        setErrors(err);
      });
  };

  return (
    <div>
      <button onClick={onClick}>Clear Cookies</button>
      <h4>{results ? results.toString() : ""}</h4>
      <h4>{errors ? errors.toString() : ""}</h4>
    </div>
  );
};

export default ClearCookies;
