import React, { useRef } from "react";
import { useState } from "react";
import { useEffect } from "react";
import { Link, Navigate, useParams } from "react-router-dom";
import {
  addCategory,
  getCategoryById,
  updateCategory
} from "../../../Services/CategoryRepository";
import { Button, Form } from "react-bootstrap";
import { isGuid } from "../../../Components/Utils";

const initialState = {
  name: "string",
  urlSlug: "string",
  description: "string",
  showOnMenu: true
};

export default function CategoryEdit() {
  const params = useParams();

  // useState
  const [category, setCategory] = useState(initialState);
  const [shouldNavigate, setShouldNavigate] = useState(false);

  // useEffects
  useEffect(() => {
    fetchCategories();
    async function fetchCategories() {
      const data = await getCategoryById(params.id);
      if (data) {
        setCategory(data);
      } else setCategory([]);
    }
  }, []);

  // handle
  const handleSubmit = (e) => {
    e.preventDefault();
    const data = {
      name: category.name,
      urlSlug: category.urlSlug,
      description: category.description,
      showOnMenu: category.showOnMenu
    };

    if (params.id) {
      UpdateCategory(data);
    } else {
      AddCategory(data);
    }

    async function UpdateCategory(data) {
      const res = await updateCategory(params.id, data);
      console.log(res);
      if (res) {
        alert("Cập nhập thành công");
        setShouldNavigate(true);
      } else {
        console.log(res);
        alert("Cập nhập thất bại");
      }
    }

    async function AddCategory(data) {
      const res = await addCategory(data);
      if (res) {
        alert("Thêm thành công");
      } else {
        alert("Thêm thất bại");
      }
    }
  };

  if (params.id && !isGuid(params.id)) {
    return <Navigate to={`/400?redirectTo=/admin/categories`} />;
  }

  if (shouldNavigate) {
    return <Navigate to="/admin/categories" />;
  }

  return (
    <>
      <Form
        method={isGuid(params.id) ? "put" : "post"}
        encType="multipart/form-data"
        onSubmit={handleSubmit}>
        <div className="row mb-3">
          <Form.Label className="col-sm-2 col-form-label">
            Tên chủ đề
          </Form.Label>
          <div className="col-sm-10">
            <Form.Control
              type="text"
              name="title"
              title="Title"
              required
              value={category.name || ""}
              onChange={(e) =>
                setCategory({
                  ...category,
                  name: e.target.value
                })
              }
            />
          </div>
        </div>
        <div className="row mb-3">
          <Form.Label className="col-sm-2 col-form-label">UrlSlug</Form.Label>
          <div className="col-sm-10">
            <Form.Control
              type="text"
              name="slug"
              title="Slug"
              required
              value={category.urlSlug || ""}
              onChange={(e) =>
                setCategory({
                  ...category,
                  urlSlug: e.target.value
                })
              }
            />
          </div>
        </div>
        <div className="row mb-3">
          <Form.Label className="col-sm-2 col-form-label">Mô tả</Form.Label>
          <div className="col-sm-10">
            <Form.Control
              type="text"
              name="description"
              title="Description"
              required
              value={category.description || ""}
              onChange={(e) =>
                setCategory({
                  ...category,
                  description: e.target.value
                })
              }
            />
          </div>
        </div>

        <div className="row mb-3">
          <div className="col-sm-10 offset-sm-2">
            <div className="form-check">
              <input
                className="form-check-input"
                type="checkbox"
                name="showOnMenu"
                checked={category.showOnMenu}
                onChange={(e) =>
                  setCategory({ ...category, showOnMenu: e.target.checked })
                }
              />
              <Form.Label className="form-check-label">Đã xuất bản</Form.Label>
            </div>
          </div>
        </div>
        <div className="text-center">
            <Button variant="primary" type="submit">
              Lưu các thay đổi
            </Button>

          <Link to="/admin/categories" className="btn btn-danger ms-2">
            Hủy và quay lại
          </Link>
        </div>
      </Form>
    </>
  );
}
