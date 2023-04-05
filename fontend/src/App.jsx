import "./App.scss";
import "bootstrap/dist/css/bootstrap.min.css";

import { BrowserRouter, Route, Routes } from "react-router-dom";
import Navbar from "./Components/Layouts/Navbar";
import Sidebar from "./Components/Layouts/Sidebar";
import Footer from "./Components/Layouts/Footer";
import Index from "./Pages/Index";
import Layout from "./Pages/Layout";
import Contact from "./Pages/Contact";
import About from "./Pages/About";
import Rss from "./Pages/Rss";

function App() {
  return (
    <>
      <BrowserRouter>
        <Navbar />
        <div className="container-fluid">
          <div className="row">
            <div className="col-9">
              <Routes>
                <Route path="/" element={<Layout />}>
                  <Route path="/" element={<Index />} />
                  <Route path="/blog" element={<Index />} />
                  <Route path="/blog/contact" element={<Contact />} />
                  <Route path="/blog/about" element={<About />} />
                  <Route path="/blog/rss" element={<Rss />} />
                </Route>
              </Routes>
            </div>
            <div className="col-3 border-start">
              <Sidebar />
            </div>
          </div>
        </div>
        <Footer />
      </BrowserRouter>
    </>
  );
}

export default App;
