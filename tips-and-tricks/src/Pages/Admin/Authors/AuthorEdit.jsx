import React from "react";
import { useEffect } from "react";
import { useState } from "react";
import { Button, Form } from "react-bootstrap";
import { Link, Navigate, useParams } from "react-router-dom";
import { addAuthor, getAuthorById, updateAuthor } from "../../../Services/AuthorRepository";
import { isGuid } from "../../../Components/Utils";

const initialState = {
  fullName: "",
  urlSlug: "",
  joinedDate: "",
  email: "",
  notes: "",
};

export default function AuthorEdit() {
  const params = useParams();

  // useState
  const [author, setAuthor] = useState(initialState);
  const [shouldNavigate, setShouldNavigate] = useState(false);

  // useEffects
  useEffect(() => {
    fetchAuthors();
    async function fetchAuthors() {
      const data = await getAuthorById(params.id);
      if (data) {
        setAuthor(data);
      } else setAuthor([]);
    }
  }, []);

  // handle
  const handleSubmit = (e) => {
    e.preventDefault();
    const data = {
      fullName: author.fullName,
      urlSlug: author.urlSlug,
      joinedDate: author.joinedDate,
      email: author.email,
      notes: author.notes || "",
    };
    console.log(data);

    if (params.id) {
      UpdateAuthor(data);
    } else {
      AddAuthor(data);
    }

    async function UpdateAuthor(data) {
      const res = await updateAuthor(params.id, data);
      if (res) {
        alert("Cập nhập thành công");
        setShouldNavigate(true);
      } else {
        alert("Cập nhập thất bại");
      }
    }

    async function AddAuthor(data) {
      const res = await addAuthor(data);
      if (res) {
        alert("Thêm thành công");
        setShouldNavigate(true);
      } else {
        alert("Thêm thất bại");
      }
    }
  };

  if (params.id && !isGuid(params.id)) {
    return <Navigate to={`/400?redirectTo=/admin/authors`} />;
  }

  if (shouldNavigate) {
    return <Navigate to="/admin/authors" />;
  }

  return (
    <>
      <Form
        method={isGuid(params.id) ? "put" : "post"}
        onSubmit={handleSubmit}>
        <div className="row mb-3">
          <Form.Label className="col-sm-2 col-form-label">
            Tên tác giả
          </Form.Label>
          <div className="col-sm-10">
            <Form.Control
              type="text"
              name="title"
              title="Title"
              required
              value={author.fullName}
              onChange={(e) =>
                setAuthor({
                  ...author,
                  fullName: e.target.value
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
              value={author.urlSlug || ""}
              onChange={(e) =>
                setAuthor({
                  ...author,
                  urlSlug: e.target.value
                })
              }
            />
          </div>
        </div>
        <div className="row mb-3">
          <Form.Label className="col-sm-2 col-form-label">Email</Form.Label>
          <div className="col-sm-10">
            <Form.Control
              type="text"
              name="description"
              title="Description"
              required
              value={author.email || ""}
              onChange={(e) =>
                setAuthor({
                  ...author,
                  email: e.target.value
                })
              }
            />
          </div>
        </div>
        <div className="row mb-3">
          <Form.Label className="col-sm-2 col-form-label">Mô tả</Form.Label>
          <div className="col-sm-10">
            <Form.Control
              type="textarea"
              name="description"
              title="Description"
              value={author.notes || ""}
              onChange={(e) =>
                setAuthor({
                  ...author,
                  notes: e.target.value
                })
              }
            />
          </div>
        </div>

        <div className="text-center">
          <Button variant="primary" type="submit">
            Lưu các thay đổi
          </Button>
          <Link to="/admin/authors" className="btn btn-danger ms-2">
            Hủy và quay lại
          </Link>
        </div>
      </Form>
    </>
  );
}
