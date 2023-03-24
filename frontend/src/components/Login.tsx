import { useState } from "react";

const Login = () => {
  const [results, setResults] = useState<string | null>("Click to login");
  const [errors, setErrors] = useState<string | null>(null);

  const onClick = () => {
    setResults("Logging in...");
    setErrors(null);
    fetch("https://localhost:7032/login?username=prateek", {
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
      <button onClick={onClick}>Login</button>
      <h4>{results ? results.toString() : ""}</h4>
      <h4>{errors ? errors.toString() : ""}</h4>
    </div>
  );
};

export default Login;
