import React from "react";
import isEmptyOrSpaces from "../Utils";
import { Card } from "react-bootstrap";
import TagList from "../TagList";
import { Link } from "react-router-dom";

export default function PostItem({ postItem }) {
  const url = "https://localhost:7058/"

  const imageUrl = isEmptyOrSpaces(postItem.imageUrl)
    ? process.env.PUBLIC_URL + `/images/image_1.jpg`
    : `${url}${postItem.imageUrl}`;

  return (
    <article className="blog-entry mb-4">
      <Card>
        <div className="row g-0">
          <div className="col-md-4">
            <Card.Img variant="top" src={imageUrl} alt={postItem.title} />
          </div>
          <div className="col-md-8">
            <Card.Body>
                <Card.Title>{postItem.title}</Card.Title>
                <Card.Text>
                    <small className="text-muted">Tác giả:</small>
                    <span className="text-primary m-1">
                        {postItem.author.fullName}
                    </span>
                    <small className="text-muted">Chủ đề:</small>
                    <span className="text-primary m-1">
                        {postItem.category.name}
                    </span>
                </Card.Text>
                <Card.Text>
                    {postItem.shortDescription}
                </Card.Text>
                <div className="tag-list">
                    <TagList tagList={postItem.tags} />
                </div>
                <div className="text-end">
                    <Link
                        to={`/blog/post?id=${postItem.id}`}
                        className="btn btn-primary"
                        title={postItem.title}
                        >
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
