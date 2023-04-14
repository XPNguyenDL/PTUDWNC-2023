import React from "react";
import { Link, Navigate, useParams } from "react-router-dom";
import {
  addTag,
  getTagById,
  updateTag
} from "../../../Services/TagsRepository";
import { useEffect } from "react";
import { useState } from "react";
import { isGuid } from "../../../Components/Utils";
import { Button, Form } from "react-bootstrap";

const initialState = {
  name: "",
  urlSlug: "",
  description: ""
};

export default function TagEdit() {
  const params = useParams();

  // useState
  const [tag, setTag] = useState(initialState);
  const [shouldNavigate, setShouldNavigate] = useState(false);

  useEffect(() => {
    fetchTag();
    async function fetchTag() {
      const data = await getTagById(params.id);
      if (data) {
        setTag(data);
      } else setTag({});
    }
  }, []);

  const handleSubmit = (e) => {
    e.preventDefault();
    const data = {
      name: tag.name,
      urlSlug: tag.urlSlug,
      description: tag.description
    };

    if (params.id) {
      UpdateTag(data);
    } else {
      AddTag(data);
    }

    async function UpdateTag(data) {
      const res = await updateTag(params.id, data);
      if (res) {
        alert("Cập nhập thành công");
        setShouldNavigate(true);
      } else {
        console.log(res);
        alert("Cập nhập thất bại");
      }
    }
    async function AddTag(data) {
      const res = await addTag(data);
      if (res) {
        alert("Thêm thành công");
      } else {
        alert("Thêm thất bại");
      }
    }
  };

  if (params.id && !isGuid(params.id)) {
    return <Navigate to={`/400?redirectTo=/admin/tags`} />;
  }

  if (shouldNavigate) {
    return <Navigate to="/admin/tags" />;
  }

  return (
    <>
      <Form
        method={isGuid(params.id) ? "put" : "post"}
        onSubmit={handleSubmit}>
        <div className="row mb-3">
          <Form.Label className="col-sm-2 col-form-label">
            Tên thẻ
          </Form.Label>
          <div className="col-sm-10">
            <Form.Control
              type="text"
              name="name"
              title="name"
              required
              value={tag.name || ""}
              onChange={(e) =>
                setTag({
                  ...tag,
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
              value={tag.urlSlug || ""}
              onChange={(e) =>
                setTag({
                  ...tag,
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
              value={tag.description || ""}
              onChange={(e) =>
                setTag({
                  ...tag,
                  description: e.target.value
                })
              }
            />
          </div>
        </div>
        <div className="text-center">
          <Button variant="primary" type="submit">
            Lưu các thay đổi
          </Button>

          <Link to="/admin/tags" className="btn btn-danger ms-2">
            Hủy và quay lại
          </Link>
        </div>
      </Form>
    </>
  );
}
