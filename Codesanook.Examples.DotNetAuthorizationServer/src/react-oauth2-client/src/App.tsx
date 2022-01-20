import { BrowserRouter, Routes, Route } from "react-router-dom";
import AuthorizationTestingFlow from "./components/Authorization/AuhorizationTestingFlow";
import AuthorizationCallback from "./components/Authorization/AuthorizationCallback";
import './App.css';

export default function App() {
  return (
    // https://reacttraining.com/blog/react-router-v6-pre/
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<AuthorizationTestingFlow />} />
        <Route path="/authentication/login-callback" element={<AuthorizationCallback />} />
      </Routes>
    </BrowserRouter>
  );
}
