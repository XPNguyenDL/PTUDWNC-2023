import "./App.scss";
import "bootstrap/dist/css/bootstrap.min.css";
import { BrowserRouter, Route, Routes } from "react-router-dom";
import { Footer, Dashboard } from "./Components";
import {
  Post,
  About,
  Contact,
  Home,
  Layout,
  PostByAuthor,
  PostByCategory,
  Rss,
  AdminLayout,
  Author,
  Category,
  Comment,
  Post as PostAdmin,
  Tags,
  Subscriber,
  NotFound,
  PostEdit,
  BadRequest
} from "./Pages";

function App() {
  return (
    <>
      <BrowserRouter>
        <Routes>
          <Route path="/" element={<Layout />}>
            <Route path="/" element={<Home />} />
            <Route path="/blog" element={<Home />} />
            <Route path="/blog/contact" element={<Contact />} />
            <Route path="/blog/about" element={<About />} />
            <Route path="/blog/rss" element={<Rss />} />
            <Route path="blog/author/:slug" element={<PostByAuthor />} />
            <Route path="blog/category/:slug" element={<PostByCategory />} />
            <Route path="blog/post/:slug" element={<Post />} />
            <Route path="blog/subscriber" element={<Subscriber />} />
          </Route>
          <Route path="/admin" element={<AdminLayout />}>
            <Route path="/admin" element={<Dashboard />} />
            <Route path="/admin/dashboard" element={<Dashboard />} />
            <Route path="/admin/authors" element={<Author />} />
            <Route path="/admin/categories" element={<Category />} />
            <Route path="/admin/posts" element={<PostAdmin />} />
            <Route path="/admin/posts/edit" element={<PostEdit />} />
            <Route path="/admin/posts/edit/:id" element={<PostEdit />} />
            <Route path="/admin/comments" element={<Comment />} />
            <Route path="/admin/tags" element={<Tags />} />
          </Route>
          <Route path="*" element={<NotFound />} />
          <Route path="/400" element={<BadRequest />} />
        </Routes>
        <Footer />
      </BrowserRouter>
    </>
  );
}

export default App;
