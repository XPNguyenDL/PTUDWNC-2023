import React from "react";
import isEmptyOrSpaces, { API_URL } from "./Utils";
import { Card } from "react-bootstrap";
import TagList from "./TagList";
import { Link } from "react-router-dom";

export default function PostItem({ postItem }) {
  const imageUrl = isEmptyOrSpaces(postItem.imageUrl)
    ? process.env.PUBLIC_URL + `/images/image_1.jpg`
    : `${API_URL}/${postItem.imageUrl}`;

  return (
    <article className="blog-entry mb-4">
      <Card>
        <div className="row g-0">
          <div className="col-md-4">
            <Card.Img variant="top" src={imageUrl} alt={postItem.title} />
          </div>
          <div className="col-md-8">
            <Card.Body>
              <Card.Title>
                <Link
                  to={`/blog/post/${postItem.urlSlug}`}
                  className="text-primary text-decoration-none m-1">
                  {postItem.title}
                </Link>
              </Card.Title>
              <Card.Text>
                <small className="text-muted">Tác giả:</small>
                <Link
                  to={`/blog/author/${postItem.author.urlSlug}`}
                  className="text-primary text-decoration-none m-1">
                  {postItem.author.fullName}
                </Link>
                <small className="text-muted">Chủ đề:</small>
                <Link
                  to={`/blog/category/${postItem.category.urlSlug}`}
                  className="text-primary text-decoration-none m-1">
                  {postItem.category.urlSlug}
                </Link>
              </Card.Text>
              <Card.Text>{postItem.shortDescription}</Card.Text>
              <div className="tag-list">
                <TagList tagList={postItem.tags} />
              </div>
              <div className="text-end">
                <Link
                  to={`/blog/post/${postItem.urlSlug}`}
                  className="btn btn-primary"
                  title={postItem.title}>
                  Xem chi tiết
                </Link>
              </div>
            </Card.Body>
          </div>
        </div>
      </Card>
    </article>
  );
}
