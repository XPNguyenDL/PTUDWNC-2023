import React from "react";
import { useRef } from "react";
import { Button, Form } from "react-bootstrap";
import { Link } from "react-router-dom";

export default function CategoriesFilterPane({ setKeyword }) {


    const keywordRef = useRef();
  
    const handleFilterPosts = (e) => {
      e.preventDefault();
      setKeyword(keywordRef.current.value);
    };
  
    const handleClearFilter = () => {
      setKeyword("");
      keywordRef.current.value = "";
    };

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
        <Button variant="primary" type="submit">
          Tìm/Lọc
        </Button>
        <Button variant="warning mx-2" onClick={handleClearFilter}>
          Bỏ lọc
        </Button>
        <Link to="/admin/categories/edit" className="btn btn-success">
          Thêm mới
        </Link>
      </Form.Group>
    </Form>
  );
}
