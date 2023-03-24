import { useState } from "react";

const Root = () => {
  const [results, setResults] = useState<string | null>(null);
  const [errors, setErrors] = useState<string | null>(null);

  const onClick = () => {
    setErrors(null);
    fetch("https://localhost:7032")
      .then((res) => res.json())
      .then((res) => setResults(res))
      .catch((err) => {
        setResults(null);
        setErrors(err);
      });
  };

  return (
    <div>
      <button onClick={onClick}>Root Route</button>
      <h4>
        {results
          ? Object.keys(results).map((item, index) => (
              <li key={index}>{item}</li>
            ))
          : ""}
      </h4>
      <h4>{errors ? errors.toString() : ""}</h4>
    </div>
  );
};

export default Root;
