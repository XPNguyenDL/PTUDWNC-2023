import React from "react";
import { Button } from "react-bootstrap";
import { Link } from "react-router-dom";

export default function Pager({ postQuery, metadata }) {
  const pageCount = metadata.pageCount,
    hasNextPage = metadata.hasNextPage,
    hasPreviousPage = metadata.hasPreviousPage,
    pageNumber = metadata.pageNumber,
    pageSize = metadata.pageSize,
    actionName = "",
    slug = "",
    keyword = postQuery.keyword ?? "";

  if (pageCount > 1) {
    return (
      <div className="text-center my-4">
        {hasPreviousPage ? (
          <Link
            to={`/blog/${actionName}?slug=${slug}&k=${keyword}&p=${
              pageNumber - 1
            }&ps=${pageSize}`}
            className="btn btn-info me-4">
            &nbsp;Trang trước
          </Link>
        ) : (
          <Button variant="outline-secondary me-4" disable>
            &nbsp;Trang trước
          </Button>
        )}
        {hasNextPage ? (
          <Link
            to={`/blog/${actionName}?slug=${slug}&k=${keyword}&p=${
              pageNumber + 1
            }&ps=${pageSize}`}
            className="btn btn-info">
            &nbsp;Trang sau
          </Link>
        ) : (
          <Button variant="outline-secondary" disable>
            &nbsp;Trang sau
          </Button>
        )}
      </div>
    );
  }

  return <div>Pager</div>;
}
