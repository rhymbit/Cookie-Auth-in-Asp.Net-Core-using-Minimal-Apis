import "./App.css";
import ClearCookies from "./components/ClearCookies";
import FetchNewTokens from "./components/FetchNewTokens";
import Login from "./components/Login";
import Root from "./components/Root";
import TestAuthentication from "./components/TestAuthentication";

function App() {
  return (
    <div className="App">
      <Root />
      <div id="divider" />
      <Login />
      <div id="divider" />
      <TestAuthentication />
      <div id="divider" />
      <FetchNewTokens />
      <div id="divider" />
      <ClearCookies />
    </div>
  );
}

export default App;
