import React from "react";
import { useState, useRef } from "react";
import { getCategories } from "../../../Services/CategoryRepository";
import { getAuthors } from "../../../Services/AuthorRepository";
import { Button, Form } from "react-bootstrap";
import { Link } from "react-router-dom";
import { useEffect } from "react";

export default function PostsFilterPane({
  setKeyword,
  setAuthorSlug,
  setCategorySlug
}) {
  const [authors, setAuthors] = useState([]);
  const [categories, setCategories] = useState([]);

  const keywordRef = useRef();
  const authorRef = useRef();
  const categoryRef = useRef();

  const handleFilterPosts = (e) => {
    e.preventDefault();
    setKeyword(keywordRef.current.value);
    setAuthorSlug(authorRef.current.value);
    setCategorySlug(categoryRef.current.value);
  };

  const handleClearFilter = () => {
    setKeyword("");
    setAuthorSlug("");
    setCategorySlug("");
    keywordRef.current.value = "";
    authorRef.current.value = "";
    categoryRef.current.value = "";
  };

  useEffect(() => {
    fetchData();
    async function fetchData() {
      const authors = await getAuthors();
      if (authors) setAuthors(authors);
      const categories = await getCategories();
      if (categories) setCategories(categories);
    }
  }, []);

  return (
    <Form
      method="get"
      onSubmit={handleFilterPosts}
      className="row gx-3 gy-2 align-items-center py-2">
      <Form.Group className="col-auto">
        <Form.Label className="visually-hidden">Từ khóa</Form.Label>
        <Form.Control
          ref={keywordRef}
          type="text"
          placeholder="Nhập từ khóa..."
          name="keyword"
        />
      </Form.Group>
      <Form.Group className="col-auto">
        <Form.Label className="visually-hidden">Tác giả</Form.Label>
        <Form.Select ref={authorRef} title="Tác giả" name="authorId">
          <option value="">-- Chọn tác giả --</option>
          {authors.map((author) => (
            <option key={author.id} value={author.urlSlug}>
              {author.fullName}
            </option>
          ))}
        </Form.Select>
      </Form.Group>
      <Form.Group className="col-auto">
        <Form.Label className="visually-hidden">Chủ đề</Form.Label>
        <Form.Select ref={categoryRef} title="Chủ đề" name="categoryId">
          <option value="">-- Chọn chủ đề --</option>
          {categories.map((category) => (
            <option key={category.id} value={category.urlSlug}>
              {category.name}
            </option>
          ))}
        </Form.Select>
      </Form.Group>
      <Form.Group className="col-auto">
        <Button variant="primary" type="submit">
          Tìm/Lọc
        </Button>
        <Button variant="warning mx-2" onClick={handleClearFilter}>
          Bỏ lọc
        </Button>
        <Link to="/admin/posts/edit" className="btn btn-success">
          Thêm mới
        </Link>
      </Form.Group>
    </Form>
  );
}
