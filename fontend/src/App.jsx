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
import PostByAuthor from "./Pages/Blog/PostByAuthor";
import PostByCategory from "./Pages/Blog/PostByCategory";
import PostDetail from "./Components/PostDetail";
import Subscriber from "./Pages/Newsletter/Subscriber";

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
                  <Route path="blog/author/:slug" element={<PostByAuthor />} />
                  <Route path="blog/category/:slug" element={<PostByCategory />} />
                  <Route path="blog/post/:slug" element={<PostDetail />} />
                  <Route path="blog/subscriber" element={<Subscriber />} />
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
