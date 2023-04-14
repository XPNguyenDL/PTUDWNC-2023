import React, { useRef } from "react";
import { useState } from "react";
import { useEffect } from "react";
import { Link, Navigate, useParams } from "react-router-dom";
import {
  addPost,
  getPostById,
  updatePost,
  updatePostPicture
} from "../../Services/BlogRepository";
import { API_URL, isEmptyOrSpaces, isGuid } from "../../Components/Utils";
import { Button, Form } from "react-bootstrap";
import { getAuthors } from "../../Services/AuthorRepository";
import { getCategories } from "../../Services/CategoryRepository";

const initialState = {
  title: "",
  shortDescription: "",
  description: "",
  meta: "",
  urlSlug: "",
  category: {
    id: ""
  },
  author: {
    id: ""
  },
  tags: [],
  categoryId: "",
  authorId: "",
  selectedTags: "",
  published: false
};

export default function PostEdit() {
  const params = useParams();

  // useState

  const [post, setPost] = useState(initialState);
  const [categories, setCategories] = useState([]);
  const [authors, setAuthors] = useState([]);
  const imageFile = useRef();

  // useEffects
  useEffect(() => {
    fetchPosts();
    async function fetchPosts() {
      const data = await getPostById(params.id);
      if (data) {
        setPost(data);
      } else setPost([]);
      const authors = await getAuthors();
      if (authors) setAuthors(authors);
      const categories = await getCategories();
      if (categories) setCategories(categories);
    }
  }, []);

  if (params.id && !isGuid(params.id)) {
    return <Navigate to={`/400?redirectTo=/admin/posts`} />;
  }

  // handle
  const handleSubmit = (e) => {
    e.preventDefault();
    console.log(post);
    const data = {
      title: post.title,
      shortDescription: post.shortDescription,
      description: post.description,
      meta: post.meta,
      urlSlug: post.urlSlug,
      categoryId: post.categoryId ? post.categoryId : post.category.id,
      authorId: post.authorId ? post.authorId : post.author.id,
      selectedTags: post.selectedTags?.split("\n"),
      published: post.published
    };

    console.log(data);
    const file = imageFile.current.files[0];
    console.log(file);
    if (params.id) {
      UpdatePost(data);
    } else {
      AddPost(data);
    }

    async function UpdatePost(data) {
      const res = await updatePost(params.id, data);
      console.log(res);
      if (res) {
        alert("Cập nhập thành công");
      } else {
        console.log(res);
        alert("Cập nhập thất bại");
      }
      const resImage = await updatePostPicture(res.id, file)
      if (resImage) {
        alert("Cập nhập hình ảnh thành công");
      } else {
        console.log(resImage);
        alert("Cập nhập hình ảnh thất bại");
      }
    }

    async function AddPost(data) {
      const res = await addPost(data);
      if (res) {
        alert("Thêm thành công");
      } else {
        alert("Thêm thất bại");
      }
    }
  };

  return (
    <>
      <Form
        method={isGuid(params.id) ? "put" : "post"}
        encType="multipart/form-data"
        onSubmit={handleSubmit}>
        <div className="row mb-3">
          <Form.Label className="col-sm-2 col-form-label">Tiêu đề</Form.Label>
          <div className="col-sm-10">
            <Form.Control
              type="text"
              name="title"
              title="Title"
              required
              value={post.title || ""}
              onChange={(e) =>
                setPost({
                  ...post,
                  title: e.target.value
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
              value={post.urlSlug || ""}
              onChange={(e) =>
                setPost({
                  ...post,
                  urlSlug: e.target.value
                })
              }
            />
          </div>
        </div>
        <div className="row mb-3">
          <Form.Label className="col-sm-2 col-form-label">
            Giới thiệu
          </Form.Label>
          <div className="col-sm-10">
            <Form.Control
              type="text"
              name="shortDescription"
              title="Short Description"
              required
              value={post.shortDescription || ""}
              onChange={(e) =>
                setPost({
                  ...post,
                  shortDescription: e.target.value
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
              value={post.description || ""}
              onChange={(e) =>
                setPost({
                  ...post,
                  description: e.target.value
                })
              }
            />
          </div>
        </div>
        <div className="row mb-3">
          <Form.Label className="col-sm-2 col-form-label">Metadata</Form.Label>
          <div className="col-sm-10">
            <Form.Control
              type="text"
              name="meta"
              title="Meta"
              required
              value={post.meta || ""}
              onChange={(e) =>
                setPost({
                  ...post,
                  meta: e.target.value
                })
              }
            />
          </div>
        </div>
        <div className="row mb-3">
          <Form.Label className="col-sm-2 col-form-label">Tác giả</Form.Label>
          <div className="col-sm-10">
            <Form.Select
              name="authorId"
              title="AuthorId"
              value={post.authorId ? post.authorId : post.author?.id}
              required
              onChange={(e) =>
                setPost({
                  ...post,
                  authorId: e.target.value
                })
              }>
              <option value="">-- Chọn tác giả --</option>
              {authors.map((author) => (
                <option key={author.id} value={author.id}>
                  {author.fullName}
                </option>
              ))}
            </Form.Select>
          </div>
        </div>
        <div className="row mb-3">
          <Form.Label className="col-sm-2 col-form-label">Chủ đề</Form.Label>
          <div className="col-sm-10">
            <Form.Select
              name="categoryId"
              title="CategoryId"
              value={post.categoryId ? post.categoryId : post.category?.id}
              required
              onChange={(e) =>
                setPost({
                  ...post,
                  categoryId: e.target.value
                })
              }>
              <option value="">-- Chọn chủ đề --</option>
              {categories.map((category) => (
                <option key={category.id} value={category.id}>
                  {category.name}
                </option>
              ))}
            </Form.Select>
          </div>
        </div>
        <div className="row mb-3">
          <Form.Label className="col-sm-2 col-form-label">Từ khóa</Form.Label>
          <div className="col-sm-10">
            <Form.Control
              type="text"
              as="textarea"
              name="selectTags"
              title="SelectTag"
              required
              value={post.selectedTags && post.tags?.map((item) => item.name).join("\n")}
              onChange={(e) =>
                setPost({
                  ...post,
                  selectedTags: e.target.value,
                  tags: e.target.value.split("\n").map((name) => ({ name }))
                })
              }
            />
          </div>
        </div>
        {!isEmptyOrSpaces(post.imageUrl) && (
          <div className="row mb-3">
            <Form.Label className="col.sm-2 col-form-label">
              Hình hiện tại
            </Form.Label>
            <div className="col-sm-10">
              <img src={`${API_URL}/${post.imageUrl}`} alt="" />
            </div>
          </div>
        )}
        <div className="row mb-3">
          <Form.Label className="col.sm-2 col-form-label">
            Chọn hình ảnh
          </Form.Label>
          <div className="col-sm-10">
            <Form.Control
              type="file"
              name="imageFile"
              accept="image/*"
              title="Image File"
              ref={imageFile}
              onChange={(e) =>
                setPost({ ...post, imageFile: e.target.files[0] })
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
                name="published"
                checked={post.published}
                onChange={(e) =>
                  setPost({ ...post, published: e.target.checked })
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
          <Link to="/admin/posts" className="btn btn-danger ms-2">
            Hủy và quay lại
          </Link>
        </div>
      </Form>
    </>
  );
}
